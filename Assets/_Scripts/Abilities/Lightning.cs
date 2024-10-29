using UnityEngine;
using DigitalRuby.LightningBolt;
using System.Linq;

public class Lightning : AbilityBase
{
    [SerializeField]
    LightningBoltScript bolt;
    [SerializeField]
    int damage;
    [SerializeField]
    private int _bonusPerLvl;
    public override string[] GetParams(Hero hero)
    {
        return new string[] { (damage).ToString()};
    }

    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive).Any(u => u is not WorkerClass);
    }

    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        var allunits = battlefieldManager.GetEnemyUnits(player).Where(target => target.IsAlive).ToList();
        UnitBase unit = null;
        if (allunits.Count > 0)
        {
            unit = allunits[Random.Range(0, allunits.Count)];
            var Bolt = Instantiate(bolt, unit.transform.position, Quaternion.identity);
            Instantiate(m_ParticleSystem,unit.transform.position, Quaternion.identity);
            Destroy(Bolt.gameObject, 0.2f);
            unit.GetTrueDamage(damage);
            base.UseAbility(battlefieldManager, player);
        }
    }
}
