public enum Fraction {Order,Death,Life,Destruction,Wisdom,Neutral }
public enum Complexity { EASY, NORMAL, HARD }
public enum HeroStat { Leadership, Sorcery, Stamina }
public enum ResourseType { Gold, Karlov, Fierce, Mana }
public enum WalkType
{
    None = 0,
    Waterwalk = 1,
    Mugwalk = 2,
    Sandwalk = 3,
    Forestwalk = 4,
    Landwalk = 100
}

public enum AttackDistanceType
{
    None = 0,
    Melee = 1,
    Range = 2,
    Artillery = 3
}

public enum ClassType
{
    None = 0,
    Shooter = 1,
    Scout = 2,
    Warrior = 3,
    Wizard = 4,
    Support = 5,
    Worker = 6,
    Assassin = 7,
    Commander = 8,
    Hero = 9,
    Summon = 10
}

public enum AuraSize
{
    Small,Medium,Large,Zero
}
public enum GamePlayersCondition
{
    humanVsAi,
    AiVsAi
}