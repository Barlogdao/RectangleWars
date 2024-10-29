
using System.Collections;
using UnityEngine;

public class MagicAcademy : StrategicObjectBase
{
    [SerializeField] PointsHolder _pointsHolder;
    [SerializeField] private float _earningFreeSpellPontCooldown;
    private Animator _animator;


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
        StartCoroutine(MagicAcademyWorkProcess(unit));
    }
    protected override void FreeObject(UnitBase unit)
    {
        base.FreeObject(unit);
        StopAllCoroutines();
        unit.Owner.FreeSpellCounterChanged -= OnFreeSpellCounerChanged;
        _pointsHolder.HidePointsVisual();
    }

    private IEnumerator MagicAcademyWorkProcess(UnitBase unit)
    {
        UpdateOutlineColor(unit);
        _pointsHolder.ShowPointsVisual(unit.Owner.FreeSpellCounter);
        unit.Owner.FreeSpellCounterChanged += OnFreeSpellCounerChanged;
        yield return Utilis.GetWait(_earningFreeSpellPontCooldown);
        while (unit != null && unit.IsAlive)
        {
            UpdateOutlineColor(unit);
            unit.Owner.FreeSpellCounter++;
            yield return Utilis.GetWait(_earningFreeSpellPontCooldown);
        }
          
    }

    private void OnFreeSpellCounerChanged(int counterValue)
    {
        _pointsHolder.ShowPointsVisual(counterValue);
    }
}
