using Assets.SimpleLocalization;
using UnityEngine;

namespace RB.HeroStats
{
    [CreateAssetMenu(fileName = "HSE_UnitPerk", menuName = "HeroStatEffects/UnitPerk")]
    public class UnitPerkBuffEffect : HeroStatUnitEffect
    {
        [SerializeField] public UnitPerksSO Perk;
        private const string UNIT_PERK_MESSAGE = "Hero.UnitPerkEffect.Description";
        public override void Execute (UnitBase unit)
        {
            unit.AddPerk(Perk);
        }

        public override string ShowDescription()
        {
            return LocalizationManager.Localize(UNIT_PERK_MESSAGE);
        }
    }
}

