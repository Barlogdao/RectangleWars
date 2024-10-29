using System.Collections;
using UnityEngine;

public class WarriorClass : UnitBase
{
    private IncomingAura _aura;
    

    public override void DealDamage(IDamagable target)
    {
        CurrentAttack = Attack + UnityEngine.Random.Range(0, 3);
        if (target.Class == ClassType.Scout)
        {
            CurrentAttack += BONUS_DAMAGE;
        }
        UsePerks(PerkType.BeforeAttack, target);
        if (CurrentAttack == 0) return;
        target.GetDamage(CurrentAttack + (IsIgnoreArmor ? target.Armor : 0), this);
        UsePerks(PerkType.AfterAttack, target);
    }


    public override void PowerPlaceEnable()
    {
        Armor += 5;
        _aura = CreateAura( AuraSize.Medium,aura => aura.UnitEnterAura += OnUnitEnterAura);

    }

    private void OnUnitEnterAura(UnitBase unit)
    {
        if (unit.IsAlive && IsAlive && !unit.IsFrendly(Owner))
        {
            Instantiate(VfxProvider.Instance.Taunt, unit.transform.position.AddY(0.9f), Quaternion.identity, unit.transform);
            unit.MoveModule.MoveTo(transform.position);
        }
    }

    IEnumerator Taunt()
    {
        while (IsAlive)
        {
            yield return Utilis.GetWait(3f);
            foreach (var a in Physics2D.OverlapCircleAll(transform.position, 4f, LayerMask.GetMask("AllyUnits", "EnemyUnits")))
            {
                if (!CompareTag(a.tag))
                {
                    a.TryGetComponent<UnitBase>(out var unit);
                    unit.MoveModule.MoveTo(transform.position);
                }
            }
        }
    }

    public override void PowerPlaceDisable()
    {
        Armor -= 5;
        Destroy(_aura.gameObject);
        
    }
}
