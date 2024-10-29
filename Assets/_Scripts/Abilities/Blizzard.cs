using System.Linq;
using UnityEngine;

public class Blizzard : AbilityBase
{
    [SerializeField]
    float duration;
    [SerializeField] UnitPerksSO _stunPref;



    public override string[] GetParams(Hero hero)
    {
        return new string[] { (duration).ToString() };
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        foreach (UnitBase unit in battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive))
        {
            unit.AddPerk(_stunPref);
            ShowParticleOnUnit(unit);
        }
        base.UseAbility(battlefieldManager, player);
    }
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive && target.CanMove).Count() > 3;
    }
}
