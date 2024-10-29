using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class HolyShieldPerk : PerkBase
{
    [SerializeField]
    AudioClip sound;
    public override void InitializePerk(UnitBase unit)
    {
       unit.ShaderModule.HolyShieldON();
    }
    public override void UsePerk(IDamagable damageSourse, UnitBase unit)
    {
        if (damageSourse.Class == ClassType.Hero)
        {
            return;
        }
        EventBus.SoundEvent?.Invoke(sound);
        unit.CurrentDamage = 0;
    }
    protected override void OnRemovePerk(UnitBase self)
    {
        self.ShaderModule.HolyShieldOff();
    }
}
