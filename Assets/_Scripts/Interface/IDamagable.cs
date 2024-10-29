

using System;
using UnityEngine;

public interface IDamagable
{
    void GetDamage(int damage, IAttackable sourse);
    void GetTrueDamage(int damage);
    bool IsAlive { get; }
    int Health { get; }
    int Armor { get; }
   
    ClassType Class{get;}

}


