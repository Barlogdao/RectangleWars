using UnityEngine;

public class Rage : PerkBase
{
    [SerializeField]
    int healthForRage;


	public override void UsePerk(IDamagable target, UnitBase unit)
	{
		if (unit.Health <= healthForRage)
			unit.CurrentAttack = Mathf.Max(unit.CurrentAttack, unit.CritAttack);
	}
    public override string[] GetParams()
    {
        return new string[] { healthForRage.ToString() };
    }
}



