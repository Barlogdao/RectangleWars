using UnityEngine;

namespace RB.HeroStats
{
    public abstract class HeroStatEffect : ScriptableObject
    {
        public abstract string ShowDescription();
    }

    public abstract class HeroStatUnitEffect : HeroStatEffect
    {
        public abstract void Execute(UnitBase unit);
    }

}

