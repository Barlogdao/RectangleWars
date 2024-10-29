using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FractionHolder : MonoBehaviour
{
    // словарь со всеми фракциями

    Dictionary<Fraction, List<UnitDataSO>> allUnits = new();
    Dictionary<Fraction, List<SpellSO>> allSpells = new();


    private Dictionary<ClassType,ClassSO> _unitClassConfigs = new();

    public List<UnitDataSO> GetAllUnits()
    {
        List<UnitDataSO> units = new List<UnitDataSO>();
        foreach(var unitList in allUnits)
        {
            units.AddRange(unitList.Value);
        }
        return units;
    } 

    public List<SpellSO> GetAllSpells()
    {
        List<SpellSO> spells = new List<SpellSO>();
        foreach(var spellList in allSpells)
        {
            spells.AddRange(spellList.Value);
        }
        return spells;
    }

    private void Awake()
    {


        var units = Resources.LoadAll<UnitDataSO>("Units");
        foreach (var unit in units)
        {
            if (!allUnits.ContainsKey(unit.fraction))
            {
                allUnits.Add(unit.fraction, new List<UnitDataSO>());
            }
            allUnits[unit.fraction].Add(unit);
        }
        var spells = Resources.LoadAll<SpellSO>("Spells");
        foreach (var spell in spells)
        {
            if (!allSpells.ContainsKey(spell.Fraction))
            {
                allSpells.Add(spell.Fraction, new List<SpellSO>());
            }
            allSpells[spell.Fraction].Add(spell);
        }
        GoogleSheetLoader.OnProcessData += InitUnitStats;
        var classes = Resources.LoadAll<ClassSO>("Classes");
        foreach(var clazz in classes)
        {
            _unitClassConfigs.Add(clazz.ClassType,clazz);
        }
    }

    public UnitDataSO GetStandartUnit(ClassType type)
    {
        return _unitClassConfigs[type].StandartUniit;
    }
    public ClassSO GetClassConfig(ClassType type)
    {
        return _unitClassConfigs[type];
    }

    private void InitUnitStats(Dictionary<string, (UnitStats, int)> stats)
    {
        foreach (var unitData in Resources.LoadAll<UnitDataSO>("Units"))
        {
            unitData.stats = stats[unitData.NameKey].Item1;
            unitData.GoldCost = stats[unitData.NameKey].Item2;
        }
    }

    // метод для получения доступного юнита
    public UnitDataSO GetAvaliableUnit(Hero hero)
    {
        // поиск максимального тира юнита

        int maxTier = Mathf.Max(1, Mathf.Max(
            hero.StartUnit.Count > 0 ? hero.StartUnit.Max(a => a.Tier) : 0,
            hero.Inventory.Units.Count > 0 ? hero.Inventory.Units.Max(b => b.Tier) : 0));

        var units = allUnits[hero.fraction]
            .Where(a => a.Tier <= maxTier + 1)
            .Except(hero.StartUnit)
            .Except(hero.Inventory.Units)
            .Except(allUnits[hero.fraction].FindAll(u => u.Class == ClassType.Worker || u.Class == ClassType.Summon))
            .ToArray();
        //if (units.Length > 0 && units.Any(u => u.fraction == Hero.fraction) && Utilis.Chanse(30))
        //{
        //    return units.First(u => u.fraction == Hero.fraction);
        //}
        return units.Length > 0 ? units[Random.Range(0, units.Length)] : null;
    }
    // метод для получения доступного спелла
    public SpellSO GetAvaliableSpell(Hero hero)
    {
        int maxTier = Mathf.Max(1, Mathf.Max(
            hero.StartSpell.Count > 0 ? hero.StartSpell.Max(a => a.Tier) : 0,
            hero.Inventory.Spells.Count > 0 ? hero.Inventory.Spells.Max(b => b.Tier) : 0));


        var spells = allSpells[hero.fraction]
            .Where(a => a.Tier <= maxTier + 1)
            .Union(allSpells[Fraction.Neutral])
            .Where(t => t.Tier <= maxTier + 1)
            .Except(hero.StartSpell)
            .Except(hero.Inventory.Spells)
            .ToArray();

        if (spells.Length > 0 && spells.Any(s => s.Fraction == hero.fraction) && Utilis.Chanse(35))
        {
            return spells.First(u => u.Fraction == hero.fraction);
        }
        return spells.Length > 0 ? spells[Random.Range(0, spells.Length)] : null;
    }

    public UnitDataSO GetWorker(Hero hero)
    {
        return allUnits[Fraction.Neutral].Find(u => u.Class == ClassType.Worker);
    }

    private void OnDisable()
    {
        GoogleSheetLoader.OnProcessData -= InitUnitStats;
    }

}
