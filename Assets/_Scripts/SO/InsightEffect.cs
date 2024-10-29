using Assets.SimpleLocalization;
using UnityEngine;

namespace RB.HeroStats
{
    [CreateAssetMenu(fileName = "HSE_InsightEffect", menuName = "HeroStatEffects/InsightEffect")]
    public class InsightEffect: HeroStatEffect
    {
        private const string INSIGHT_MESSAGE = "Hero.InsightEffect.Description";
        public SpellSO Insight;
        public void Execute(BattlefieldManager battlefieldManager, Player player)
        {
            Insight.Spell.UseAbility(battlefieldManager, player);
        }

        public override string ShowDescription()
        {
           return LocalizationManager.Localize(INSIGHT_MESSAGE);
        }
    }





}

