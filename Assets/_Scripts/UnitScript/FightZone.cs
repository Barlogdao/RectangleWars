using System;
using UnityEngine;
using Redcode.Extensions;

public class FightZone : MonoBehaviour
{
    UnitBase unit;
    public event Action<UnitBase> UnitInFightZone;
    [SerializeField]
    private SpriteRenderer _backArc, _frontArc;
    
    public CircleCollider2D FightRadius;

    private void Awake()
    {
        FightRadius = GetComponent<CircleCollider2D>();
    }
    public void InitFightZone()
    {
        unit = GetComponentInParent<UnitBase>();
        
        gameObject.layer = 3;
        FightRadius.radius *= (int)unit.AttackDistance;
        FightRadius.radius += 0.1f;
        SetUnitRingColor();
    }
    public void SetUnitRingColor()
    {
        tag = unit.tag;
        _backArc.color = _frontArc.color = unit.Owner.PlayerColor;
    }

    public bool EnemyInRadius(Collider2D target) => FightRadius.IsTouching(target);
    private bool IsEnemy(string targetTag) => !CompareTag(targetTag);

    //только когда триггер натыкается на коллайдер
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!unit.InBattle)
        {
            if (IsEnemy(collision.tag) && 
                collision.TryGetComponent(out IDamagable target) && 
                target.IsAlive)
            {
                unit.StartBattle(target, collision);
                return;
            }
        }
    }
    // Триггер чтобы сообщать юниту,что в зону вошло тело
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out UnitBase other) && unit.IsAlive)
        {
            UnitInFightZone?.Invoke(other);
        }
    }
}
