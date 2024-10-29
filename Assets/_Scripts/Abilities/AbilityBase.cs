using UnityEngine;
public enum SpawnPosition { Bottom, Center, Top }

[System.Serializable]
public abstract class AbilityBase : MonoBehaviour
{
    [SerializeField]
    protected AudioClip m_Clip;
    [SerializeField]
    protected ParticleSystem m_ParticleSystem;
    [SerializeField]
    protected SpawnPosition _spawnPosition;

    public virtual void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        EventBus.SoundEvent?.Invoke(m_Clip);
    }

    public virtual string[] GetParams(Hero hero)
    {
        return new string[0];
    }

    protected float GetParticleSpawnYPos(UnitBase unit, SpawnPosition position)
    {
        switch (position)
        {
            case SpawnPosition.Bottom: return 0f;
            case SpawnPosition.Center: return unit.SpriteHeight / 2;
            case SpawnPosition.Top: return unit.SpriteHeight - 0.1f;
        }
        return 0f;
    }

    protected void RunParticle(ParticleSystem particle, float duration)
    {
        if (duration > 0f)
        {
            var main = particle.main;
            main.startLifetime = duration;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
        particle.Play();
    }
    protected  ParticleSystem ShowParticleOnUnit(UnitBase unit)
    {
       return  Instantiate(m_ParticleSystem, unit.transform.position.AddY(GetParticleSpawnYPos(unit, _spawnPosition)), Quaternion.identity, unit.transform);
    }
    public abstract bool Resolver(BattlefieldManager battlefieldManager, Player player);
}
