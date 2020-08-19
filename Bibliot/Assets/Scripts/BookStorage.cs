using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BookStorage : Clickable
{
    public Transform pivot;

    public List<BookModel> books;

    public int capacity = 10;

    public virtual void Init()
    {
    }

    public virtual bool AddBook(BookModel bookModel, bool instantaneous = false)
    {
        if (books.Count >= capacity)
            return false;
        books.Add(bookModel);
        bookModel.transform.SetParent(null);
        UpdateBooks(instantaneous);
        return true;
    }

    public virtual bool TransferBook(BookStorage target, BookModel bookModel)
    {
        if (!ContainBook(bookModel.book))
            throw new Exception("This storage does not contain the book " + bookModel.book.Title);

        if (!target.AddBook(bookModel))
            return false;

        books.Remove(bookModel);
        return true;
    }

    public bool ContainBook(Book book)
    {
        return books.Select(bookModel => bookModel.book).Contains(book);
    }


    protected virtual void UpdateBooks(bool instantaneous = false)
    {
        IEnumerator TakeBookCoroutine(BookModel bookModel, float y)
        {
            if (Vector3.Distance(bookModel.transform.localPosition, pivot.forward * y) < 0.01f) yield break;


            Transform bookT = bookModel.transform;
            bookT.SetParent(pivot);
            Vector3 initialPos = bookT.localPosition;
            Quaternion initialRot = bookT.localRotation;
            Vector3 finalPosition = pivot.worldToLocalMatrix.MultiplyVector(pivot.forward) * y;
            Quaternion finalRotation = Quaternion.identity;
            int k = 10;
            int i = instantaneous ? k : 0;

            for (; i <= k; i++)
            {
                bookModel.transform.localPosition = Vector3.Lerp(initialPos, finalPosition, (float) i / k);
                bookModel.transform.localRotation = Quaternion.Lerp(initialRot, finalRotation, (float) i / k);
                yield return 0;
            }
        }

        float y = 0;
        foreach (BookModel model in books)
        {
            StartCoroutine(TakeBookCoroutine(model, y));
            y += model.book.thickness;
        }
    }
}