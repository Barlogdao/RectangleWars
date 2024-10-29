using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "newGameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float GoldEarningTime;
    public float ManaEarningTime;
    public float InsightEarningTime;
    public int StartGold;
    public int StartMana;
    public int InsightEarningAmount;
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int MaxGold { get; private set; }
    [Range(0, 100)]
    [SerializeField] private int _additionalRewardChanse;
    public int AdditionalRewardChanse { get => _additionalRewardChanse; }

    [Header("Кулдаун юнитов Игрока и AI по уровням сложности")]
    public float PlayerUnitCD;
    [SerializeField]
    [Range(3f, 6f)]
    float EasyCD, NormalCD, HardCD;
    [Header("Базовые характеристики Героев")]
    public int HeroHealth;
    public int HeroAttack;
    public int HeroArmor;
    [Header ("Цвета игрока и врага")]
    [ColorUsage(true, true)]
    public List<Color> PlayerColors = new();
    [ColorUsage(true, true)]
    public Color EnemyPlayerColor;

    public void SetPlayerSettings(Player player)
    {
        player.Gold = StartGold;
        player.Mana = StartMana;
        player.GoldCD = GoldEarningTime;
        player.ManaCD = ManaEarningTime;
        player.InsightCD = InsightEarningTime;
        if (player is HumanPlayer) { player.UnitCD = PlayerUnitCD;}
        else if (player is AIPlayer) { player.UnitCD = GetAIUnitCD(); }
    }
    //public float GoldEarning(Hero hero) => Mathf.Max(1, GoldEarningTime - (float)hero.Sorcery / 6);
    //public float ManaEarning(Hero hero) => Mathf.Max(1, ManaEarningTime - (float)hero.Sorcery / 3);
    public float GetAIUnitCD()
    {
        return GameManager.Instance.GameComplexity switch
        {
            Complexity.EASY => EasyCD,
            Complexity.NORMAL => NormalCD,
            Complexity.HARD => HardCD,
            _ => EasyCD,
        };
    }
}

