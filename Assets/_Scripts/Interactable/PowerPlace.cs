using System.Collections;
using UnityEngine;



public class PowerPlace : DualStateStrategicObject
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsOccupied && collision.TryGetComponent<UnitBase> (out UnitBase unit) && unit is not WorkerClass && unit is not SummonClass && unit.CanMove && unit.IsAlive) 
        {
            IsOccupied = true;
            unit.SetAsBusy(_type);
            if (unit.IsBusy) 
            {
                OccupieObject(unit);
                StartCoroutine(CheckUnitColor(unit));
                return;
            }
            IsOccupied = false;

        }
    }

    private IEnumerator CheckUnitColor(UnitBase unit)
    {
        
        yield return Utilis.GetWait(1f);
        while(unit != null && unit.IsAlive && unit.IsBusy)
        {
            UpdateOutlineColor(unit);
            yield return Utilis.GetWait(1f);
        }

    }

}
