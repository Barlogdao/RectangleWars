using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class HealEffect : EffectBase
{
    [SerializeField] private int _healAmount;
    public override void Execute(UnitBase sender, UnitBase target)
    {
        if (target.Health < target.MaxHealth)
        {
            Instantiate(VfxProvider.Instance.HealEffect, target.transform.position.AddY(target.SpriteHeight / 2), Quaternion.identity, target.transform);
        }
        target.Heal(_healAmount);
        
    }
    public override void OnEndEffect(UnitBase sender, UnitBase target) { }

}
