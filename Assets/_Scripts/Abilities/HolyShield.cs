using System.Linq;
using UnityEngine;

public class HolyShield : AbilityBase
{

	[SerializeField]
	UnitPerksSO holyshield;

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive && !target.PerkBaseList.Contains(holyshield.Perkprefab)).Count() > 2;
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
	{
		foreach (var unit in battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive))
		{
				unit.AddPerk(holyshield);
				ShowParticleOnUnit(unit);
		}
		base.UseAbility(battlefieldManager, player);
	}
}
