using System;
using UnityEngine;
using Redcode.Extensions;

[RequireComponent(typeof(CircleCollider2D))]
public class AreaEffectZone : MonoBehaviour
{
    private Player _owner;
    private UnitBase _holder;
    [SerializeField]
    private AuraSize _areaSize;
    [SerializeField]
    private Target _target;
    [SerializeField]
    private EffectBase[] _effects;
    [SerializeField] ParticleSystem _areaVisual;
    void Start()
    {
        _holder = GetComponentInParent<UnitBase>();
        _owner = _holder.Owner;
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        switch (_areaSize)
        {
            case AuraSize.Small:
                collider.radius = GameLibrary.Instance.SmallAura;
                break;
            case AuraSize.Medium:
              collider.radius = GameLibrary.Instance.MediumAura;
                break;
            case AuraSize.Large:
                collider.radius = GameLibrary.Instance.LargeAura;
                break;
            case AuraSize.Zero:
                collider.radius = 0f;
                break;
        }
        var particle = Instantiate<ParticleSystem>(_areaVisual, transform).main;
        particle.startSize = collider.radius * 2;
        particle.startColor = _owner.Hero == GameManager.Instance.Hero ? particle.startColor: GameManager.Instance.Settings.EnemyPlayerColor.WithA(0.3f);

        _holder.LocalDieEvent += OnDieHandler;

        
    }

    private void OnDieHandler(UnitBase unit)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out UnitBase unit) && _holder.IsAlive)
        {
            switch (_target)
            {
                case Target.Ally:
                    if (unit.Owner == _owner && unit.IsAlive)
                    foreach (EffectBase effect in _effects)
                    {
                        effect.Execute(_holder, unit);
                    }
                    break;
                case Target.Enemy:
                    if (unit.Owner != _owner && unit.IsAlive)
                    foreach (EffectBase effect in _effects)
                    {
                        effect.Execute(_holder, unit);
                    }
                    break;
                case Target.All:
                    if (unit.IsAlive)
                    foreach (EffectBase effect in _effects)
                    {
                        effect.Execute(_holder, unit);
                    }
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out UnitBase unit) && unit.IsAlive)
        {
            switch (_target)
            {
                case Target.Ally:
                    if (unit.Owner == _owner)
                        foreach (EffectBase effect in _effects)
                        {
                            effect.OnEndEffect(_holder, unit);
                        }
                    break;
                case Target.Enemy:
                    if (unit.Owner != _owner)
                        foreach (EffectBase effect in _effects)
                        {
                            effect.OnEndEffect(_holder, unit);
                        }
                    break;
                case Target.All:
                        foreach (EffectBase effect in _effects)
                        {
                            effect.OnEndEffect(_holder, unit);
                        }
                    break;
            }
        }
    }
}

public enum Target
{
    Ally,
    Enemy,
    All
}
