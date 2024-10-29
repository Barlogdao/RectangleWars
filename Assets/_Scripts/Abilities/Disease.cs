using System.Linq;
using UnityEngine;

public class Disease : AbilityBase
{
    [SerializeField] private UnitPerksSO _diseasePerkSO;
    [SerializeField] private DiseasePerk _diseasePerk;
    
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetEnemyUnits(player).Count(unit=> unit.IsAlive && !unit.HasPerk(_diseasePerkSO)) >2;
    }
    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        var avaliableUnits = battlefieldManager.GetEnemyUnits(player);
        if (avaliableUnits.Count > 0 )
        {
            avaliableUnits[Random.Range(0,avaliableUnits.Count)].AddPerk(_diseasePerkSO);
        }
    }

    public override string[] GetParams(Hero hero)
    {
        return new string[] { (_diseasePerk._damage).ToString(),_diseasePerk._duration.ToString() };
    }

}



