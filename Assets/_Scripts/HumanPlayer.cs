using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{

    //События для начала визуализации перезарядки спауна
    public event Action<float> OnStartCD;
    public override void OnStart()
    {
        BattlefieldHero.HeroDamaged += OnHeroDamaged;
        
    }
    public override void OnDestroyHandler()
    {
        BattlefieldHero.HeroDamaged -= OnHeroDamaged;
    }

    public void BuyUnit(UnitDataSO unitData)
    {
        if (!SpawnerOnCD && unitData.CanBuyUnit(this) && Time.timeScale != 0)
        {
            CreateUnit(unitData);
        }
    }
    protected override IEnumerator SpawnerReload()
    {
        OnStartCD?.Invoke(UnitCD);
        yield return new WaitForSeconds(UnitCD);
        SpawnerOnCD = false;

    }
    private void OnHeroDamaged()
    {
        CameraShake.CameraShaked?.Invoke();
    }


}
