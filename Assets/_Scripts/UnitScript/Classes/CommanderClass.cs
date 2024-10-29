using Redcode.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderClass : UnitBase
{
    private IncomingAura _aura;
    private int _multipler = 1;
    private ParticleSystem _auraArea;

    public override void PowerPlaceDisable()
    {
        _aura.SetAuraSize(AuraSize.Medium);
        _multipler = 1;
        UpdateAuraArea();
    }

    public override void PowerPlaceEnable()
    {
        StartCoroutine(SwitchAuraSize());
        IEnumerator SwitchAuraSize()
        {
            _aura.SetAuraSize(AuraSize.Zero);
            yield return null;
            _multipler = 2;
            _aura.SetAuraSize(AuraSize.Large);
            UpdateAuraArea();
        }
    }
    protected override void OnStart()
    {
       _aura = CreateAura(AuraSize.Medium ,OnAuraCreated);
        _auraArea = Instantiate(VfxProvider.Instance.CommanderAuraArea, transform);
        UpdateAuraArea();
        var particle = _auraArea.main;
        particle.startColor = Owner.Hero == GameManager.Instance.Hero ? particle.startColor : GameManager.Instance.Settings.EnemyPlayerColor.WithA(0.3f);


    }
    private void OnAuraCreated(IncomingAura aura)
    {
        aura.UnitEnterAura += OnUnitEnterAura;
        aura.UnitLeaveAura += OnUnitLeaveAura;
    }
    private void OnUnitEnterAura(UnitBase unit)
    {

        if (unit.IsAlive && unit.Class != ClassType.Commander && unit.IsFrendly(Owner))
        {
            var part = Instantiate(VfxProvider.Instance.CommanderAura, unit.transform.position.AddY(unit.SpriteHeight - 0.1f), Quaternion.identity, unit.transform);
            part.name = VfxProvider.Instance.CommanderAura.name;
            unit.Attack += 5 * _multipler;
            unit.Armor += 2 * _multipler;
            
        }
    }
    private void OnUnitLeaveAura(UnitBase unit)
    {
        if (unit.IsAlive && unit.Class != ClassType.Commander && unit.IsFrendly(Owner))
        {
            unit.Attack -= 5 * _multipler;
            unit.Armor -= 2 * _multipler;
            unit.RemoveParticle(VfxProvider.Instance.CommanderAura.name);
            
        }
    }
    protected override void Die()
    {
        _aura.SetAuraSize(AuraSize.Zero);
        base.Die();
    }

    private void UpdateAuraArea()
    {
        var particle = _auraArea.main;
        particle.startSize = _aura.GetAuraSize() * 2;
    }


}
