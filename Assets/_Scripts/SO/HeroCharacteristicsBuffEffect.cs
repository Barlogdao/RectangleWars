using Assets.SimpleLocalization;
using System.Text;
using UnityEngine;

namespace RB.HeroStats
{
    [CreateAssetMenu(fileName = "HSE_HeroBuff", menuName = "HeroStatEffects/HeroBuff")]
    public class HeroCharacteristicsBuffEffect: HeroStatEffect
    {
        public int Health;
        public int Attack;
        public int Armor;
        private const string HEALTH_MESSAGE = "Hero.AddHealth.Description";
        private const string ARMOR_MESSAGE = "Hero.ArmorArmor.Description";
        private const string ATTACK_MESSAGE = "Hero.AddAttack.Description";
        public void Execute(BattlefieldHero hero)
        {
            hero.Buff(Health, Attack, Armor);
        }

        public override string ShowDescription()
        {
            StringBuilder sb = new StringBuilder();
            if (Health > 0) { sb.AppendLine(LocalizationManager.Localize(HEALTH_MESSAGE, new string[] { Health.ToString() })); }
            if(Attack > 0) { sb.AppendLine(LocalizationManager.Localize(ATTACK_MESSAGE, new string[] { Attack.ToString() })); }
            if(Armor > 0) { sb.AppendLine(LocalizationManager.Localize(ARMOR_MESSAGE, new string[] { Armor.ToString() })); }
            return sb.ToString();
        }
    }





}

