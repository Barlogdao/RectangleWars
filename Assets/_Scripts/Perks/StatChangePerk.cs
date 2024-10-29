using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangePerk : PerkBase
{
    [SerializeField]
    private UnitStats _stats;
    [OnValueChanged("OnPermanentChanged")]
    [field:SerializeField]
    private bool _isPermanent;
    [HideIf("_isPermanent")]
    [SerializeField]
    [MinValue(0f)]
    private float _duration;
    [SerializeField]
    private ParticleSystem _particle;
    [SerializeField]
    private SpawnPosition _spawnPosition;

    public override void UsePerk(IDamagable another, UnitBase self)
    {
       
    }
    public override void InitializePerk(UnitBase self)
    {
        switch (EffectType)
        {
            case PerkEffectType.persistent:
                break;
            case PerkEffectType.buff: self.BuffStat(_stats);
                break;
            case PerkEffectType.debuff: self.DebuffStat(_stats);
                break;
        }
        if (_particle != null)
        {
            var particle = Instantiate(_particle, self.transform.position.AddY(Utilis.GetParticleSpawnYPos(self, _spawnPosition)), Quaternion.identity, self.transform);
            particle.name = _particle.name;
        }
        if (_duration > 0f)
        {
            self.StartCoroutine(RemovePerkWithDuration(_duration,self));
        }
    }
    private IEnumerator RemovePerkWithDuration(float duration, UnitBase self)
    {
        yield return Utilis.GetWait(duration);
        RemovePerk(self);
    }
    protected override void OnRemovePerk(UnitBase self)
    {
        switch (EffectType)
        {
            case PerkEffectType.persistent:
                break;
            case PerkEffectType.buff: self.DebuffStat(_stats);
                break;
            case PerkEffectType.debuff: self.BuffStat(_stats);
                break;
        }
        self.RemoveParticle(_particle.name);
    }
    public override string[] GetParams()
    {
        List<string> strings = new();
        if (!_isPermanent) strings.Add(_duration.ToString());
        foreach (string a in _stats)
        {
            if (a != "0")
                strings.Add(a);

        }

        return strings.ToArray();
    }

    private void OnPermanentChanged()
    {
        if (_isPermanent) _duration = 0f;
    }
}