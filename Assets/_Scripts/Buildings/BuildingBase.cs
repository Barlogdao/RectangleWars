using UnityEngine;

public class BuildingBase : MonoBehaviour, IDamagable
{
    
    protected int _health;
    public virtual int Health {
        get => _health;
        set {
            _health = OnHealthChanged(value); 
            if(!IsAlive)
                Destroy(gameObject);
        }
    }
    protected int _armor;
    public virtual int Armor { get => _armor; set { _armor = Mathf.Max(0, value); } }

    public bool IsAlive { get => Health > 0; }

    public ClassType Class => ClassType.None;

    protected string onDestroyText;

    [SerializeField]

    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected AudioClip hitsound;
    [SerializeField]
    protected AudioClip destroysound;

    protected virtual void Start()
    {
                     
    }
      
    public virtual void GetDamage(int damage, IAttackable sourse)
    {
        EventBus.HitUnitEvent?.Invoke(transform);
        if (hitsound != null) 
            EventBus.SoundEvent?.Invoke(hitsound);
        Health -= Mathf.Max(1, (damage - Armor));
        
    }



    protected int OnHealthChanged(int value)
    {
        // Попап с уроном
        if (_health >= value) EventBus.DamagableDamaged?.Invoke(_health - value, transform);
        else EventBus.DamagableHealed?.Invoke(value - _health, transform);
        return Mathf.Max(0, value);
    }

    public void GetTrueDamage(int damage)
    {
        EventBus.HitUnitEvent?.Invoke(transform);
        if (hitsound != null)
            EventBus.SoundEvent?.Invoke(hitsound);
        Health -= Mathf.Max(1, damage);
    }
}
