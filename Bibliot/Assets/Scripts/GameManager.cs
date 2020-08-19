using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    
    private void Awake()
    {
        if (gm == null)
            gm = this;
        else if (gm != this)
            Destroy(gameObject);

        gm.pm = GetComponent<PrefabManager>();
        gm.bm = GetComponent<BookManager>();
        
        SaverLoader.Load();
        gm.Init();
    }

    public PrefabManager pm;
    public BookManager bm;

    public List<Checkout> Checkouts;
    public List<Checkout> DropOffCheckouts;

    public List<Bookshelf> Bookshelfs;

    public List<Client> Clients;

    public Transform Entrance;
    public Transform Exit;

    public PlayerTransport PlayerTransport;

    public SaverLoader.GameData CurrentSave;


    public void Save()
    {
        CurrentSave.BookStorages = Bookshelfs.Select(bookshelf => new SaverLoader.GameData.BookStorage()
        {
            Position = bookshelf.transform.position,
            Rotation = bookshelf.transform.rotation,
            Type = SaverLoader.GameData.BookStorage.BuildingType.Bookshelf,
            BookIds = bookshelf.books.Select(bookModel => bookModel.book.Id).ToList(),
        }).Concat(Checkouts.Concat(DropOffCheckouts).Select(checkout => new SaverLoader.GameData.BookStorage()
        {
            Position = checkout.transform.position,
            Rotation = checkout.transform.rotation,
            Type = SaverLoader.GameData.BookStorage.BuildingType.Checkout,
            BookIds = checkout.books.Select(bookModel => bookModel.book.Id).ToList(),
            DropOffCheckout = checkout.dropOffCheckout
        })).ToList();
            
        CurrentSave.TransportedBooks = PlayerTransport.books.Select(model => model.book.Id).ToList();

        CurrentSave.Clients = Clients.Select(client => new SaverLoader.GameData.Client()
        {
            State = client.State,
            Position = client.transform.position,
            Books = client.books.Select(book => book.book.Id).ToList(),
            WantedBooks = client.WantedBooks.Select(book => book.Id).ToList()
        }).ToList();
        
        SaverLoader.Save();
    }

    private void Init()
    {
        foreach (SaverLoader.GameData.BookStorage bookStorage in CurrentSave.BookStorages)
        {
            BookStorage bookStorageInstance = null;
            switch (bookStorage.Type)
            {
                case SaverLoader.GameData.BookStorage.BuildingType.Bookshelf:
                    bookStorageInstance = Instantiate(pm.BookshelfPrefab, bookStorage.Position, bookStorage.Rotation);
                    break;
                case SaverLoader.GameData.BookStorage.BuildingType.Checkout:
                    bookStorageInstance = Instantiate(pm.CheckoutPrefab, bookStorage.Position, bookStorage.Rotation);
                    ((Checkout) bookStorageInstance).dropOffCheckout = bookStorage.DropOffCheckout;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (int bookId in bookStorage.BookIds)
            {
                BookModel bookModel = BookModel.Instantiate(bm.Books[bookId]);
                bookStorageInstance.AddBook(bookModel, true);
            }
            
            bookStorageInstance.Init();
        }

        foreach (int bookId in CurrentSave.TransportedBooks)
        {
            BookModel bookModel = BookModel.Instantiate(bm.Books[bookId]);
            PlayerTransport.AddBook(bookModel, true);
            PlayerTransport.booksToStore.Add(bookModel);
        }
    }
}
