using UnityEngine;
using System.Linq;


public class CallOfTheDead : AbilityBase
{
    [SerializeField]
    UnitDataSO spawnedUnitData;

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetAllUnits().Where(target => !target.IsAlive).Count() > 2;
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        foreach (var unit in battlefieldManager.GetAllUnits().Where(target => !target.IsAlive))
        {
           Ressurect(unit,player);
        }

       void Ressurect(UnitBase target, Player player)
       {

            target.StopAllCoroutines();
            var res = spawnedUnitData.SpawnUnit(target.transform.position, player.transform);
            Instantiate(m_ParticleSystem, res.transform.position.AddY(0.5f), Quaternion.identity, res.transform);
            target.KillImmediate();
       }
    }

    
}
