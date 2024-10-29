
using UnityEngine;

[CreateAssetMenu(fileName = "StrategicObjectSO", menuName = "ScriptableObjects/StrategicObject", order = 3)]
public class StrategicObjectsSO : ScriptableObject
{
    public string Name;
    [Multiline]
    public string Description;
    public float Cooldown;
    public ResourseType resourseType;
    public ResourseSO Resource;

    public Color NormalColor;
    public Color OccupiedColor;
}
