using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

public class Bookshelf : Interactible
{
    public static Bookshelf Instantiate(List<Book> assignedBooks, List<BookModel> books, Vector3 position, Quaternion rotation)
    {
        Bookshelf bookshelf = Instantiate(GameManager.gm.pm.BookshelfPrefab, position, rotation);
        bookshelf.assignedBooks = assignedBooks;
        bookshelf.books = books;
        return bookshelf;
    }
    
    public List<Book> assignedBooks;

    [SerializeField] private List<Book> missingBooks;

    private void Start()
    {
        GameManager.gm.Bookshelfs.Add(this);
        
        foreach (BookModel book in books)
        {
            if (!assignedBooks.Contains(book.book))
                missingBooks.Add(book.book);
        }
    }

    public bool ShouldStoreBookIn(BookModel bookModel)
    {
        return missingBooks.Contains(bookModel.book);
    }
    
    public override bool TransferBook(BookStorage target, BookModel bookModel)
    {
        if (!base.TransferBook(target, bookModel))
            return false;

        missingBooks.Add(bookModel.book);
        
        return true;
    }

    public override bool AddBook(BookModel bookModel, bool instantaneous = false)
    {
        if (!base.AddBook(bookModel, instantaneous))
            return false;

        if (missingBooks.Contains(bookModel.book))
            missingBooks.Remove(bookModel.book);
        
        return true;
    }
}
