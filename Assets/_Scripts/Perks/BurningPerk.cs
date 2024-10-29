using System.Collections;
using UnityEngine;

public class BurningPerk : PerkBase
{
    [SerializeField]
    protected int dotTime = 3;
    [SerializeField]
    protected int dotDamage = 10;

	public override void UsePerk(IDamagable damageSourse, UnitBase unit)
	{
        if (!unit.IsAlive && 
            damageSourse is UnitBase target && 
            target.IsAlive && 
            target.AttackDistance == AttackDistanceType.Melee)
        {
            target.StartCoroutine(DotDamage(target));
        }
	}
    public override string[] GetParams()
    {
        return new string[] { dotDamage.ToString(),dotTime.ToString()};
    }

    IEnumerator DotDamage(UnitBase unit)
    {
        int secAmount = dotTime;
        unit.ShaderModule.FireDoTOn();
        while (secAmount > 0 && unit.IsAlive)
        {
            yield return Utilis.GetWait(0.9f);
            unit.GetTrueDamage(dotDamage);
            secAmount--;
            yield return Utilis.GetWait(0.1f);
        }
        unit.ShaderModule.FireDoTOff();
    }
}
