using UnityEngine;



public enum PerkType
{
    BeforeAttack = 0,
    AfterAttack = 1,
    BeforeDamage = 2,
    AfterDamage = 3,
    Walktype = 4,
    Passive = 5
}
public enum PerkEffectType
{
    persistent,
    buff,
    debuff
}

public abstract class PerkBase : MonoBehaviour
{
    [field: SerializeField]
    public PerkType perkType { get; protected set; }
    [field: SerializeField]
    public PerkEffectType EffectType { get; protected set; }
    [field: SerializeField]
    public bool Disposable { get; protected set; } = false;

    [SerializeField] private ParticleSystem _lifeCycleParticles;
    [SerializeField] private SpawnPosition _lifeCycleParticklesPosition;
    public abstract void UsePerk(IDamagable another, UnitBase self);
    public virtual void InitializePerk(UnitBase self)
    {
        if (_lifeCycleParticles != null)
        {
            var particle = Instantiate(_lifeCycleParticles, self.transform.position.AddY(Utilis.GetParticleSpawnYPos(self, _lifeCycleParticklesPosition)), Quaternion.identity, self.transform);
            particle.name = _lifeCycleParticles.name;
        }
    }
    public void RemovePerk(UnitBase self)
    {
        if (self == null || !self.PerkBaseList.Contains(this)) return;
        OnRemovePerk(self);
        self.PerkBaseList.Remove(this);
    }
    protected virtual void OnRemovePerk(UnitBase self)
    {
        if (_lifeCycleParticles != null)
        {
            self.RemoveParticle(_lifeCycleParticles.name);
        }

    }
    public virtual string[] GetParams()
    {
        return new string[0];
    }
    public virtual void ExecuteDisposable(UnitBase unit) { }

    protected float GetAuraRadius(AuraSize size)
    {
        return size switch
        {
            AuraSize.Small => GameLibrary.Instance.SmallAura,
            AuraSize.Medium => GameLibrary.Instance.MediumAura,
            AuraSize.Large => GameLibrary.Instance.LargeAura,
            _ => 0f,
        };
    }


}
