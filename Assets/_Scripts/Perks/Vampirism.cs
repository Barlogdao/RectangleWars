using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampirism : PerkBase
{
    [SerializeField]
    private int healAmount;
    public override void UsePerk(IDamagable target, UnitBase unit)
    {
        unit.Health+= healAmount;
    }
    public override string[] GetParams()
    {
        return new string[] { healAmount.ToString()};
    }
}
