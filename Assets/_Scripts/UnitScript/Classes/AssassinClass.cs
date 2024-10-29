using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinClass : UnitBase
{
    private IEnumerator AssassinEffect;

    protected override void OnStart()
    {
        AssassinEffect = RepeatEffect(5f, () => Owner.EnemyHero.GetTrueDamage(3));
    }

    public override void PowerPlaceEnable()
    {
        StartCoroutine(AssassinEffect);
    }

    public override void DealDamage(IDamagable target)
    {
        if (target.Class == ClassType.Hero) CurrentAttack = CritAttack;
        else CurrentAttack = Attack;
        UsePerks(PerkType.BeforeAttack, target);
        if (CurrentAttack == 0) return;
        target.GetDamage(CurrentAttack, this);
        UsePerks(PerkType.AfterAttack, target);
    }

    public override void PowerPlaceDisable()
    {
       StopCoroutine(AssassinEffect);
    }
}
