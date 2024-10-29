using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkWalkType : PerkBase
{
	[field:SerializeField]
	public WalkType walkType { get; protected set; }
	
	public override void UsePerk(IDamagable damageSourse, UnitBase unit) { }

}
