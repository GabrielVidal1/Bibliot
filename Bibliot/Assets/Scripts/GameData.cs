using System;
using System.Collections.Generic;

public static partial class SaverLoader
{
    [Serializable]
    public class GameData
    {
        [Serializable]
        public class BookStorage
        {
            public enum BuildingType
            {
                Bookshelf,
                Checkout
            }

            public BuildingType Type;
            
            public Serializables.Vector3 Position;
            public Serializables.Quaternion Rotation;
            public List<int> BookIds;

            public bool DropOffCheckout;
        }

        public List<BookStorage> BookStorages;

        public List<int> TransportedBooks;

        public List<Client> Clients;
        
        [Serializable]
        public class Client
        {
            public Serializables.Vector3 Position;

            public List<int> WantedBooks;
            public List<int> Books;

            public global::Client.ClientState State;
        }
    }
}