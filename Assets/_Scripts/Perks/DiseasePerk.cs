using System.Collections;
using UnityEngine;

public class DiseasePerk : PerkBase
{
    [SerializeField] private UnitPerksSO _diseasePrefab;
    [SerializeField]
    private ParticleSystem _particle;
    public float _duration;
    public int _damage;
    private const string AURA_NAME = "DISEASE_AURA";
    public override void UsePerk(IDamagable another, UnitBase self)
    {

    }
    public override void InitializePerk(UnitBase self)
    {
        //создать ауру
        var aura = self.CreateAura(AuraSize.Medium, OnAuraCreated);
        aura.gameObject.name = AURA_NAME;
        self.StartCoroutine(RemovePerkWithDuration(_duration, self));
        self.StartCoroutine(DamageResolve(_duration));



        var particle = Instantiate(_particle, self.transform.position.AddY(Utilis.GetParticleSpawnYPos(self, SpawnPosition.Center)), Quaternion.identity, self.transform);
        particle.name = _particle.name;


        void OnAuraCreated(IncomingAura aura)
        {
            aura.UnitEnterAura += OnUnitEnterAura;
        }

        void OnUnitEnterAura(UnitBase unit)
        {
            if (self.IsAlive && unit.IsFrendly(self.Owner) && unit.IsAlive && !unit.ImmuneToMagic)
            {
                unit.AddPerk(_diseasePrefab);
            }
        }
        IEnumerator DamageResolve(float duration)
        {
            float currentTime = 1f;
            yield return Utilis.GetWait(1f);
            while (currentTime <= duration && self.IsAlive && self.HasPerk(_diseasePrefab))
            {
                self.GetTrueDamage(_damage);
                currentTime += 1f;
                yield return Utilis.GetWait(1f);
            }
        }
        IEnumerator RemovePerkWithDuration(float duration, UnitBase self)
        {
            yield return Utilis.GetWait(duration);
            RemovePerk(self);
        }
    }
    protected override void OnRemovePerk(UnitBase self)
    {
        self.RemoveParticle(_particle.name);
        foreach (Transform t in self.transform)
        {
            if (t.gameObject.name == AURA_NAME)
            {
                Destroy(t.gameObject);
            }
        }
    }

    public override string[] GetParams()
    {
        return new string[] { _damage.ToString(), _duration.ToString() };
    }

}
