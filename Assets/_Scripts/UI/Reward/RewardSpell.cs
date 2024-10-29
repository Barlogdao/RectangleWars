using Assets.SimpleLocalization;
public class RewardSpell : RewardBase
{
    
    SpellSO Spell;

    public RewardSpell (SpellSO spell)
    {
        Spell = spell;
        Icon = Spell.Image;
        Name = Spell.Name;
        
    }

    public override void Execute()
    {
        GameManager.Instance.Hero.Inventory.Spells.Add(Spell);
    }

    public override string[] GetRewardInfo()
    {
       return  new string[] { Spell.GetLeftInfo(GameManager.Instance.Hero), Spell.GetRightInfo(GameManager.Instance.Hero) };
    }
}
