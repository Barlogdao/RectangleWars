using System.Collections.Generic;
using UnityEngine;
using Redcode.Extensions;
using System.Linq;

public class AddPerkSpell : AbilityBase
{
    [SerializeField] SpellUnitTarget _spellTarget;
    [SerializeField] UnitPerksSO _unitPerk;

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        foreach(var target in GetTargetUnits(_spellTarget, battlefieldManager, player))
        {
            target.AddPerk(_unitPerk);
            if(m_ParticleSystem != null)
            {
                ShowParticleOnUnit(target);
            }
        }

    }


    private List<UnitBase> GetTargetUnits(SpellUnitTarget target, BattlefieldManager battlefieldManager, Player spellcaster)
    {
        List<UnitBase> avaliableUnits = new();
        switch (target)
        {
            case SpellUnitTarget.allies:
                avaliableUnits = battlefieldManager.GetAllyUnits(spellcaster);
                break;
            case SpellUnitTarget.enemies:
                avaliableUnits = battlefieldManager.GetEnemyUnits(spellcaster);
                break;
            case SpellUnitTarget.randomAlly:
                var units = battlefieldManager.GetAllyUnits(spellcaster)
                    .Where(target => target.IsAlive)
                    .ToList();
                AddIfTargetExist(units);
                break;
            case SpellUnitTarget.randomEnemy:
                var enemies = battlefieldManager.GetEnemyUnits(spellcaster)
                    .Where(target => target.IsAlive)
                    .ToList();
                AddIfTargetExist(enemies);
                break;
            case SpellUnitTarget.allUnits:
                avaliableUnits = battlefieldManager.GetAllUnits();
                break;
            case SpellUnitTarget.randomUnit:
                var all = battlefieldManager.GetAllUnits()
                    .Where(target => target.IsAlive)
                    .ToList();
                AddIfTargetExist(all);
                break;
        }
        return avaliableUnits;

        void AddIfTargetExist(List<UnitBase> unitList)
        {
            if (unitList.Count > 0)
            {
                avaliableUnits.Add(unitList.GetRandomElement());    
            }
        }
    }
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return _spellTarget switch
        {
            SpellUnitTarget.allies => battlefieldManager.GetAllyUnits(player).Count(target => target.IsAlive && !target.HasPerk(_unitPerk)) > 1,
            SpellUnitTarget.enemies => battlefieldManager.GetEnemyUnits(player).Count(target => target.IsAlive && !target.HasPerk(_unitPerk)) > 1,
            SpellUnitTarget.randomAlly => battlefieldManager.GetAllyUnits(player).Count(target => target.IsAlive && !target.HasPerk(_unitPerk)) > 1,
            SpellUnitTarget.randomEnemy => battlefieldManager.GetEnemyUnits(player).Count(target => target.IsAlive && !target.HasPerk(_unitPerk)) > 1,
            SpellUnitTarget.allUnits => battlefieldManager.GetAllUnits().Count(target => target.IsAlive && !target.HasPerk(_unitPerk)) > 2,
            SpellUnitTarget.randomUnit => battlefieldManager.GetAllUnits().Count(target => target.IsAlive && !target.HasPerk(_unitPerk)) > 2,
            _ => false,
        };
    }

    public override string[] GetParams(Hero hero)
    {
        return _unitPerk.Perkprefab.GetParams();
    }
}
