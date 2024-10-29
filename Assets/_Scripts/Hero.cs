using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using Assets.SimpleLocalization;
using NaughtyAttributes;

[System.Serializable]
public class Hero
{
    public string HeroNameKey;
    public RuntimeAnimatorController Animator;
    public Sprite HeroSprite;
    public Fraction fraction;
    public Fraction friendlyFraction;

    [MaxValue (5)]
    public int Stamina;
    [MaxValue(5)]
    public int Leadership;
    [MaxValue(5)]
    public int Sorcery;
    public int level;



    public List<UnitDataSO> StartUnit;
    public List<SpellSO> StartSpell;
    public HeroInventory Inventory;

    public List<SpellSO> Insights;

    public Hero(Hero other)
    {
        HeroNameKey = other.HeroNameKey;
        HeroSprite = other.HeroSprite;
        fraction = other.fraction;
        friendlyFraction = other.friendlyFraction;
        Stamina = other.Stamina;
        Leadership = other.Leadership;
        Sorcery = other.Sorcery;
        level = other.level;
        StartUnit = new List<UnitDataSO>(other.StartUnit);
        StartSpell = new List<SpellSO>(other.StartSpell);
        Inventory = new HeroInventory(other.Inventory);
        Insights = new List<SpellSO>(other.Insights);
        Animator = other.Animator;

    }

    public int GetUnitAttackBonus()
    {
        switch (Leadership)
        {
            case 1: return 1;
            case 2:
            case 3: return 2;
            case 4:
            case 5: return 5;
            default: return 0;
        }
    }

    public static string GetLeftInfo(Hero hero)
    {
        StringBuilder text = new();
        text.AppendLine($"{LocalizationManager.Localize(hero.HeroNameKey)}");
        text.AppendLine();
        text.AppendLine($"{LocalizationManager.Localize("Hero.Leadership")}: {hero.Leadership}");
        text.AppendLine($"{LocalizationManager.Localize("Hero.Sorcery")}: {hero.Sorcery}");
        text.AppendLine($"{LocalizationManager.Localize("Hero.Stamina")}: {hero.Stamina}");
        text.AppendLine();
        text.AppendLine($"{LocalizationManager.Localize("Hero.GoldEarn")}: {GameManager.Instance.Settings.GoldEarningTime:F1}");
        text.AppendLine($"{LocalizationManager.Localize("Hero.ManaEarn")}: {GameManager.Instance.Settings.ManaEarningTime:F1}");

        return text.ToString();
    }

    public List<UnitDataSO> GetHeroUnits()
    {
        var unitList = new List<UnitDataSO>();
        unitList.Add(GameLibrary.Instance.Fractions.GetStandartUnit(ClassType.Worker));
        unitList.Add(FindUnitOrStandart(ClassType.Scout));
        unitList.Add(FindUnitOrStandart(ClassType.Shooter));
        unitList.Add(FindUnitOrStandart(ClassType.Warrior));
        if (level > 1)
        {
            unitList.Add(FindUnitOrStandart(ClassType.Wizard));
        }
        if(level > 2)
        {
            unitList.Add(FindUnitOrStandart(ClassType.Commander));
        }
        return unitList;
    }
    private UnitDataSO FindUnitOrStandart(ClassType type)
    {
        return StartUnit.Exists(unit => unit.Class == type) ? StartUnit.Find(u => u.Class == type) : GameLibrary.Instance.Fractions.GetStandartUnit(type);
    }



    #region Äëÿ ÀÈ
    public void CheckAILevel()
    {
        if (level > 1)
        {
            for (int i = 2; i <= level; i++)
                AILVLUP();
        }
    }




    void AILVLUP()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
            case 1:
                UnitDataSO unit = GameLibrary.Instance.Fractions.GetAvaliableUnit(this);
                if (unit != null)
                {
                    StartUnit.Add(unit);
                    break;
                }
                goto case 2;
            case 2:
            case 3:
                SpellSO spell = GameLibrary.Instance.Fractions.GetAvaliableSpell(this);
                if (spell != null)
                {
                    StartSpell.Add(spell);
                    break;
                }
                goto case 4;
            case 4: RandomStatUP(); break;
        }
    }
    void RandomStatUP()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                if (Stamina >= 5)
                    goto case 1;
                Stamina++;
                break;
            case 1:
                if (Leadership >= 5)
                    goto case 2;
                Leadership++;
                break;
            case 2:
                if (Sorcery >= 5)
                    goto case 0;
                Sorcery++;
                break;
        }
    }
    #endregion
}

