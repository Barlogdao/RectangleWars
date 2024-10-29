using System.Linq;
using UnityEngine;

public class Hypnosis : AbilityBase
{
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetEnemyUnits(player).Any(target => target.IsAlive);
    }
    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        var allunits = battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive).OrderByDescending(u=> u.Data.Tier).ToList();
        if (allunits.Count > 0)
        {
            UnitBase unit = allunits[0];
            unit.transform.SetParent(player.transform);
            ShowParticleOnUnit(unit);
            base.UseAbility(battlefieldManager, player);
            
        }
        base.UseAbility(battlefieldManager, player);

    }
}
