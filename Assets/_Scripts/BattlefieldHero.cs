using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BattlefieldHero : MonoBehaviour, IDamagable, IAttackable
{
    public Hero Hero;
    private Player _player;
    protected SpriteRenderer image;
    TextMeshProUGUI healthUI;
    [SerializeField] protected AudioClip hitsound;
    [SerializeField] protected AudioClip destroysound;
    Animator animator;
    Material mat;
    public event Action HeroDamaged;
    public event Action<int> HeroHealthChanged;

    private int _health, _armor, _attack;
    private bool _getLethalDamage = false;

    private IEnumerator _teleportRoutine;
    private bool _isUnKill = false;

    public int Health
    {
        get => _health; set
        {
            ShowDamageVisual(_health, value);

            int oldValue = _health;
            _health = OnHealthChanged(value);
            healthUI.text = Health.ToString();
            if (!IsAlive) OnHeroDie();

        }
    }
    //public int MaxHealth => GameManager.Instance.Settings.HeroHealth + (5 * Hero.Stamina);
    public int MaxHealth { get; private set; }
    public int Armor { get => _armor; set => _armor = Mathf.Max(0, value); }
    public int Attack { get => _attack; set => _attack = Mathf.Max(0, value); }
    public bool IsAlive => Health > 0;
    public bool IsIgnoreArmor => true;
    public bool GetLethalDamage => _getLethalDamage;

    public ClassType Class => ClassType.Hero;
    public Transform Transform => transform;
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthUI = GetComponentInChildren<TextMeshProUGUI>();
        mat = GetComponent<SpriteRenderer>().material;
        _teleportRoutine = TeleportEffect();
    }


    public void InitHero(Transform playerTransform)
    {
        _player = GetComponentInParent<Player>();
        tag = _player.tag;
        Hero = _player.Hero;
        animator.runtimeAnimatorController = Hero.Animator;

        ColorChange(_player.PlayerColor);

        _health = GameManager.Instance.Settings.HeroHealth;
        Armor = GameManager.Instance.Settings.HeroArmor;
        Attack = GameManager.Instance.Settings.HeroAttack;
        _player.SetHeroBuffs(this);
        MaxHealth = _health;



        CheckDirection();

        image.enabled = false;
        healthUI.enabled = false;
    }
    public void ShowHeroModel()
    {
        image.enabled = true;
    }
    public void InitHeroUI()
    {
        healthUI.enabled = true;
        healthUI.text = Health.ToString();
        GetComponentInChildren<Arrow>().Init(_player);
    }

    public void DealDamage(IDamagable target)
    {
        if (!IsAlive) return;
        if (target != null && target is UnitBase unit && unit.IsAlive && unit.Class != ClassType.Assassin)
        {
            CheckDirection(target);
            animator.Play(AnimatorStates.Attack);
            target.GetDamage(Attack + target.Armor,this);
        }
    }

    protected void ResetUnitDisposablePerks(PerkType type, UnitBase unit)
    {
        var list = unit.PerkBaseList.FindAll(p => p.perkType == type && p.EffectType == PerkEffectType.buff);
        if (list.Count < 1) return;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Disposable)
            {
                list[i].RemovePerk(unit);
            }
        }
    }

    public void GetDamage(int damage, IAttackable sourse)
    {
        if (_isUnKill) return;
        EventBus.HitUnitEvent?.Invoke(transform);
        if (hitsound != null) EventBus.SoundEvent?.Invoke(hitsound);
        // Хит Эффект
        StartCoroutine(HitEffect(0.1f));
        Health -= Mathf.Max(MinimalDamage(sourse.Class), damage - Armor);
        if (IsAlive)
        {
            DealDamage(sourse as IDamagable);
            _player.InsightLevel += Mathf.Max(MinimalDamage(sourse.Class), damage - Armor);
            StopCoroutine(_teleportRoutine);
            _teleportRoutine = TeleportEffect();
            StartCoroutine(_teleportRoutine);
        }
        else animator.Play(AnimatorStates.Die);
    }
    private int MinimalDamage(ClassType classType)
    {
        return classType == ClassType.Worker ? 1 : 10;
    }


    public void GetTrueDamage(int damage)
    {
        if (_isUnKill) return;
        EventBus.HitUnitEvent?.Invoke(transform);
        if (hitsound != null) EventBus.SoundEvent?.Invoke(hitsound);
        StartCoroutine(HitEffect(0.1f));
        Health -= Mathf.Max(1, damage);
        if (IsAlive)
        {
            _player.InsightLevel += Mathf.Max(1, damage);
        }
    }

    private void ShowDamageVisual(int oldValue, int newValue)
    {
        if (oldValue >= newValue)
        {
            EventBus.DamagableDamaged?.Invoke(oldValue - newValue, transform);
            HeroDamaged?.Invoke();
        }
        else EventBus.DamagableHealed?.Invoke(newValue - oldValue, transform);
    }
    int OnHealthChanged(int value)
    {
        if (value <= 0 && _getLethalDamage == false)
        {
            _getLethalDamage = true;
            StartCoroutine(LethalDamageResolve(2f));
            HeroHealthChanged?.Invoke(Mathf.Min(5, _health));
            return Mathf.Min(5, _health);
        }
        HeroHealthChanged?.Invoke(Mathf.Clamp(value, 0, MaxHealth));
        return Mathf.Clamp(value, 0, MaxHealth);
    }
    private IEnumerator LethalDamageResolve(float duration)
    {
        UnKillOn();
        var field = Instantiate(GameLibrary.Instance.ForceField, transform.position, Quaternion.identity);
        field.Init(_player, Attack, duration);
        
        yield return Utilis.GetWait(duration);
        UnkillOFF();
    }

    private void UnKillOn()
    {
        _isUnKill = true;
        mat.SetFloat("_OutlineValue", 1f);
    }
    private void UnkillOFF()
    {
        mat.SetFloat("_OutlineValue", 0f);
        _isUnKill = false;
    }

    private void OnHeroDie()
    {
        // Анимация cмерти
        GetComponent<Collider2D>().enabled = false;
        animator.Play(AnimatorStates.Die);
        healthUI.enabled = false;
        EventBus.SoundEvent?.Invoke(destroysound);
        EventBus.GameOverEvent?.Invoke(this);
    }

    public IEnumerator HitEffect(float duration)
    {
        mat.SetFloat("_HitValue", 1f);
        yield return Utilis.GetWait(duration);
        mat.SetFloat("_HitValue", 0f);
    }

    public void PlaySpecialAnim()
    {
        if (!IsAlive) return;
        animator.Play(AnimatorStates.Special);
        CheckDirection();
    }
    private void ColorChange(Color color)
    {
        mat.SetColor("_PartColor", color);
    }

    private void CheckDirection()
    {
        image.flipX = (transform.position.x > 0);
    }
    private void CheckDirection(IDamagable target)
    {
        if (target is UnitBase other)
        {
            image.flipX = (transform.position.x > other.transform.position.x);
        }
    }

    public void Buff(int health, int attack, int armor)
    {
        _health += health;
        _attack += attack;
        _armor += armor;
    }

    private IEnumerator TeleportEffect()
    {
        var newSpawn = _player.BattlefieldManager.GetNewHeroPosition(this);
        if (!IsAlive || newSpawn == null) yield break;
        UnKillOn();
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(HitEffect(0.3f));
        transform.position = newSpawn.position;
        UnkillOFF();
    }

}
