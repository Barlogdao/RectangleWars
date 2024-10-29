using UnityEngine;

public class HealingAuraPerk : PerkBase
{
    [SerializeField] private AreaEffectZone _effectZone;
    public override void UsePerk(IDamagable another, UnitBase self)
    {
    }
    public override void InitializePerk(UnitBase self)
    {
        Instantiate(_effectZone, self.transform);
    }
}
