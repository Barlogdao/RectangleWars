using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "ScriptableObjects/Buildings", order = 1)]
public class BuildingSO : ScriptableObject
{
    public int Health;
    public int Armor;
    public int Attack;
    [Multiline]
    public string OnDestroyText;
    public Sprite Sprite;
    public Color ActiveColor;
    public Color DisabledColor;
}
