using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Client : BookStorage
{
    public enum ClientState
    {
        NeedingBooks,
        Gone,
        DroppingBooksOff
    }

    public ClientState State;
    
    public List<Book> WantedBooks;

    [SerializeField] private GameObject mesh;
    
    
    private NavMeshAgent agent;

    private Checkout targetCheckout;

    public Client NextClient = null;
    public Client PreviousClient = null; 
    
    
    public void Init()
    {
        State = ClientState.NeedingBooks;
        
        GameManager.gm.Clients.Add(this);
        
        agent = GetComponent<NavMeshAgent>();
        targetCheckout = Utility.RandomElement(GameManager.gm.Checkouts);

        AreBooksReady = false;
        
        if (targetCheckout.Client == null)
        {
            targetCheckout.SetClient(this);
        }
        else
        {
            Client parent = targetCheckout.Client;
            while (parent.NextClient != null)
                parent = parent.NextClient;

            parent.NextClient = this;
            PreviousClient = parent;

        }
        
        StartCoroutine(Cycle());
    }
    
    public bool AreBooksReady;
    
    IEnumerator Cycle()
    {
        if (PreviousClient != null)
        {
            agent.isStopped = true;
            while (PreviousClient != null)
                yield return new WaitForSeconds(0.5f);
        }

        agent.isStopped = false;

        agent.SetDestination(targetCheckout.waitingPosition.position);
        while (Vector3.SqrMagnitude(transform.position - targetCheckout.waitingPosition.position) > 0.1f)
            yield return new WaitForSeconds(0.5f);

        while (!AreBooksReady)
            yield return new WaitForSeconds(0.5f);

        foreach (Book wantedBook in WantedBooks)
        {
            for (int i = 0; i < targetCheckout.books.Count; i++)
            {
                BookModel bookModel = targetCheckout.books[i];
                if (bookModel.book == wantedBook)
                {
                    targetCheckout.books.RemoveAt(i);
                    AddBook(bookModel);
                }
                else
                {
                    i++;
                }
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        WantedBooks.Clear();

        targetCheckout.FinishCommand();

        if (NextClient != null)
        {
            NextClient.PreviousClient = null;
            targetCheckout.SetClient(NextClient);
        }
        
        State = ClientState.Gone;


        agent.SetDestination(GameManager.gm.Exit.position);
        while (Vector3.SqrMagnitude(transform.position - GameManager.gm.Exit.position) > 0.5f)
            yield return new WaitForSeconds(0.5f);

        mesh.SetActive(false);
        agent.isStopped = true;

        
        yield return new WaitForSeconds(Random.value * 3f);

        State = ClientState.DroppingBooksOff;

        
        transform.position = GameManager.gm.Entrance.position;
        mesh.SetActive(true);
        agent.isStopped = false;

        Checkout dropOffCheckout = Utility.RandomElement(GameManager.gm.DropOffCheckouts);
        
        agent.SetDestination(dropOffCheckout.waitingPosition.position);
        while (Vector3.SqrMagnitude(transform.position - dropOffCheckout.waitingPosition.position) > 0.1f)
            yield return new WaitForSeconds(0.5f);



        for (int i = 0; i < books.Count; i++)
        {
            if (!dropOffCheckout.AddBook(books[0]))
                yield return new WaitForSeconds(1f);
            else
                yield return new WaitForSeconds(0.2f);
                
            books.RemoveAt(0);
        }
        
        agent.SetDestination(GameManager.gm.Exit.position);
        while (Vector3.SqrMagnitude(transform.position - GameManager.gm.Exit.position) > 0.5f)
            yield return new WaitForSeconds(0.5f);
        
        GameManager.gm.Clients.Remove(this);
        Destroy(gameObject, 1f);
    }

}
