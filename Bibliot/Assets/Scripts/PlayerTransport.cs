using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransport : BookStorage
{
    public List<Book> booksToTake;
    public List<BookModel> booksToStore;

    private void Update()
    {
        if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetMouseButtonDown(0)
                ? Input.mousePosition
                : Utility.ToVector3(Input.touches[0].position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Clickable"))
                {
                    hit.collider.GetComponent<Clickable>().Click();
                }
            }
        }
    }

    public void AddBookToTake(Book book)
    {
        BookModel bookModel = booksToStore.Find(b => b.book == book);
        if (bookModel != null)
            booksToStore.Remove(bookModel);
        else
            booksToTake.Add(book);
    }
    
    #region Interaction

    public void InteractWith(Interactible interactible)
    {
        switch (interactible)
        {
            case Bookshelf bookshelf:
                InteractWith(bookshelf);
                break;
            case Checkout checkout:
                InteractWith(checkout);
                break;
        }
    }
    
    private void InteractWith(Checkout checkout)
    {
        if (checkout.dropOffCheckout)
        {
            Utility.Execute(checkout.books,
                b => true,
                b =>
                {
                    if (!checkout.TransferBook(this, b))
                        Debug.Log(
                            $"Could not transfer book '{b.book.Title}' to player because already transporting max book count");

                    if (booksToTake.Contains(b.book))
                        booksToTake.Remove(b.book);
                    else
                        booksToStore.Add(b);
                });

            return;
        }

        Utility.Execute(books,
            b => checkout.neededBook.Contains(b.book),
            b =>
            {
                if (!TransferBook(checkout, b))
                    Debug.Log($"Could not transfer book '{b.book.Title}' to {checkout.name} because checkout is full");
            });
    }
    
    private void InteractWith(Bookshelf bookshelf)
    {
        Utility.Execute(booksToTake,
            bookshelf.ContainBook,
            book =>
            {
                BookModel bookModel = bookshelf.books.Find(b => b.book == book);

                if (bookshelf.TransferBook(this, bookModel))
                    booksToTake.Remove(bookModel.book);
                else
                    Debug.Log(
                        $"Could not transfer book '{book.Title}' to player because already transporting max book count");
            });
        
        Utility.Execute(booksToStore,
            bookshelf.ShouldStoreBookIn,
            b =>
            {
                if (!TransferBook(bookshelf, b))
                    Debug.Log(
                        $"Could not transfer book '{b.book.Title}' to {bookshelf.name} because already containing max book count");
                booksToStore.Remove(b);
            });
    }
    
    #endregion
}