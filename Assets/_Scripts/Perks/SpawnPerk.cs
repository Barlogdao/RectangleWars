using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPerk : PerkBase
{
    [SerializeField]
    UnitDataSO spawnedUnitData;
    [SerializeField]
    ParticleSystem mainParticle;
    [SerializeField]
    AudioClip sound;
    [SerializeField]
    private bool Ressurectable; // Если true, то спавнит из трупов
    [SerializeField]
    private float spawnFrequency;
    [SerializeField]
    AuraSize _size;

    public override void UsePerk(IDamagable unit1, UnitBase unit2)
    {
        
    }
    public override void InitializePerk(UnitBase unit)
    {
        if(Ressurectable)
            unit.StartCoroutine(Ressurect(unit));
        else
            unit.StartCoroutine(SpawnUnit(unit));
    }

    public override string[] GetParams()
    {
        return new string[] {spawnedUnitData.Name, GetAuraRadius(_size).ToString(), spawnFrequency.ToString()};
    }

    // Спавн юнита из мертвого тела
    IEnumerator Ressurect(UnitBase unit)
    {
        yield return Utilis.GetWait(1f);
        while (unit.IsAlive)
        {
            yield return Utilis.GetWait(spawnFrequency);
            if (unit.InBattle) continue;

            foreach (var a in Physics2D.OverlapCircleAll(unit.transform.position, GetAuraRadius(_size), LayerMask.GetMask("AllyUnits", "EnemyUnits")))
            {
                if (a.TryGetComponent(out UnitBase target) && !target.IsAlive)
                {
                    target.StopAllCoroutines();
                    var res = spawnedUnitData.SpawnUnit(target.transform.position, unit.Owner.transform);
                    target.KillImmediate();
                    Instantiate(mainParticle, res.transform.position.AddY(0.5f), Quaternion.identity, res.transform);
                    EventBus.SoundEvent?.Invoke(sound);
          
                    unit.SpecialAction();
                    yield return null;
                    unit.ImmobilizeUnit(unit.GetAnimationLenght());
                    yield return null;
                    unit.SpecialAction();
                    break;
                }
            }
        }
    }

    IEnumerator SpawnUnit(UnitBase unit)
    {
        yield return Utilis.GetWait(spawnFrequency);
        while (unit.IsAlive)
        {
            var list = unit.Owner.BattlefieldManager.MManager.GetFreeTiles(unit.transform.position, GetAuraRadius(_size));
            if (list.Count > 0)
            {
                var res = spawnedUnitData.SpawnUnit(list[Random.Range(0, list.Count)], unit.Owner.transform);
                
                Instantiate(mainParticle, res.transform.position.AddY(0.5f), Quaternion.identity, res.transform);
                EventBus.SoundEvent?.Invoke(sound);
            }
            yield return Utilis.GetWait(spawnFrequency);
        }
    }
}
