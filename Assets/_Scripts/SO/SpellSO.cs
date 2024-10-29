using System;
using System.Text;
using UnityEngine;
using Assets.SimpleLocalization;


[CreateAssetMenu(fileName = "AbilitySO", menuName = "ScriptableObjects/Abilities", order = 7)]
public class SpellSO : ScriptableObject
{
    
    [Range(1,4)]
    public int Tier;
    public Fraction Fraction;
    private string NameKey => "Spell." + name;
    public string Name { get => LocalizationManager.Localize(NameKey); }
    public string Description(Hero hero) { return LocalizationManager.Localize(NameKey + LocalizationManager.Desc, Spell.GetParams(hero)); }
    public Sprite Image;
    public AbilityBase Spell;

    [Space(10)]
    [Header("Стоимость")]
    public int ManaCost;
    public float SpellCooldown;

    public string GetLeftInfo(Hero hero)
    {
        StringBuilder text = new();
        text.AppendLine($"@{GetFractionText}`");
        text.AppendLine();
        text.AppendLine($"~{Name}`");
        text.AppendLine();
        text.AppendLine(Description(hero));
        return text.ToString();
    }
    public string GetRightInfo(Hero hero)
    {
        StringBuilder text = new();
        text.AppendLine($"${LocalizationManager.Localize("Res.Mana")}: {ManaCost}`");
        return text.ToString();

    }
    public string GetFractionText
    {
        get
        {
            return Fraction switch
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

}
