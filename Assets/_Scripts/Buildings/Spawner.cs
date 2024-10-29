using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void SpawnUnit(UnitBase unit,Transform parent, float speed)
    {
        
        UnitBase a = Instantiate(unit, transform.position, Quaternion.Euler(0, 0, 0), parent);
        a.GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
    public void SpawnUnit(UnitDataSO unitData, Transform parent)
    {
        unitData.SpawnUnit(transform.position, parent).GetComponent<Rigidbody2D>().AddForce(transform.up * unitData.Speed, ForceMode2D.Impulse);
        //var unit = Instantiate(GameLibrary.Instance.UnitPrefab, transform.position, Quaternion.Euler(0, 0, 0), parent);
        //spawnedUnitData.GetClass(unit);
        
        ////unit.GetComponent<UnitBase>().Data = spawnedUnitData;        
        //unit.
    }

}
