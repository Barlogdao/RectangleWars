using UnityEngine;

public class TauntEffect : EffectBase
{
    public override void Execute(UnitBase sender, UnitBase target)
    {
        target.MoveModule.MoveTo(sender.transform.position);
        Instantiate(VfxProvider.Instance.Taunt, target.transform.position.AddY(0.8f), Quaternion.identity, target.transform);
    }

    public override void OnEndEffect(UnitBase sender, UnitBase target)
    {
       
    }
}
