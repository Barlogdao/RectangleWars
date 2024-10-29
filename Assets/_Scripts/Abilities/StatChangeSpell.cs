using Redcode.Extensions;
using System.Linq;
using UnityEngine;

public enum ChangeType { Buff, Debuff }
public enum SpellUnitTarget { allies, enemies, randomAlly, randomEnemy, allUnits, randomUnit }
public class StatChangeSpell : AbilityBase
{
    [SerializeField]
    private UnitPerksSO _perk;
    [SerializeField]
    SpellUnitTarget _target;

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        switch (_target)
        {
            case SpellUnitTarget.allies:
                foreach (var unit in battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive))
                {
                    unit.AddPerk(_perk);
                }
                break;
            case SpellUnitTarget.enemies:
                foreach (var unit in battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive))
                {
                    unit.AddPerk(_perk);
                }
                break;
            case SpellUnitTarget.randomAlly:
                var units = battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive).ToList();
                if (units.Count > 0) units.GetRandomElement().AddPerk(_perk);
                break;
            case SpellUnitTarget.randomEnemy:
                var enemies = battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive).ToList();
                if (enemies.Count > 0) enemies.GetRandomElement().AddPerk(_perk);
                break;
            case SpellUnitTarget.allUnits:
                foreach (var unit in battlefieldManager.GetAllUnits().Where(target => target.IsAlive))
                {
                    unit.AddPerk(_perk);
                }
                break;
            case SpellUnitTarget.randomUnit:
                var all = battlefieldManager.GetAllUnits().Where(target => target.IsAlive).ToList();
                if (all.Count > 0) all.GetRandomElement().AddPerk(_perk);
                break;
        }
        base.UseAbility(battlefieldManager, player);
    }
    public override string[] GetParams(Hero hero)
    {
        return _perk.Perkprefab.GetParams();
    }

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return _target switch
        {
            SpellUnitTarget.allies => battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive && !target.PerkBaseList.Contains(_perk.Perkprefab)).Count() > 1,
            SpellUnitTarget.enemies => battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive && !target.PerkBaseList.Contains(_perk.Perkprefab)).Count() > 2,
            SpellUnitTarget.randomAlly => battlefieldManager.GetAllyUnits(player).Where(target => target.IsAlive && !target.PerkBaseList.Contains(_perk.Perkprefab)).Count() > 1,
            SpellUnitTarget.randomEnemy => battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive && !target.PerkBaseList.Contains(_perk.Perkprefab)).Count() > 1,
            SpellUnitTarget.allUnits => battlefieldManager.GetAllUnits().Where(target => target.IsAlive && !target.PerkBaseList.Contains(_perk.Perkprefab)).Count() > 2,
            SpellUnitTarget.randomUnit => battlefieldManager.GetAllUnits().Where(target => target.IsAlive && !target.PerkBaseList.Contains(_perk.Perkprefab)).Count() > 2,
            _ => false,
        };
    }
}
