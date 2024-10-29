
using UnityEngine;

public class CriticalStrike : PerkBase
{
    [SerializeField]
    private int chanse;

    public override void UsePerk(IDamagable target, UnitBase unit)
	{
        if (Utilis.Chanse(chanse))
            unit.CurrentAttack = Mathf.Max(unit.CurrentAttack, unit.CritAttack + Mathf.Max(0,unit.CurrentAttack - unit.Attack));
    }
    public override string[] GetParams()
    {
        return new string[] { chanse.ToString() };
    }
}

