using Assets.SimpleLocalization;
using NaughtyAttributes.Test;
using System.Text;
using UnityEngine;

namespace RB.HeroStats
{
    [CreateAssetMenu(fileName = "HSE_UnitBuff", menuName = "HeroStatEffects/UnitBuff")]
    public class UnitStatBuffEffect : HeroStatUnitEffect
    {
        [SerializeField] public UnitStats Stat;

        private const string STAT_BUFF_MESSAGE = "Hero.UnitStatBuff.Description";
        private const string HEALTH = "Unit.Health";
        private const string ATTACK = "Unit.Attack";
        private const string ARMOR = "Unit.Armor";
        private const string ATTACK_SPEED = "Unit.AttackSpeed";
        private const string SPEED = "Unit.Speed";
        public override void Execute(UnitBase unit)
        {
            unit.BuffStat(Stat);
        }

        public override string ShowDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine((LocalizationManager.Localize(STAT_BUFF_MESSAGE)));
            if (Stat.Health > 0) { sb.AppendLine(LocalizationManager.Localize(HEALTH) + " + "  + Stat.Health.ToString()); }
            if(Stat.Attack > 0) { sb.AppendLine(LocalizationManager.Localize(ATTACK) + " + " + Stat.Attack.ToString()); }
            if (Stat.Armor > 0) { sb.AppendLine(LocalizationManager.Localize(ARMOR) + " + " + Stat.Armor.ToString()); }
            if (Stat.AttackSpeed > 0) { sb.AppendLine(LocalizationManager.Localize(ATTACK_SPEED) + " + " + Stat.AttackSpeed.ToString()); }
            if (Stat.Speed > 0) { sb.AppendLine(LocalizationManager.Localize(SPEED) + " + " + Stat.Speed.ToString("F1")) ; }
            return sb.ToString();
        }
    }
}

