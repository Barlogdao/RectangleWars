using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomTower : StrategicObjectBase
{
    [SerializeField]
    private LightningBoltScript bolt;
    [SerializeField]
    private float _interval;
    [SerializeField]
    private int _damage;
    [SerializeField]
    GameObject _damageEmitionPoint;
    [SerializeField] ParticleSystem _doomTowerEye;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsOccupied && collision.TryGetComponent<UnitBase>(out UnitBase unit) && unit is not WorkerClass && unit is not SummonClass && unit.CanMove && unit.IsAlive)
        {
            IsOccupied = true;
            unit.SetAsBusy(_type);
            if (unit.IsBusy)
            {
               OccupieObject(unit);
               return;
            }
            IsOccupied = false;
        }
    }

    protected override void OnOccupiedStateChange()
    {
        if (IsOccupied)
        {
            _doomTowerEye.Play();
        }
        else
        {
            _doomTowerEye.Stop();
        }
    }

    protected override void OccupieObject(UnitBase unit)
    {
        base.OccupieObject(unit);
        StartCoroutine(Effect(unit));
    }

    protected override void FreeObject(UnitBase unit)
    {
        StopAllCoroutines();
        base.FreeObject(unit);
    }

    private IEnumerator Effect (UnitBase unit)
    {
        yield return Utilis.GetWait(_interval);
        while (unit != null && unit.IsAlive && unit.Owner.EnemyHero.IsAlive)
        {
            UpdateOutlineColor(unit);
            var Bolt = Instantiate(bolt, transform.position, Quaternion.identity);
            Bolt.StartObject = _damageEmitionPoint;
            Bolt.EndPosition = unit.Owner.EnemyHero.transform.position.AddY(0.5f);
            Destroy(Bolt.gameObject, 0.2f);
            unit.Owner.EnemyHero.GetTrueDamage(_damage);
            yield return Utilis.GetWait(_interval);
        }
    }


}
