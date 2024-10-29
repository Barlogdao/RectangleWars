using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageArea : MonoBehaviour
{

    private int _damage;
    public void Init(int damage, string tag)
    {
        this.tag = tag;
        _damage = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitBase>(out UnitBase unit) && unit.IsAlive && !CompareTag(collision.tag))
        {
            unit.GetTrueDamage(_damage);
        }
    }

}
