using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Book", menuName = "Game/Book", order = 1)]
public class Book : ScriptableObject
{
    [HideInInspector] public int Id;

    public string Title;

    public Color coverColor;

    public float thickness;
}
