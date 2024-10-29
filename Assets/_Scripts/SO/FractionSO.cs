using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FractionSO", menuName = "ScriptableObjects/fraction", order = 5)]
public class FractionSO : ScriptableObject
{
    public string Description;

    public List<UnitBase> ArmyList = new List<UnitBase>();
    //public BuildingSO castle;
    public List<SpellSO> AbilityList = new List<SpellSO>();
    //public UnitDataSO[] UnitDatalist = new UnitDataSO[5];

}
