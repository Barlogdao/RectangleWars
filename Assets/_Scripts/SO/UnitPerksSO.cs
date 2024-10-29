using UnityEngine;
using Assets.SimpleLocalization;

[CreateAssetMenu(fileName = "NewUnitPerk", menuName = "ScriptableObjects/UnitPerk", order = 9)]
public class UnitPerksSO : ScriptableObject
{

    private string NameKey => "Perk." + name;
    public string Name { get => LocalizationManager.Localize(NameKey); }

    public string Description { get => LocalizationManager.Localize(NameKey + LocalizationManager.Desc,Perkprefab.GetParams()); }
    public PerkBase Perkprefab;
}
