using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newFractionList", menuName = "ScriptableObjects/FractionList")]
public class FractionList : ScriptableObject
{
    public Fraction fraction;

    public UnitDataSO Worker;

    public List<UnitDataSO> ArmyList;
    public List<SpellSO> SpellList;
}
