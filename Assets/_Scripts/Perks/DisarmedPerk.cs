using System.Collections;
using UnityEngine;



public class DisarmedPerk : PerkBase
{
    [SerializeField] private float _duration;
    public override void UsePerk(IDamagable another, UnitBase self)
    {
  
    }
    public override void InitializePerk(UnitBase self)
    {
        base.InitializePerk(self);
        self.FightZone.FightRadius.enabled = false;
        self.StartCoroutine(DisarmUnit(self));
    }

    protected override void OnRemovePerk(UnitBase self)
    {
        base.OnRemovePerk(self);
        self.FightZone.FightRadius.enabled = true;
    }
    private IEnumerator DisarmUnit(UnitBase unit)
    {
        
        yield return Utilis.GetWait(_duration);
        RemovePerk(unit);
    }
    public override string[] GetParams()
    {
        return new string[] {_duration.ToString()};
    }
}
