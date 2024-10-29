using Cysharp.Threading.Tasks;
using RB.HeroStats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public abstract class Player : MonoBehaviour
{
    private const float FREE_SPELL_CD_OFFSET = 3f;
    private const int MAX_FREE_SPELL_VALUE = 4;

    private Hero _hero;
    private BattlefieldHero _bHero;
    protected Spawner spawner;

    private int _gold;
    private int _mana;
    private int _insightLevel = 0;
    private int _freeSpellCounter = 0;
    public int InsightLevel
    {
        get => _insightLevel; set
        {
            _insightLevel = value;
            InsightChanged?.Invoke(_insightLevel);
            if (_insightLevel >= 100 && _bHero.IsAlive)
            {
                InsightLevel = 0;
                ExecuteInsight();
            }
        }
    }



    private float _goldCD;
    private float _manaCD;
    private float _unitCD;
    private float _insightCD;
    public int SpellCostReduce { get; private set; } = 0;
    protected bool spawnerOnCD;

    protected List<UnitBase> _unitList = new List<UnitBase>();
    public Dictionary<SpellSO, float> LastCastedTimeOfSpells { get; protected set; }


    public int Gold { get => _gold; set { _gold = Math.Clamp(value, 0, MaxGold); GoldChanged?.Invoke(_gold); } }
    public int Mana { get => _mana; set { _mana = Math.Clamp(value, 0, MaxMana); ManaChanged?.Invoke(_mana); } }
    public int MaxGold => GameManager.Instance.Settings.MaxGold + Hero.Sorcery;
    public int MaxMana => GameManager.Instance.Settings.MaxMana + Hero.Sorcery;
    public ArrowRotateType ArrowRotateType { get; private set; }

    public float GoldCD { get => _goldCD; set => _goldCD = value; }
    public float ManaCD { get => _manaCD; set => _manaCD = value; }
    public float UnitCD { get => _unitCD; set => _unitCD = value; }
    public float InsightCD { get => _insightCD; set => _insightCD = value; }
    public BattlefieldManager BattlefieldManager { get; protected set; }
    public BattlefieldHero EnemyHero { get; private set; }
    public Hero Hero => _hero;
    public BattlefieldHero BattlefieldHero => _bHero;
    public virtual Color PlayerColor { get; protected set; }
    public Transform SpawnerTransform => spawner.transform;
    public virtual List<UnitBase> PlayerUnits => _unitList;
    public int FreeSpellCounter
    {
        get => _freeSpellCounter; 
        set
        {
            _freeSpellCounter = Mathf.Clamp(value, 0, MAX_FREE_SPELL_VALUE);
            FreeSpellCounterChanged?.Invoke(_freeSpellCounter);
        }
    }


    public event Action<int> ManaChanged;
    public event Action<int> GoldChanged;
    public event Action<int> InsightChanged;
    public event Action SpellCasted;
    public event Action<int> FreeSpellCounterChanged;


    public virtual bool SpawnerOnCD
    {
        get { return spawnerOnCD; }
        set
        {
            spawnerOnCD = value;
            if (SpawnerOnCD)
            {
                StartCoroutine(SpawnerReload());
            }
        }
    }



    public virtual void Init(Hero hero, Color color, BattlefieldManager bm, BattlefieldHero bHero, ArrowRotateType arrowRotateType)
    {
        _hero = hero;
        PlayerColor = color;
        BattlefieldManager = bm;
        ArrowRotateType = arrowRotateType;

        GameManager.Instance.Settings.SetPlayerSettings(this);
        _bHero = bHero;
        bHero.InitHero(transform);
        spawner = GetComponentInChildren<Spawner>();
        LastCastedTimeOfSpells = new Dictionary<SpellSO, float>();
        foreach (var spell in Hero.StartSpell)
        {
            LastCastedTimeOfSpells[spell] = -spell.SpellCooldown;
        }
        SetSpellCostReduceAmount();
    }



    public void StartGame()
    {
        EnemyHero = BattlefieldManager.GetEnemyBH(this);
        _bHero.InitHeroUI();
        StartCoroutine(DefaultGold());
        StartCoroutine(DefaultMana());
        StartCoroutine(DefaultInsight());
        OnStart();
    }
    

    public virtual void OnStart() { }



    public void GetRes(ResourseType resourse, int amount = 1)
    {
        switch (resourse)
        {
            case ResourseType.Gold: Gold += amount; break;
            case ResourseType.Mana: Mana += amount; break;
            default:
                break;
        }
    }

    //Генерация бесплатной голды
    private IEnumerator DefaultGold()
    {
        yield return null;
        GoldChanged?.Invoke(Gold);
        while (_bHero.IsAlive)
        {
            yield return Utilis.GetWait(GoldCD);
            Gold++;
        }
    }
    private IEnumerator DefaultMana()
    {
        yield return null;
        ManaChanged?.Invoke(Mana);
        while (_bHero.IsAlive)
        {
            yield return Utilis.GetWait(ManaCD);
            Mana++;
        }
    }

    public void AddFreeSpellPoint()
    {
        FreeSpellCounter += 1;
    }

    private IEnumerator DefaultInsight()
    {
        while (_bHero.IsAlive)
        {
            yield return Utilis.GetWait(InsightCD);
            InsightLevel += GameManager.Instance.Settings.InsightEarningAmount;
        }
    }

    //Перезарядка кулдауна на спаун юнитов
    protected virtual IEnumerator SpawnerReload()
    {
        yield return Utilis.GetWait(UnitCD);
        SpawnerOnCD = false;
    }

    protected void CreateUnit(UnitDataSO unit)
    {
        SpendResForUnit(unit);
        SpawnerOnCD = true;
        spawner.SpawnUnit(unit, transform);
        BattlefieldHero.PlaySpecialAnim();
    }


    protected void SpendResForUnit(UnitDataSO unit)
    {
        Gold -= unit.GoldCost;
    }

    void OnSpawnUnit(UnitBase unit)
    {
        if (unit.Owner == this)
        {
            _unitList.Add(unit);
            SetUnitBuff(unit);
        }

    }
    void OnUnitDie(UnitBase unit)
    {
        if (unit.Owner == this && _unitList.Contains(unit))
        {
            _unitList.Remove(unit);
            InsightLevel += GetAllyUnitInsightScore(unit);
        }
        else if (unit.Owner != this)
        {
            InsightLevel += GetEnemyUnitScore(unit);
        }
    }

    private int GetAllyUnitInsightScore(UnitBase unit)
    {
        if (unit.Class == ClassType.Worker) return 0;
        return unit.Data.Tier switch
        {
            1 => 1,
            2 => 5,
            3 => 10,
            _ => 0,
        };
    }


    private int GetEnemyUnitScore(UnitBase unit)
    {
        if (unit.Class == ClassType.Worker) return 0;
        return unit.Data.Tier switch
        {
            1 => 1,
            2 => 3,
            3 => 5,
            _ => 0,
        };
    }

    private int GetSpellInsightScore(SpellSO spell)
    {
        return spell.Tier switch
        {
            1 => 5,
            2 => 10,
            3 => 15,
            4 => 20,
            _ => 0,
        };
    }
    public bool CanCastSpell(SpellSO spell)
    {
        return HaveManaforCast(spell) && LastCastedTimeOfSpells[spell] + spell.SpellCooldown  < Time.time;
    }
    public bool HaveManaforCast(SpellSO spell)
    {
        return Mana >= spell.ManaCost - SpellCostReduce || spell.Tier <= FreeSpellCounter;
    }

    public void CastSpell(SpellSO spell)
    {
        BattlefieldHero.PlaySpecialAnim();
        if (spell.Tier <= FreeSpellCounter)
        {

            LastCastedTimeOfSpells[spell] = Time.time - spell.SpellCooldown + FREE_SPELL_CD_OFFSET;
            FreeSpellCounter -= spell.Tier;
        }
        else
        {
            Mana -= spell.ManaCost;
            LastCastedTimeOfSpells[spell] = Time.time;
        }

        spell.Spell.UseAbility(BattlefieldManager, this);

        InsightLevel += GetSpellInsightScore(spell);
        SpellCasted?.Invoke();

    }

    private void ExecuteInsight()
    {
        foreach (var insight in _hero.Insights)
        {
            insight.Spell.UseAbility(BattlefieldManager, this);
        }
        PlayInsights();
        BattlefieldHero.PlaySpecialAnim();
    }

    protected virtual void Awake()
    {

        UnitBase.UnitIsSpawned += OnSpawnUnit;
        UnitBase.UnitIsDead += OnUnitDie;

    }

    protected virtual void OnDestroy()
    {
        UnitBase.UnitIsSpawned -= OnSpawnUnit;
        UnitBase.UnitIsDead -= OnUnitDie;
        OnDestroyHandler();
    }
    public virtual void OnDestroyHandler() { }
    #region StatEffects
    internal void SetHeroBuffs(BattlefieldHero battlefieldHero)
    {
        ExecuteHeroBuff(HeroStat.Stamina, _hero.Stamina);
        ExecuteHeroBuff(HeroStat.Leadership, _hero.Leadership);
        ExecuteHeroBuff(HeroStat.Sorcery, _hero.Sorcery);


        void ExecuteHeroBuff(HeroStat stat, int statLevel)
        {
            if (statLevel > 0)
            {
                for (int i = 0; i < statLevel; i++)
                {
                    if (GameLibrary.Instance.HeroStats[stat].StatEffects[i] is HeroCharacteristicsBuffEffect effect)
                    {
                        effect.Execute(battlefieldHero);
                    }
                }
            }
        }
    }
    public void SetUnitBuff(UnitBase unit)
    {
        ExecuteUnitBuff(HeroStat.Stamina, _hero.Stamina);
        ExecuteUnitBuff(HeroStat.Leadership, _hero.Leadership);
        ExecuteUnitBuff(HeroStat.Sorcery, _hero.Sorcery);

        void ExecuteUnitBuff(HeroStat stat, int statLevel)
        {
            if (statLevel > 0)
            {
                for (int i = 0; i < statLevel; i++)
                {
                    if (GameLibrary.Instance.HeroStats[stat].StatEffects[i] is HeroStatUnitEffect effect)
                    {
                        effect.Execute(unit);
                    }
                }
            }

        }

    }
    private void SetSpellCostReduceAmount()
    {
        ExecuteSpellReduceEffect(HeroStat.Stamina, _hero.Stamina);
        ExecuteSpellReduceEffect(HeroStat.Leadership, _hero.Leadership);
        ExecuteSpellReduceEffect(HeroStat.Sorcery, _hero.Sorcery);

        void ExecuteSpellReduceEffect(HeroStat stat, int statLevel)
        {
            if (statLevel > 0)
            {
                for (int i = 0; i < statLevel; i++)
                {
                    if (GameLibrary.Instance.HeroStats[stat].StatEffects[i] is SpellCostReduceEffect effect)
                    {
                        SpellCostReduce += effect.Execute();
                    }
                }
            }

        }
    }

    private void PlayInsights()
    {
        ExecuteInsightEffect(HeroStat.Stamina, _hero.Stamina);
        ExecuteInsightEffect(HeroStat.Leadership, _hero.Leadership);
        ExecuteInsightEffect(HeroStat.Sorcery, _hero.Sorcery);

        void ExecuteInsightEffect(HeroStat stat, int statLevel)
        {
            if (statLevel > 0)
            {
                for (int i = 0; i < statLevel; i++)
                {
                    if (GameLibrary.Instance.HeroStats[stat].StatEffects[i] is InsightEffect effect)
                    {
                        effect.Execute(BattlefieldManager, this);
                    }
                }
            }

        }
    }
    #endregion

}
