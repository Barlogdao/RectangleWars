using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public abstract class UnitBase : MonoBehaviour, IDamagable, IAttackable
{
    public UnitDataSO Data;
    public Player Owner;
    protected SpriteRenderer _spriteRenderer;
    protected Collider2D _bodyCollider;

    public FightZone FightZone;
    public UnitMove MoveModule;
    public UnitShaderControl ShaderModule;
    protected UnitAnimationModule _animationModule;

    protected SOType _occupiedSO;
    private Transform _enemyTransform;
    private IDamagable _enemyTarget;

    private bool _inBattle;
    protected bool _isBusy;
    public bool ImmuneToMagic = false;
    protected const int BONUS_DAMAGE = 10;

    protected UnitStats _stats;
    public int CurrentAttack; //Расчет атаки перед ударом
    public int CurrentDamage;//Расчет урона перед получением

    public List<PerkBase> PerkBaseList = new();

    public event Action<UnitBase> LocalDieEvent;
    public event Action<float> StartAttack;
    public event Action<int, int> UnitDamaged;
    public event Action<UnitBase> NotBusy;

    public static event Action<UnitBase> UnitIsDead;
    public static event Action<UnitBase> UnitIsSpawned;

    private IEnumerator _battleRoutine;
    private IEnumerator _stunEffect;

    #region Свойства
    public virtual int Health
    {
        get => _stats.Health;
        set
        {
            if (!IsAlive || Health == value || value > MaxHealth) return;
            ShowDamageVisual(_stats.Health, value);

            _stats.Health = Mathf.Clamp(value, 0, MaxHealth);
            UnitDamaged?.Invoke(Health, MaxHealth);
            if (!IsAlive)
            {
                MoveModule.Stop();
                Die();
            }
        }
    }
    private int _maxHealth;
    public int MaxHealth { get => Mathf.Max(1,_maxHealth); set => _maxHealth = value; }
    public virtual int Attack { get => Mathf.Max(0, _stats.Attack); set => _stats.Attack = value; }
    public virtual int AttackSpeed { get => Math.Max(1, _stats.AttackSpeed); set => _stats.AttackSpeed = value; }
    public virtual int Armor { get => Mathf.Max(0, _stats.Armor); set => _stats.Armor = value; }
    public float AttackDuration => 1f / (AttackSpeed / 10f);
    public virtual float Speed
    {
        get => _stats.Speed;
        set
        {
            //if (_stats.Speed == value) return;
            _stats.Speed = value;
            if (IsAlive) MoveModule.OnChangeSpeed();
        }
    }
    public int CritAttack { get => (int)(Attack * 1.5f); }
    public bool InBattle { get => _inBattle; protected set { _inBattle = value; if (!_inBattle) { EnemyTransform = null; EnemyTarget = null; } } }
    public bool IsImmobilized { get => MoveModule.IsImmobilized; }
    public bool IsAlive { get => Health > 0; }
    public bool IsIgnoreArmor { get; set; } = false;
    public bool IsBusy
    {
        get => _isBusy;
        protected set
        {
            _isBusy = value;
            if (_isBusy == false && IsAlive)
            {
                NotBusy?.Invoke(this);
                MoveModule.RandomMove();
            }

        }
    }
    public float SpriteHeight => _spriteRenderer.size.y;
    public int SortingOrder { get => _spriteRenderer.sortingOrder; set => _spriteRenderer.sortingOrder = value; }
    public bool CanMove { get => MoveModule.CanMove && IsAlive; }
    public AttackDistanceType AttackDistance { get; private set; }
    public ClassType Class { get; protected set; }
    public Transform EnemyTransform { get => _enemyTransform; protected set => _enemyTransform = value; }
    public IDamagable EnemyTarget { get => _enemyTarget; protected set => _enemyTarget = value; }

    public Transform Transform => transform;
    #endregion


    protected virtual void Awake()
    {
        _bodyCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        FightZone = GetComponentInChildren<FightZone>();
        MoveModule = gameObject.AddComponent<UnitMove>();
        ShaderModule = gameObject.AddComponent<UnitShaderControl>();
        _animationModule = GetComponent<UnitAnimationModule>();
        Owner = GetComponentInParent<Player>();

    }
    protected void Start()
    {
        InitializeStats();
        FightZone.InitFightZone();
        FightZone.UnitInFightZone += OnUnitInFightZone;
        MoveModule.CheckDirection();
        OnStart();
    }
    protected virtual void OnStart() { }
    protected virtual void InitializeStats()
    {
        tag = Owner.tag;
        gameObject.layer = Owner.gameObject.layer;
        _stats = Data.stats;
        AttackDistance = Data.AttackDistance;
        Class = Data.Class;
        MaxHealth = _stats.Health;
        _animationModule.Init(AttackDistance, this, Data.BulletPrefab, Data.Animator);
        foreach (var perk in Data.PerkList)
        {
            AddPerk(perk);
        }
        UnitIsSpawned?.Invoke(this);
    }
    public void AddPerk(UnitPerksSO perk)
    {
        CheckAndAddPerk(perk);
        void CheckAndAddPerk(UnitPerksSO perk)
        {
            if (HasPerk(perk)) return;
            PerkBaseList.Add(perk.Perkprefab);
            perk.Perkprefab.InitializePerk(this);
        }
    }

    #region Битва
    //Вход в битву
    public virtual void StartBattle(IDamagable target, Collider2D collision)
    {
        MoveModule.Stop();
        InBattle = true;
        MoveModule.CheckDirection(collision.transform.position);

        if (target != null && target.IsAlive && IsAlive)
        {
            if(_battleRoutine != null)
            {
                StopCoroutine(_battleRoutine);
            }
            
            _battleRoutine = BattleCoroutine();
            StartCoroutine(_battleRoutine);
        }
        else
            ExitFromBattle();

        IEnumerator BattleCoroutine()
        {
            EnemyTransform = collision.transform;
            EnemyTarget = target;
            while (EnemyTarget != null && target.IsAlive && InBattle && FightZone.FightRadius.enabled)
            {
                StartAttack?.Invoke(AttackDuration);
                if ((AttackDistance == AttackDistanceType.Artillery && AreEnemiesNear()) || Attack <= 0)
                {
                    yield return Utilis.GetWait(AttackDuration);
                }
                else
                {
                    _animationModule.PerformAttack();
                    yield return Utilis.GetWait(AttackDuration);
                }
                
                if (target != null && target.IsAlive && !FightZone.EnemyInRadius(collision))
                    break;
            }
            
            yield return Utilis.GetWait(0.1f);
            if(InBattle) 
            ExitFromBattle();
        }
    }
    public void Stun()
    {
        MoveModule.Stun();
        if ( InBattle && _battleRoutine != null)
        {
            StopCoroutine(_battleRoutine);
            ExitFromBattle();
        }
        InBattle = true;
    }

    public void UnStun()
    {

        if (!IsAlive) return;
        if (_occupiedSO == SOType.Resmine || _occupiedSO == SOType.BattleRestrictedObject)
        {
            InBattle = true;
        }
        else
        {
            InBattle = false;
        }
        
        MoveModule.UnStun();
    }

    public IEnumerator FakeBattleState(float duration)
    {
        if (_battleRoutine != null)
        {
            StopCoroutine(_battleRoutine);
        }
        ExitFromBattle();
        InBattle = true;
        yield return Utilis.GetWait(duration);
        if (!IsAlive) yield break;
        if (_occupiedSO == SOType.Resmine || _occupiedSO == SOType.BattleRestrictedObject)
        {
            InBattle = true;
            //_stunEffect = null;
            yield break;
        }
        InBattle = false;
        MoveModule.Go();
        //_stunEffect = null;
    }

    //Выход из битвы
    public virtual void ExitFromBattle()
    {
        if (!IsAlive) return;
        InBattle = false;
        if (CanMove)
        MoveModule.Go();
    }
    #endregion
    #region Нанесение урона и получение
    public virtual void DealDamage(IDamagable target)
    {
        CurrentAttack = Attack + UnityEngine.Random.Range(0, 3);
        UsePerks(PerkType.BeforeAttack, target);
        if (CurrentAttack == 0) return;
        target.GetDamage(CurrentAttack + (IsIgnoreArmor ? target.Armor : 0), this);
        UsePerks(PerkType.AfterAttack, target);
    }
    public virtual void GetDamage(int damage, IAttackable sourse)
    {
        CurrentDamage = damage;
        UsePerks(PerkType.BeforeDamage, sourse as IDamagable);
        if (CurrentDamage == 0) return;
        EventBus.HitUnitEvent?.Invoke(transform);
        EventBus.HitSoundEvent?.Invoke(Data.HitSound);
        Health -= Mathf.Max(1, (CurrentDamage - Armor));
        UsePerks(PerkType.AfterDamage, sourse as IDamagable);
    }
    public void GetTrueDamage(int damage)
    {
        EventBus.HitUnitEvent?.Invoke(transform);
        EventBus.HitSoundEvent?.Invoke(Data.HitSound);
        Health -= Mathf.Max(1, damage);
    }
    private void ShowDamageVisual(int oldValue, int newValue)
    {
        if (oldValue > newValue)
        {
            EventBus.DamagableDamaged?.Invoke(oldValue - newValue, transform);
            ShaderModule.PlayUnitHit();
        }
        else if (oldValue < newValue)
        {
            EventBus.DamagableHealed?.Invoke(newValue - oldValue, transform);
        }
    }

    public void Heal (int healAmount)
    {
        if (Health < MaxHealth)
        {
            Health += healAmount;
        }
    }

    #endregion


    public virtual void ImmobilizeUnit(float seconds) => MoveModule.ImmobilizeUnit(seconds);


    public abstract void PowerPlaceEnable();
    public abstract void PowerPlaceDisable();
    protected void UsePerks(PerkType type, IDamagable another)
    {
        var list = PerkBaseList.FindAll(p => p.perkType == type);

        if (list.Count > 0)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                list[i].UsePerk(another, this);
                if (list[i].Disposable) list[i].RemovePerk(this);
                if ((type == PerkType.BeforeAttack && CurrentAttack == 0 )|| (type == PerkType.BeforeDamage && CurrentDamage == 0) ) return;
            }
        }
    }

    /// <summary>
    /// Callback on unit, detected in fightzone
    /// </summary>
    /// <param name="unit"></param>
    protected virtual void OnUnitInFightZone(UnitBase unit) { }

    public bool IsFrendly(Player targetOwner)
    {
        return Owner == targetOwner;
    }

    public IncomingAura CreateAura(AuraSize size, Action<IncomingAura> callback)
    {
        var aura = Instantiate<IncomingAura>(GameLibrary.Instance.Aura, transform);
        aura.SetAuraSize(size);
        callback(aura);
        return aura;
    }
    /// <summary>
    ///  Корутина для повторяющегося эффекта с определенным интервалом, пока юнит жив
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    protected IEnumerator RepeatEffect(float interval, Action effect)
    {
        yield return Utilis.GetWait(interval);
        while (IsAlive)
        {
            effect();
            yield return Utilis.GetWait(interval);
        }
    }


    protected bool AreEnemiesNear()
    {
        foreach (var col in Physics2D.OverlapCircleAll(transform.position, (float)AttackDistanceType.Melee + 0.11f, LayerMask.GetMask("AllyUnits", "EnemyUnits")))
        {
            if (col.TryGetComponent<UnitBase>(out UnitBase unit) && !unit.IsFrendly(Owner))
            {
                return true;
            }
        }
        return false;
    }

    public void BuffStat(UnitStats stats, float duration = 0f)
    {
        _stats += stats;
        MaxHealth += stats.Health;
        RefreshStats();
        if (duration > 0f)
        {
            StartCoroutine(EndBuffOrDebuff(duration, () => _stats -= stats));
        }
    }
    public void DebuffStat(UnitStats stats, float duration = 0f)
    {
        _stats -= stats;
        MaxHealth -= stats.Health;
        RefreshStats();
        if (duration > 0f)
        {
            StartCoroutine(EndBuffOrDebuff(duration, () => _stats += stats));
        }
    }
    private void RefreshStats()
    {
        Health = _stats.Health;
        Attack = _stats.Attack;
        Armor = _stats.Armor;
        AttackSpeed = _stats.AttackSpeed;
        Speed = _stats.Speed;
    }

    private IEnumerator EndBuffOrDebuff(float duration, Action callback)
    {
        yield return Utilis.GetWait(duration);
        callback();
        RefreshStats();
    }

    public bool HasPerk(UnitPerksSO perk)
    {
        return PerkBaseList.Contains(perk.Perkprefab);
    }


    public void SetAsBusy(SOType type)
    {
        MoveModule.Stop();
        _isBusy = true;
        SortingOrder += 1;
        _occupiedSO = type;
        switch (type)
        {
            case SOType.Resmine:
                InBattle = true;
                break;
            case SOType.PowerPlace:
                PowerPlaceEnable();
                break;
            case SOType.BattleRestrictedObject:
                InBattle = true;
                break;
            default:
                break;
        }
    }

    private IEnumerator SetAsUnbusy(SOType type)
    {
        yield return Utilis.GetWait(0.9f);
        if (IsBusy)
        {

            switch (type)
            {
                case SOType.None:
                    break;
                case SOType.Resmine:
                    InBattle = false;
                    break;
                case SOType.PowerPlace:
                    PowerPlaceDisable();
                    break;
                case SOType.BattleRestrictedObject:
                    InBattle = false;
                    break;
                default:
                    break;
            }
            IsBusy = false;
            SortingOrder -= 1;
            _occupiedSO = SOType.None;

        }
    }

    public void RemoveParticle(string particleName)
    {
        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            if (particle.name == particleName)
            {
                particle.Clear();
                particle.Stop();
                break;
            }
        }
    }

    public float GetAnimationLenght()
    {
        return _animationModule.GetAnimationLenght();
    }
    public void SpecialAction()
    {
        _animationModule.PerformSpecial();
    }
    public void RandomMove()
    {
        MoveModule.RandomMove();
    }

    protected virtual void Die()
    {
        LocalDieEvent?.Invoke(this);
        FightZone.gameObject.SetActive(false);
        StopAllCoroutines();
        InBattle = false;
        _animationModule.PerformDeath();

        EventBus.DieSoundEvent?.Invoke(Data.DieSound);
        SortingOrder = Math.Max(0, SortingOrder - 1);
        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            particle.Clear();
            particle.Stop();
        }
        StartCoroutine(removeUnit());

        IEnumerator removeUnit()
        {
            yield return Utilis.GetWait(5f);
            Destroy(gameObject);
        }

    }
    public void KillImmediate()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        FightZone.UnitInFightZone -= OnUnitInFightZone;
       UnitIsDead?.Invoke(this);
    }

    protected void RaiseDieEvent()
    {
        UnitIsDead?.Invoke(this);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        MoveModule.CheckDirection();
        if (collision.gameObject.layer != 10 && IsBusy)
        {
            StartCoroutine(SetAsUnbusy(_occupiedSO));
        }
    }

    private void OnTransformParentChanged()
    {
        UnitIsDead?.Invoke(this);
        Owner = GetComponentInParent<Player>();
        tag = Owner.tag;
        gameObject.layer = Owner.gameObject.layer;
        FightZone.SetUnitRingColor();
        UnitIsSpawned?.Invoke(this);
        ShaderModule.SetUnitColor();
    }

}
