using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleavePerk : PerkBase
{
    public override void InitializePerk(UnitBase unit)
    {
       unit.IsIgnoreArmor = true;
    }

	public override void UsePerk(IDamagable unit1, UnitBase unit2)
	{
	
	}
}
