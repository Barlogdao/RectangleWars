using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "TileSO", menuName = "ScriptableObjects/Tiles", order = 6)]
public class TileSO : ScriptableObject
{
    [Header("�������� �����")]
    public string TileName;
   
    [Header("��� �������������")]
    public WalkType ImmuneWalkType;

    [Header("���� ������ ��������")]
    public bool IsChangeSpeed;
    [ShowIf("IsChangeSpeed")]
    public float SpeedRatio;
    
    [Header("���� �������������")]
    public bool IsStopUnit;
    [ShowIf("IsStopUnit")]
    public int ChanseToStop;
    [ShowIf("IsStopUnit")]
    public float StopTime;
   
    [Header("���� ������� ����")]
    public bool IsDealDamage;
    [ShowIf("IsDealDamage")]
    public int ChanseToDealDamage;


    [ShowIf("IsDealDamage")]
    public int Damage;
   
    [Header ("���� �����")]
    public bool IsHeal;
    [ShowIf("IsHeal")]
    public int HealAmount; 
  
    [Header("���� �������")]
    public bool IsKillUnit;
    [ShowIf("IsKillUnit")]
    public int ChanseToKill;

    [Header("���� ������ �����������")]
    public bool IsChangeDirection;
    [ShowIf("IsChangeDirection")]
    public int ChanseToChangeDirection;


}
