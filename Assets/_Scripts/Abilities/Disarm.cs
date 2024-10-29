using System.Collections;
using System.Linq;
using UnityEngine;

public class Disarm : AbilityBase
{
    [SerializeField] private float _duration;
    
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetEnemyUnits(player).Count(unit => unit.IsAlive) > 3; 
    }

    public override string[] GetParams(Hero hero)
    {
        return new string[] { (_duration).ToString() };
    }


    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        foreach (var unit in battlefieldManager.GetEnemyUnits(player))
        {
            if (unit.IsAlive)
            {
                unit.StartCoroutine(DisarmUnit(unit));
                RunParticle(ShowParticleOnUnit(unit), _duration);
            }
        }
    }

    private IEnumerator DisarmUnit(UnitBase unit)
    {
        unit.FightZone.FightRadius.enabled = false;
        yield return Utilis.GetWait(_duration);
        unit.FightZone.FightRadius.enabled = true;
    }


}
