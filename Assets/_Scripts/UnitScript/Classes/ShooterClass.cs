using UnityEngine;

public class ShooterClass : UnitBase
{
    

    public override void DealDamage(IDamagable target)
    {
        CurrentAttack = Attack + UnityEngine.Random.Range(0, 3);
        if (target.Class == ClassType.Warrior)
        {
            CurrentAttack += BONUS_DAMAGE + 5;
        }
        UsePerks(PerkType.BeforeAttack, target);
        if (CurrentAttack == 0) return;
        target.GetDamage(CurrentAttack + (IsIgnoreArmor ? target.Armor : 0), this);
        UsePerks(PerkType.AfterAttack, target);
    }


    public override void PowerPlaceEnable()
    {
        MoveModule.Stop();
        _isBusy = true;
        SortingOrder += 1;
        FightZone.FightRadius.radius += 1;
    }

    public override void PowerPlaceDisable()
    {
        SortingOrder -= 1;
        FightZone.FightRadius.radius -= 1;
    }
}
