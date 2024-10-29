using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void DealDamage(IDamagable target);
    public bool IsIgnoreArmor { get; }
    int Attack { get; }
    ClassType Class { get; }
    Transform Transform { get; }
}
