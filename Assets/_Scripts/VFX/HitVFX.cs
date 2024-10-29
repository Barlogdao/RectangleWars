using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class HitVFX : MonoBehaviour, IPoolObject
{
    PoolManager manager;
    

    private ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        manager = GetComponentInParent<PoolManager>();
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    public void OnCreatedInPool()
    {
       
        
    }

    public void OnGettingFromPool()
    {
        particle.Play();
    }
 
    private void OnParticleSystemStopped()
    {
        transform.parent = manager.GetPool<HitVFX>().Container;
        manager.TakeToPool<HitVFX>(this);
    }
}
