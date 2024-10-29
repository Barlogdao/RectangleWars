using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonClass : UnitBase
{
    protected override void Awake()
    {
        base.Awake();
        _bodyCollider.enabled = false;
        InBattle = true;

    }
    protected override void OnStart()
    {
        StartCoroutine(Summon());
    }
    IEnumerator Summon()
    {
        yield return null;
        yield return Utilis.GetWait(GetAnimationLenght());

        _bodyCollider.enabled = true;
        InBattle = false;
        MoveModule.RandomMove();
    }
    protected override void Die()
    {
        base.Die();
        _bodyCollider.enabled = false;
        RaiseDieEvent();

    }

    public override void PowerPlaceDisable() { }
    public override void PowerPlaceEnable() { }

}
