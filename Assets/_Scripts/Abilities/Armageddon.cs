
using System.Linq;
using UnityEngine;

public class Armageddon : AbilityBase
{
    [SerializeField] private int _damage;
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetEnemyUnits(player).Count(unit=> unit.IsAlive) > 3;
    }
    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        foreach(var unit in battlefieldManager.GetEnemyUnits(player))
        {
            if (unit.IsAlive)
            {
                unit.GetTrueDamage(_damage);
                ShowParticleOnUnit(unit);
            }
        }
    }

    public override string[] GetParams(Hero hero)
    {
        return new string[] { (_damage).ToString() };
    }

}
