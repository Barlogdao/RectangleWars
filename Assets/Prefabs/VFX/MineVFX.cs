using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineVFX : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _particleOne, _particleTwo;

    public void VFXON(Player player)
    {
        _particleOne.Play();
        _particleTwo.Play();
    }
    public void VFXOFF()
    {
        _particleOne.Stop(); 
        _particleTwo.Stop();
    }

}
