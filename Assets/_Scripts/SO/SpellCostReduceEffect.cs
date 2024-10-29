using Assets.SimpleLocalization;
using UnityEngine;

namespace RB.HeroStats
{
    [CreateAssetMenu(fileName = "HSE_SpellCostReduce", menuName = "HeroStatEffects/SpellCostReduce")]
    public class SpellCostReduceEffect: HeroStatEffect
    {
        private const string CDRELOAD_MESSAGE = "Hero.CDReload.Description";
        public int SpellCostReduce;
        public int Execute()
        {
            return SpellCostReduce;
        }

        public override string ShowDescription()
        {
            return LocalizationManager.Localize(CDRELOAD_MESSAGE, new string[]{ SpellCostReduce.ToString()});
        }
    }





}

