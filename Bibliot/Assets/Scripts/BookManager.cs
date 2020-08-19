using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public List<Book> Books;

    private void Awake()
    {
        for (int i = 0; i < Books.Count; i++)
        {
            Books[i].Id = i;
        }
    }
}
