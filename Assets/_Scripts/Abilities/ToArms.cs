using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToArms : AbilityBase
{
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive && target.CanMove).Count() > 2;
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
       
        BattlefieldHero hero = battlefieldManager.GetEnemyBH(player);
        foreach (UnitBase unit in battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive))
        {
            if (unit.CanMove)
            {
                ShowParticleOnUnit(unit);
                unit.MoveModule.MoveTo(hero.transform.position);
            }
        }
        base.UseAbility(battlefieldManager, player);
    }
}
