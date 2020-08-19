using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkout : Interactible
{
    public Transform waitingPosition;
    
    public List<Book> neededBook;

    [SerializeField] public bool dropOffCheckout;
    
    [SerializeField] private GameObject interactObject;
    
    private void Start()
    {
        if (dropOffCheckout)    
            GameManager.gm.DropOffCheckouts.Add(this);
        else
            GameManager.gm.Checkouts.Add(this);
    }

    public override bool AddBook(BookModel bookModel, bool instantaneous = false)
    {
        if (!base.AddBook(bookModel, instantaneous))
            return false;
        neededBook.Remove(bookModel.book);
        
        if (neededBook.Count == 0)
        {
            if (Client != null)
                Client.AreBooksReady = true;
        }

        return true;
    }

    public Client Client { get; private set; }
    
    public void SetClient(Client client)
    {
        Client = client;

        interactObject.SetActive(true);
    }

    public void FinishCommand()
    {
        Client = null;
    }

    public override void Click()
    {
        if (neededBook.Count > 0) return;
        
        if (Client == null) return;
        
        interactObject.SetActive(false);
        
        neededBook.AddRange(Client.WantedBooks);

        foreach (Book book in neededBook)
            GameManager.gm.PlayerTransport.AddBookToTake(book);
    }
}