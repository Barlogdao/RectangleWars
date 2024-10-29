using UnityEngine;
using Redcode.Pools;
using System;

public class HitVFXService : MonoBehaviour
{
    PoolManager _manager;

    private void Start()
    {
        _manager = GetComponent<PoolManager>();
    }
    private void OnEnable()
    {
        EventBus.DamagableDamaged += OnDamagableDamaged;
    }

    private void OnDamagableDamaged(int arg1, Transform target)
    {
        var clone = _manager.GetFromPool<HitVFX>();
        
        clone.transform.position = target.transform.position.AddY(0.5f);
        clone.transform.parent = target;
    }
    private void OnDisable()
    {
        EventBus.DamagableDamaged -= OnDamagableDamaged;
    }
}
