using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "TileSO", menuName = "ScriptableObjects/Tiles", order = 6)]
public class TileSO : ScriptableObject
{
    [Header("Название Тайла")]
    public string TileName;
   
    [Header("Тип сопротивления")]
    public WalkType ImmuneWalkType;

    [Header("Тайл меняет скорость")]
    public bool IsChangeSpeed;
    [ShowIf("IsChangeSpeed")]
    public float SpeedRatio;
    
    [Header("Тайл останавливает")]
    public bool IsStopUnit;
    [ShowIf("IsStopUnit")]
    public int ChanseToStop;
    [ShowIf("IsStopUnit")]
    public float StopTime;
   
    [Header("Тайл наносит урон")]
    public bool IsDealDamage;
    [ShowIf("IsDealDamage")]
    public int ChanseToDealDamage;


    [ShowIf("IsDealDamage")]
    public int Damage;
   
    [Header ("Тайл лечит")]
    public bool IsHeal;
    [ShowIf("IsHeal")]
    public int HealAmount; 
  
    [Header("Тайл убивает")]
    public bool IsKillUnit;
    [ShowIf("IsKillUnit")]
    public int ChanseToKill;

    [Header("Тайл меняет направление")]
    public bool IsChangeDirection;
    [ShowIf("IsChangeDirection")]
    public int ChanseToChangeDirection;


}
