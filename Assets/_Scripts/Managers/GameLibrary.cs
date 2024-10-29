using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FractionHolder))]
public class GameLibrary : Singleton<GameLibrary>
{

    public Dictionary<HeroStat, HeroStatSO> HeroStats = new();
    public List<HeroSO> HeroList = new();
    public FractionHolder Fractions { get; private set; }
    public GameObject UnitPrefab;
    [Header("Аура юнита")]
    public IncomingAura Aura;
    public ForceField ForceField;
    [Range(1f, 6f)]
    public float SmallAura, MediumAura, LargeAura;
    [field: SerializeField]
    public ResourseSO Gold { get; private set; }
    [field: SerializeField]
    public ResourseSO Mana { get; private set; }
    [SerializeField]
    private UnitPerksSO _markPerk;
    public UnitPerksSO MarkPerk => _markPerk;

    protected override void OnAwake()
    {
        Fractions = GetComponent<FractionHolder>();
        foreach (var stat in Resources.LoadAll<HeroStatSO>("HeroStat"))
        {
            HeroStats.TryAdd(stat.Stat, stat);
        }
        foreach (var hero in Resources.LoadAll<HeroSO>("Heroes"))
        {
            HeroList.Add(hero);
        }
        DontDestroyOnLoad(gameObject);
    }
}
