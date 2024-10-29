using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    public abstract void Execute(UnitBase sender, UnitBase target);
    public abstract void OnEndEffect(UnitBase sender, UnitBase target);

}
