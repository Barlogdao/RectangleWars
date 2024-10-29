using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourseSO", menuName = "ScriptableObjects/Resourses", order = 2)]
public class ResourseSO : ScriptableObject
{
    public Sprite Image;
    public string Name;
    public string Description;
}
