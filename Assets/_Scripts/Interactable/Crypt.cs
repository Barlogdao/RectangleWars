using System.Collections;
using UnityEngine;


public class Crypt : DualStateStrategicObject
{
    [SerializeField] UnitDataSO _summonedUnitData;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] PointsHolder _pointsHolder;
    [SerializeField] ParticleSystem _spawnParticle;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsOccupied && other.TryGetComponent<UnitBase>(out UnitBase unit) && unit is not WorkerClass && unit is not SummonClass && unit.CanMove && unit.IsAlive)
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
    protected override void OccupieObject(UnitBase unit)
    {
        base.OccupieObject(unit);
        StartCoroutine(CryptWorkProcess(unit));
    }
    protected override void FreeObject(UnitBase unit)
    {
        base.FreeObject(unit);
        StopAllCoroutines();
        _pointsHolder.HidePointsVisual();
    }

    private IEnumerator CryptWorkProcess(UnitBase unit)
    {
        UpdateOutlineColor(unit);
        int counter = 0;
        _pointsHolder.ShowPointsVisual(counter);
        yield return Utilis.GetWait(_spawnDelay);
        while (unit != null && unit.IsAlive)
        {
            UpdateOutlineColor(unit);
            if (++counter == 4)
            {
                var spawned = SpawnUnit(unit.Data);
                
                yield return null;
                spawned.MoveModule.RandomMove();

            }
            else
            {
               SpawnUnit(_summonedUnitData);
            }
            _pointsHolder.ShowPointsVisual(counter);
            if (counter == 4)
            {
                counter = 0;
            }
            yield return Utilis.GetWait(_spawnDelay);
        }

        UnitBase SpawnUnit(UnitDataSO unitData)
        {
            var spawned = unitData.SpawnUnit(_spawnPoint.position.AddY(0.3f), unit.Owner.transform).GetComponent<UnitBase>();
            Instantiate(_spawnParticle, spawned.transform);
            return spawned;
        }
    }

}
