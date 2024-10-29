using Assets.SimpleLocalization;

public class RewardUnit : RewardBase
{
    UnitDataSO unit;
    public RewardUnit(UnitDataSO incomeUnit)
    {
        unit = incomeUnit;
        Icon = unit.Image;
        Name = unit.Name;
    }

    public override void Execute()
    {
        GameManager.Instance.Hero.Inventory.Units.Add(unit);
    }

    public override string[] GetRewardInfo()
    {

        return new string[] { unit.GetLeftInfo(GameManager.Instance.Hero), unit.GetRightInfo(GameManager.Instance.Hero) };
    }
}
