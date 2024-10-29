using Redcode.Extensions;
using System.Linq;
using UnityEngine;

public class RemoveEffects : AbilityBase
{
    [SerializeField]
    private SpellUnitTarget _target;
    [SerializeField]
    private PerkEffectType _removeType;



    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        switch (_target)
        {
            case SpellUnitTarget.allies:
                foreach (var unit in battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive))
                {
                    Remove(_removeType, unit);
                }
                break;
            case SpellUnitTarget.enemies:
                foreach (var unit in battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive))
                {
                    Remove(_removeType, unit);
                }
                break;
            case SpellUnitTarget.randomAlly:
                var units = battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive).ToList();
                if (units.Count > 0) Remove(_removeType, units.GetRandomElement());
                break;
            case SpellUnitTarget.randomEnemy:
                var enemies = battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive).ToList();
                if (enemies.Count > 0) Remove(_removeType, enemies.GetRandomElement());
                break;
            case SpellUnitTarget.allUnits:
                foreach (var unit in battlefieldManager.GetAllUnits().Where(target => target.IsAlive))
                {
                    Remove(_removeType, unit);
                }
                break;
            case SpellUnitTarget.randomUnit:
                var all = battlefieldManager.GetAllUnits().Where(target => target.IsAlive).ToList();
                if (all.Count > 0) Remove(_removeType, all.GetRandomElement());
                break;
        }


        base.UseAbility(battlefieldManager, player);
    }
    private void Remove(PerkEffectType type, UnitBase unit)
    {
        unit.PerkBaseList.FindAll(p => p.EffectType == type).ForEach(a => a.RemovePerk(unit));
        if(m_ParticleSystem != null)
        {
            ShowParticleOnUnit(unit);
        }
    }

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return _target switch
        {
            SpellUnitTarget.allies =>       battlefieldManager.GetAllyUnits(player).    Where(target => target.IsAlive && target.PerkBaseList.Any(p => p.EffectType == _removeType)).Count() > 1,
            SpellUnitTarget.enemies =>      battlefieldManager.GetEnemyUnits(player).   Where(target => target.IsAlive && target.PerkBaseList.Any(p => p.EffectType == _removeType)).Count() > 1,
            SpellUnitTarget.randomAlly =>   battlefieldManager.GetAllyUnits(player).    Where(target => target.IsAlive && target.PerkBaseList.Any(p => p.EffectType == _removeType)).Count() > 1,
            SpellUnitTarget.randomEnemy =>  battlefieldManager.GetEnemyUnits(player).   Where(target => target.IsAlive && target.PerkBaseList.Any(p => p.EffectType == _removeType)).Count() > 1,
            SpellUnitTarget.allUnits =>     battlefieldManager.GetAllUnits().           Where(target => target.IsAlive && target.PerkBaseList.Any(p => p.EffectType == _removeType)).Count() > 2,
            SpellUnitTarget.randomUnit =>   battlefieldManager.GetAllUnits().           Where(target => target.IsAlive && target.PerkBaseList.Any(p => p.EffectType == _removeType)).Count() > 2,
            _ => false,
        };
    }
}
