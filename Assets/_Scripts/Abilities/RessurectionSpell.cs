
using System.Collections;
using System.Linq;


public class RessurectionSpell : AbilityBase
{
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
       return battlefieldManager.GetAllyUnits(player).Where(target => !target.IsAlive).Count() > 2;
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        foreach(var unit in battlefieldManager.GetAllyUnits(player))
        {
            if (!unit.IsAlive)
            {
                battlefieldManager.StartCoroutine(Ressurect(unit,player));
            }
        }
    }

    private IEnumerator Ressurect(UnitBase target, Player player)
    {
        target.StopAllCoroutines();
        var unitData = target.Data;
        var position = target.transform.position;
        target.KillImmediate();
        Instantiate(m_ParticleSystem, position,UnityEngine.Quaternion.identity);
        yield return Utilis.GetWait(0.3f);
        var res = unitData.SpawnUnit(position, player.transform).GetComponent<UnitBase>();
        yield return Utilis.GetWait(0.1f);
        res.MoveModule.RandomMove();
    }

}
