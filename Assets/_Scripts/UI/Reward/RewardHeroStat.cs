
using UnityEngine;

public class RewardHeroStat : RewardBase
{
    HeroStatSO stat;

    Hero _hero;
    public RewardHeroStat()
    {
        _hero = GameManager.Instance.Hero;
        switch (Random.Range(0, 3))
        {
            case 0:
                if (_hero.Leadership >= 5) goto case 1 ;
                stat = GameLibrary.Instance.HeroStats[HeroStat.Leadership];
                break;
            case 1:
                if (_hero.Sorcery >= 5) goto case 2;
                stat = GameLibrary.Instance.HeroStats[HeroStat.Sorcery];
                break;
            case 2: if (_hero.Stamina >= 5)
                    goto case 0;
                stat = GameLibrary.Instance.HeroStats[HeroStat.Stamina];
                break;
        }
        Icon = stat.Image;
        Name = stat.Name + " + 1";
    }

    public override void Execute()
    {
        switch (stat.Stat)
        {
            case HeroStat.Leadership: GameManager.Instance.Hero.Leadership++;
                break;
            case HeroStat.Sorcery: GameManager.Instance.Hero.Sorcery++;
                break;
            case HeroStat.Stamina: GameManager.Instance.Hero.Stamina++;
                break;
        }
    }

    public override string[] GetRewardInfo()
    {
       return new string[] { stat.ShortDesc, "" };
    }
}
