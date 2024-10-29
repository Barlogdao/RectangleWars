using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortify : BuildingBase
{
    ParticleSystem particle;

    public override int Health
    {
        get => _health;
        set
        {
            _health = OnHealthChanged(value);

            if (_health <= 0)
            {
                particle.Play();
            }
        }
    }
    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        _health = 50;
        _armor = 1;
       
    }


}
