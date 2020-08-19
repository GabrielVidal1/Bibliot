using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Book bookToAdd;

    public void AddBook()
    {
        GameManager.gm.Bookshelfs[0].AddBook(BookModel.Instantiate(bookToAdd));
    }
    
    public void Client()
    {
        Client client = Instantiate(GameManager.gm.pm.ClientPrefab, GameManager.gm.Entrance.position, Quaternion.identity);
        client.Init();
        client.WantedBooks.Add(bookToAdd);
    }
}
