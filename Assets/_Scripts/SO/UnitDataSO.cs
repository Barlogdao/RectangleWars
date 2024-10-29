using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Assets.SimpleLocalization;
using NaughtyAttributes;

[System.Serializable]
public struct UnitStats
{
    public int Health;
    public int Attack;
    public int AttackSpeed;
    public int Armor;
    public float Speed;
    public UnitStats(int health, int attack, int attackSpeed, int armor, float speed)
    {
        Health = health;
        Attack = attack;
        AttackSpeed = attackSpeed;
        Armor = armor;
        Speed = speed;
    }
    public static UnitStats operator +(UnitStats a, UnitStats b)
    {
        return new UnitStats
        {
            Health = a.Health + b.Health,
            Attack = a.Attack + b.Attack,
            Armor = a.Armor + b.Armor,
            Speed = b.Speed != 0f? a.Speed * b.Speed: a.Speed * 1f,
            AttackSpeed = a.AttackSpeed + b.AttackSpeed
        };
    }
    public static UnitStats operator -(UnitStats a, UnitStats b)
    {
        return new UnitStats
        {
            Health = a.Health - b.Health,
            Attack = a.Attack - b.Attack,
            Armor = a.Armor - b.Armor,
            Speed = b.Speed != 0f ? a.Speed / b.Speed : a.Speed / 1f,
            AttackSpeed = a.AttackSpeed + -b.AttackSpeed
        };
    }
    // перечислитель для прохода по параметрам структуры для локализации
    public IEnumerator<string> GetEnumerator()
    {
        yield return Health.ToString();
        yield return Attack.ToString();
        yield return AttackSpeed.ToString();
        yield return Armor.ToString();
        yield return (Speed * 100).ToString();
    }
}

[CreateAssetMenu(fileName = "UnitDataSO", menuName = "ScriptableObjects/Data", order = 4)]
public class UnitDataSO : ScriptableObject
{
    [Range(1, 3)]
    public int Tier;
    [field:SerializeField] public bool BlankUnit { get; private set; }
    public Fraction fraction;
    public ClassType Class;
    public AttackDistanceType AttackDistance;
    [ShowAssetPreview]
    public Sprite Image;
    public string NameKey => "Unit." + name;
    public string Name { get => LocalizationManager.Localize(NameKey); }
    public int Health => stats.Health;
    public int Attack => stats.Attack;
    public float AttackSpeed => stats.AttackSpeed;
    public int Armor => stats.Armor;
    public float Speed => stats.Speed;
    [Header("Характеристики")]
    public UnitStats stats;
    public RuntimeAnimatorController Animator;

    private bool NeedBullet => AttackDistance != AttackDistanceType.Melee;
    [ShowIf("NeedBullet")]
    public GameObject BulletPrefab;

    [Space(10)]
    [Header("Стоимость")]
    public int GoldCost;

    [Header("Звуки")]
    [Tooltip("Размещать если нужен не звук по умолчанию")]
    public AudioClip HitSound;
    [Tooltip("Размещать если нужен не звук по умолчанию")]
    public AudioClip DieSound;

    [Header("Умения")]
    [HideInInspector]
    public string PerkInfo;

    public List<UnitPerksSO> PerkList;

    public string GetFractionText
    {
        get
        {
            return fraction switch
            {
                Fraction.Order => LocalizationManager.Localize("Fraction.Order"),
                Fraction.Death => LocalizationManager.Localize("Fraction.Death"),
                Fraction.Life => LocalizationManager.Localize("Fraction.Life"),
                Fraction.Destruction => LocalizationManager.Localize("Fraction.Destruction"),
                Fraction.Wisdom => LocalizationManager.Localize("Fraction.Wisdom"),
                Fraction.Neutral => LocalizationManager.Localize("Fraction.Neutral"),
                _ => "Нейтральный",
            };
        }
    }

    public string GetClassText => LocalizationManager.Localize("Class." + Class);

    /// <summary>
    /// Проверка хватает ли ресурсов у игрока на юнита
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool CanBuyUnit(Player player)
    {
        return player.Gold >= GoldCost;
    }
    private void OnEnable()
    {
        LocalizationManager.LocalizationChanged += SetPerksText;
        SetPerksText();
    }
    private void OnDisable()
    {
        LocalizationManager.LocalizationChanged -= SetPerksText;
    }

    private void SetPerksText()
    {
        StringBuilder text = new();
        foreach (var perk in PerkList)
        {
            text.AppendLine(perk.Name);
        }
        PerkInfo = text.ToString();
    }

    public string GetLeftInfo(Hero hero)
    {
        StringBuilder text = new();
        text.AppendLine($"@{GetFractionText}`");
        text.AppendLine();
        text.AppendLine($"~{Name}`");
        text.AppendLine();
        switch (hero.Leadership)
        {
            case 0:
                text.AppendLine($"{LocalizationManager.Localize("Unit.Health")}: {Health}");
                text.AppendLine($"{LocalizationManager.Localize("Unit.Attack")}: {Attack}");
                break;
            default:
                text.AppendLine($"{LocalizationManager.Localize("Unit.Health")}: {Health}");
                text.AppendLine($"{LocalizationManager.Localize("Unit.Attack")}: {Attack} + ${hero.GetUnitAttackBonus()}`");
                break;
        }
        text.AppendLine($"{LocalizationManager.Localize("Unit.Armor")}: {Armor}");
        text.AppendLine($"{LocalizationManager.Localize("Unit.AttackSpeed")}: {AttackSpeed}");
        text.AppendLine();
        text.AppendLine($"*{PerkInfo}");
        return text.ToString();
    }

    public string GetRightInfo(Hero hero)
    {
        StringBuilder text = new();
        text.AppendLine($"%{LocalizationManager.Localize("Res.Gold")}: {GoldCost}`");
        text.AppendLine();
        text.AppendLine($"@{GetClassText}`");
        return text.ToString();

    }

    public GameObject SpawnUnit(Vector3 position, Transform parent)
    {
        var unit = Instantiate(GameLibrary.Instance.UnitPrefab, position, Quaternion.Euler(0, 0, 0), parent);
        switch (Class)
        {
            case ClassType.Shooter:
                unit.AddComponent<ShooterClass>();
                break;
            case ClassType.Scout:
                unit.AddComponent<ScoutClass>();
                break;
            case ClassType.Warrior:
                unit.AddComponent<WarriorClass>();
                break;
            case ClassType.Wizard:
                unit.AddComponent<WizardClass>();
                break;
            case ClassType.Support:
                unit.AddComponent<SupportClass>();
                break;
            case ClassType.Worker:
                unit.AddComponent<WorkerClass>();
                break;
            case ClassType.Assassin:
                unit.AddComponent<AssassinClass>();
                break;
            case ClassType.Commander:
                unit.AddComponent<CommanderClass>();
                break;
            case ClassType.Summon:
                unit.AddComponent<SummonClass>();
                break;
            default: break;
        }
        unit.GetComponent<UnitBase>().Data = this;
        unit.name = Name;
        return unit;
    }

}
