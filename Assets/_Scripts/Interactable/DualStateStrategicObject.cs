using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DualStateStrategicObject : StrategicObjectBase
{
    private Animator _animator;
    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
    }
    protected override void OnOccupiedStateChange()
    {
        _animator.SetBool(IS_ACTIVE, IsOccupied);
    }
}
