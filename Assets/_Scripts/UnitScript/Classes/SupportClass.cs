using System;
using System.Collections;
using UnityEngine;

public class SupportClass : UnitBase
{
    private IEnumerator SupEffect;

    public override void PowerPlaceEnable()
    {
        SupEffect = RepeatEffect(5f, Effect);
        StartCoroutine(SupEffect);
    }

    private void Effect()
    {
        Owner.BattlefieldHero.Health += 5;
        foreach (var collider in Physics2D.OverlapCircleAll(transform.position, GameLibrary.Instance.MediumAura, LayerMask.GetMask("AllyUnits")))
        {
            if (collider.TryGetComponent<UnitBase>(out var unit) && unit.IsAlive && unit.IsFrendly(Owner))
            {
                unit.Health += 5;
                Instantiate(VfxProvider.Instance.HealEffect, unit.transform.position.AddY(SpriteHeight / 2), Quaternion.identity, unit.transform);
            }
        }
    }

    protected override void OnStart()
    {
        CreateAura(AuraSize.Small, aura => aura.UnitEnterAura += OnUnitEnterAura);
        SupEffect = RepeatEffect(5f, Effect);
    }

    private void OnUnitEnterAura(UnitBase unit)
    {
        if (unit.IsAlive && IsAlive && unit.IsFrendly(Owner))
        {
            unit.Health += (int)(unit.Data.Health * 0.4f);
            Instantiate(VfxProvider.Instance.HealEffect, unit.transform.position.AddY(SpriteHeight / 2), Quaternion.identity, unit.transform);
        }
    }

    public override void PowerPlaceDisable()
    {
        StopCoroutine(SupEffect);
    }
}
