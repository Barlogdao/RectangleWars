using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class MarkPerk : PerkBase
{
    [SerializeField]
    ParticleSystem particlePref;
    [SerializeField]
    private int damageAmount;
    public override void UsePerk(IDamagable damagesourse, UnitBase unit)
    {
        unit.CurrentDamage += damageAmount;
        unit.RemoveParticle(particlePref.name);
    }
    public override void InitializePerk(UnitBase unit)
    {
        var paritcle = Instantiate(particlePref, unit.transform.position.AddY(unit.SpriteHeight - 0.1f), Quaternion.identity, unit.transform);
        paritcle.name = particlePref.name;
    }
    protected override void OnRemovePerk(UnitBase self)
    {
        self.RemoveParticle(particlePref.name);
    }
    public override string[] GetParams()
    {
        return new string[] {damageAmount.ToString() };
    }

}
