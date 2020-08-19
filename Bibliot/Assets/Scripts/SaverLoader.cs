using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static partial class SaverLoader
{
    private const string SaveName = "save.bibliothe";

    private static string SavePath => Path.Combine(Application.persistentDataPath, SaveName);
    
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using FileStream fileStream =
            File.Open(SavePath, FileMode.Create);
        bf.Serialize(fileStream, GameManager.gm.CurrentSave);
    }

    public static void Load()
    {
        Debug.Log(SavePath);
        
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No Save File Found");
            return;
        }
        
        BinaryFormatter bf = new BinaryFormatter();

        using FileStream fileStream = File.Open(SavePath, FileMode.Open);
        GameManager.gm.CurrentSave = (GameData) bf.Deserialize(fileStream);
    }
}