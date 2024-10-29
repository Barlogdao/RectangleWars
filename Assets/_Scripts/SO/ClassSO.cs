using UnityEngine;

[CreateAssetMenu(fileName = "ClassSO", menuName = "SO/Class")]
public class ClassSO : ScriptableObject
{
    [field:SerializeField]
    public ClassType ClassType { get;private set; }
    [field:SerializeField]
    public Sprite ClassIcon { get; private set; }
    [field:SerializeField]
    public UnitDataSO StandartUniit { get; private set; }

}
