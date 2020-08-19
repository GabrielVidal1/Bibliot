using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookModel : MonoBehaviour
{
    public static BookModel Instantiate(Book book)
    {
        BookModel bookModel = Instantiate(GameManager.gm.pm.BookModelPrefab);
        bookModel.Initialize(book);
        bookModel.cover.material.color = book.coverColor;
        bookModel.transform.localScale = new Vector3(1, 1, book.thickness);
        return bookModel;
    }
    
    public Book book;

    public MeshRenderer cover;
    
    public void Initialize(Book book)
    {
        this.book = book;
    }
}
