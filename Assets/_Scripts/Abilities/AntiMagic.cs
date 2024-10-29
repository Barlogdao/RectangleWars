using System.Collections;
using UnityEngine;

public class AntiMagic : AbilityBase
{
    [SerializeField] private float _duration;
    public override bool Resolver(BattlefieldManager battlefieldManager, Player player)
    {
        return battlefieldManager.GetAllyUnits(player).Count > 3;
    }
    public override void UseAbility(BattlefieldManager battlefieldManager, Player player)
    {
        base.UseAbility(battlefieldManager, player);
        foreach(var unit in battlefieldManager.GetAllyUnits(player))
        {
            if (unit.IsAlive)
            {
                unit.StartCoroutine(SetAntimagic(unit));
                RunParticle(ShowParticleOnUnit(unit), _duration);
            }
        }

    }
    public override string[] GetParams(Hero hero)
    {
        return new string[] { (_duration).ToString() };
    }

    private IEnumerator SetAntimagic(UnitBase unit)
    {
        unit.ImmuneToMagic = true;
        yield return Utilis.GetWait(_duration);
        unit.ImmuneToMagic = false;
    }

}
