using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Telega : MonoBehaviour, IDamagable
{
    [SerializeField]
    int _maxHealth, _minResAmount, _maxResAmount;
    [SerializeField]
    float cd, _speed;
    PatrolUser patrolModule;
    private int _health;


    private Transform nextPoint = null;

    int _count;
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;
    Material _material;
    ParticleSystem _particleSystem;

    Tween tween;

    public int Health { get { return _health; } set { _health = OnHealthChanged(value); } }
    public bool IsIgnoreArmor => false;
    public bool IsAlive => _health > 0;
    private int RandomAmount { get => Random.Range(_minResAmount, _maxResAmount + 1); }
    public AttackDistanceType AttackDistance => AttackDistanceType.None;
    public ClassType Class => ClassType.None;

    public int Armor => 0;

    public void GetDamage(int damage, IAttackable sourse)
    {
        StartCoroutine(ShowHit());
        Health -= Mathf.Max(1, (damage - Armor));
        if (!IsAlive)
            Demolish(sourse);
    }

 
    private IEnumerator Start()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
        _particleSystem = GetComponent<ParticleSystem>();
        patrolModule = GetComponent<PatrolUser>();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        yield return Utilis.GetWait(3f);
        InitializeObject();
    }

    void InitializeObject()
    {
        _health = _maxHealth;
        transform.position = patrolModule.StartPoint().position;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        _particleSystem.Play();
        _material.SetFloat("_HitValue", 0f);
        GoNext();
    }
    private void GoNext()
    {
       nextPoint = patrolModule.NextPoint();
        if (nextPoint == null)
        {
            Removeobject();
        }
        else
        {
            _spriteRenderer.flipX = transform.position.x >= nextPoint.position.x;
            tween = transform.DOMove(nextPoint.position, _speed).SetSpeedBased().OnComplete(() => GoNext());
        }

    }
    public void Demolish(IAttackable sourse)
    {
        if (sourse is UnitBase unit)
        {
            unit.GetComponentInParent<Player>().GetRes(RandomRes(), RandomAmount);
        }
        Removeobject();
    }

    void Removeobject()
    {
        tween.Kill();
        StartCoroutine(CDStart());
        _particleSystem.Play();
        _spriteRenderer.enabled =false;
        _collider.enabled = false;
    }

    private ResourseType RandomRes()
    {
        return ResourseType.Gold;
    }


    IEnumerator CDStart()
    {
        yield return Utilis.GetWait(cd);
        InitializeObject();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator ShowHit()
    {
        _material.SetFloat("_HitValue", 1f);
        yield return Utilis.GetWait(0.1f);
        _material.SetFloat("_HitValue", 0f);
    }

    int OnHealthChanged(int value)
    {
        // Попап с уроном
        if (_health >= value) EventBus.DamagableDamaged?.Invoke(_health - value, transform);
        else EventBus.DamagableHealed?.Invoke(value - _health, transform);
        return Mathf.Max(0, value);
    }

    public void GetTrueDamage(int damage)
    {
        StartCoroutine(ShowHit());
        Health -= damage;
        if(!IsAlive) Removeobject();
    }
}
