using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Stun : PerkBase
{
    
    public float Duration;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private SpawnPosition _spawnPosition;
    public override void UsePerk(IDamagable another, UnitBase self)
    {
        
    }

    public override void InitializePerk(UnitBase self)
    {
        self.Stun();
        if (_particle != null)
        {
            var particle = Instantiate(_particle, self.transform.position.AddY(Utilis.GetParticleSpawnYPos(self, _spawnPosition)), Quaternion.identity, self.transform);
            particle.name = _particle.name;
        }
        if (Duration > 0f)
        {
            self.StartCoroutine(RemovePerkWithDuration(Duration, self));
        }
    }
    protected override void OnRemovePerk(UnitBase self)
    {
        self.UnStun();
        self.RemoveParticle(_particle.name);
    }
    private IEnumerator RemovePerkWithDuration(float duration, UnitBase self)
    {
        yield return Utilis.GetWait(duration);
        RemovePerk(self);
    }
}
