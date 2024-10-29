using System.Collections;
using UnityEngine;

public class WizardClass : UnitBase
{

    private IEnumerator WizEffect;
    protected override void OnStart()
    {
        WizEffect = RepeatEffect(5f, () => PowePlaceEffect());
    }

    public override void DealDamage(IDamagable target)
    {
        CurrentAttack = Attack + UnityEngine.Random.Range(0, 3);
        if (target.Class == ClassType.Hero)
        {
            CurrentAttack += BONUS_DAMAGE;
        }
        UsePerks(PerkType.BeforeAttack, target);
        if (CurrentAttack == 0) return;
        target.GetDamage(CurrentAttack + (IsIgnoreArmor ? target.Armor : 0), this);
        UsePerks(PerkType.AfterAttack, target);
    }

    private void PowePlaceEffect()
    {
        Owner.Mana++;
        Instantiate(VfxProvider.Instance.ManaGainEffect, new Vector3(transform.position.x, transform.position.y + SpriteHeight / 2, transform.position.z), Quaternion.identity, transform);
    }

    public override void PowerPlaceEnable()
    {
        WizEffect = RepeatEffect(5f, () => PowePlaceEffect());
        StartCoroutine(WizEffect);
    }

    public override void PowerPlaceDisable()
    {
        StopCoroutine(WizEffect);
    }
}
