using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utility
{
    public static T RandomElement<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count - 1)];
    }

    public static Vector3 ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0f);
    }

    public static void Execute<T>(List<T> list, Func<T, bool> predicate, Action<T> function)
    {
        for (int i = 0; i < list.Count;)
        {
            if (predicate(list[i]))
                function(list[i]);
            else
                i++;
        }
    }

    
}