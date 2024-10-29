using UnityEngine;

public class ScoutClass : UnitBase
{

    private IncomingAura _aura;
 
    public override void DealDamage(IDamagable target)
    {
        CurrentAttack = Attack + UnityEngine.Random.Range(0, 3);
        if (target.Class == ClassType.Shooter)
        {
            CurrentAttack += BONUS_DAMAGE;
        }
        UsePerks(PerkType.BeforeAttack, target);
        if (CurrentAttack == 0) return;
        target.GetDamage(CurrentAttack + (IsIgnoreArmor ? target.Armor : 0), this);
        UsePerks(PerkType.AfterAttack, target);
    }

    public override void GetDamage(int damage, IAttackable sourse)
    {
        base.GetDamage(damage, sourse);
        if (CanMove && sourse.Class == ClassType.Shooter && Physics2D.LinecastAll(transform.position,sourse.Transform.position, LayerMask.GetMask("Obstacle")).Length <=0) 
        {
            MoveModule.MoveTo(sourse.Transform.position);
        }
    }


    public override void PowerPlaceEnable()
    {
        _aura = CreateAura( AuraSize.Medium,aura => aura.UnitEnterAura += OnUnitEnterAura);
        //StartCoroutine(Mark());
    }
    public void OnPowerPlace(IncomingAura aura)
    {
        aura.UnitEnterAura += OnUnitEnterAura;
    }

    private void OnUnitEnterAura(UnitBase unit)
    {
        if (unit.IsAlive && IsAlive && !unit.IsFrendly(Owner))
        {
            unit.AddPerk(GameLibrary.Instance.MarkPerk);
        }
    }



    public override void PowerPlaceDisable()
    {

        Destroy(_aura.gameObject);
       
    }
}
