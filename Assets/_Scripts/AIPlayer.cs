using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AIPlayer : Player
{

    public LayerMask Mask;
    AIDetector detector;
    private List<UnitDataSO> _currentUnitList;

    public override void OnStart()
    {
        _currentUnitList = Hero.GetHeroUnits();
        detector = new AIDetector(tag);
        SpawnerOnCD = true;
        detector.InteractableDetected += OnSoDetected;
        detector.EnemyUnitDetected += OnUnitDetected;
        detector.HeroDetected += OnHeroDetected;
        detector.WaterDetected += OnWaterDetected;
        StartCoroutine(SpellLogic());
        StartCoroutine(UnitSpawnLogic());
    }
    #region Логика вызова юнитов
    #region Действия на информацию детектора
    public void OnSoDetected(StrategicObjectBase so)
    {
        if (so.Type == SOType.PowerPlace && Utilis.Chanse(10f))
        {
            HireUnit();
        }
        else if (so.Type == SOType.Resmine)
        {
            if (!MnogoWorkers)
                HireWorker();
        }
        else if (so.Type == SOType.BattleRestrictedObject)
        {
            HireUnit();
        }
    }
    public void OnUnitDetected(UnitBase unit)
    {
        //Пускает воина если видит работающего крестьянина
        if (unit is WorkerClass worker && worker.IsBusy)
        {
            HireUnit(ClassType.Scout);
        }
        else if (!SpawnerOnCD && unit.IsBusy)
        {
            switch (unit.Class)
            {
                case ClassType.Shooter:
                    HireUnit(ClassType.Scout);
                    break;
                case ClassType.Scout:
                    HireUnit(ClassType.Warrior);
                    break;
                case ClassType.Warrior:
                    HireUnit(ClassType.Shooter);
                    break;
                default:
                    HireUnit();
                    break;
            }
        }
        else if (!SpawnerOnCD && Utilis.Chanse(5))
        {
            switch (unit.Class)
            {
                case ClassType.Shooter:
                    HireUnit(ClassType.Scout);
                    break;
                case ClassType.Scout:
                    HireUnit(ClassType.Warrior);
                    break;
                case ClassType.Warrior:
                    HireUnit(ClassType.Shooter);
                    break;
                default:
                    HireUnit();
                    break;
            }
        }
    }
    public void OnHeroDetected()
    {

        if (Utilis.Chanse(30))
            HireUnit();
        else
            HireUnit(ClassType.Assassin);
    }
    public void OnWaterDetected()
    {
        if (!SpawnerOnCD && Utilis.Chanse(5))
        {
            HireUnit(WalkType.Waterwalk);
        }
    }
    #endregion
    private bool MnogoWorkers => _unitList.FindAll(u => u.Class == ClassType.Worker).Count >= BattlefieldManager.MinesCount;

    /// <summary>
    /// Вызов Рандомного юнита
    /// </summary>
    private void HireUnit()
    {
        var unitsForHire = _currentUnitList.FindAll(u => u.CanBuyUnit(this));
        if (unitsForHire.Count > 0 && BattlefieldHero.IsAlive)
        {
            var unit = unitsForHire[UnityEngine.Random.Range(0, unitsForHire.Count)];
            CreateUnit(unit);
        }
    }

    /// <summary>
    ///  Вызов рандомного юнита определенного класса
    /// </summary>
    /// <param name="classType"></param>
    private void HireUnit(ClassType classType)
    {
        var unitsForHire = _currentUnitList.FindAll(u => u.Class == classType && u.CanBuyUnit(this));
        if (unitsForHire.Count > 0 && BattlefieldHero.IsAlive)
        {
            var unit = unitsForHire[UnityEngine.Random.Range(0, unitsForHire.Count)];
            CreateUnit(unit);
        }
    }
    /// <summary>
    ///  Вызов рандомного юнита с определенной дистанции атаки
    /// </summary>
    /// <param name="distanceType"></param>
    private void HireUnit(AttackDistanceType distanceType)
    {
        var unitsForHire = _currentUnitList.FindAll(u => u.AttackDistance == distanceType && u.CanBuyUnit(this));
        if (unitsForHire.Count > 0 && BattlefieldHero.IsAlive)
        {
            var unit = unitsForHire[UnityEngine.Random.Range(0, unitsForHire.Count)];
            CreateUnit(unit);
        }
    }
    /// <summary>
    ///  Вызов Рандомного юнита с определенным волктайпом
    /// </summary>
    /// <param name="type"></param>
    private void HireUnit(WalkType type)
    {
        var unitsForHire = _currentUnitList.
            FindAll(u => u.PerkList.
            Exists(perkSO => perkSO.Perkprefab is PerkWalkType perk
            && (perk.walkType == type || perk.walkType == WalkType.Landwalk))
            && u.CanBuyUnit(this));

        if (unitsForHire.Count > 0 && BattlefieldHero.IsAlive)
        {
            var unit = unitsForHire[UnityEngine.Random.Range(0, unitsForHire.Count)];
            CreateUnit(unit);
        }
    }

    private void HireWorker()
    {
        if (GameLibrary.Instance.Fractions.GetWorker(Hero).CanBuyUnit(this))
        {
            CreateUnit(GameLibrary.Instance.Fractions.GetWorker(Hero));
        }
    }
    public override void OnDestroyHandler()
    {
        detector.InteractableDetected -= OnSoDetected;
        detector.EnemyUnitDetected -= OnUnitDetected;
        detector.HeroDetected -= OnHeroDetected;
        detector.WaterDetected -= OnWaterDetected;
    }

    IEnumerator SpellLogic()
    {
        while (BattlefieldHero.IsAlive)
        {
            yield return Utilis.GetWait(3f);

            if (Utilis.Chanse(30) && GetAvaliableSpells().Count > 0)
            {
                foreach (var spell in GetAvaliableSpells().OrderByDescending(o => o.Tier).ThenByDescending(c => c.ManaCost))
                {
                    if (spell.Spell.Resolver(BattlefieldManager, this))
                    {
                        CastSpell(spell);
                        break;
                    }
                }
            }
        }
    }
    IEnumerator UnitSpawnLogic()
    {
        while (BattlefieldHero.IsAlive)
        {
            yield return Utilis.GetWait(0.5f);
            if (!SpawnerOnCD)
            {
                detector.CastRay(spawner.transform.position, spawner.transform.up, Mask);
                if (!SpawnerOnCD && Utilis.Chanse(13))
                {
                    HireUnit();
                }
            }
        }
    }
    List<SpellSO> GetAvaliableSpells()
    {
        return Hero.StartSpell.FindAll(s => CanCastSpell(s));
    }
    #endregion
}
