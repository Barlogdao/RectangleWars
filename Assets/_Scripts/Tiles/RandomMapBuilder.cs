using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapBuilder : MonoBehaviour
{
    [SerializeField] private MapZone _playerZone, _neutralZone, _enemyZone;
    [SerializeField] BattleSettings _battleSettings;
    [SerializeField] private StrategicObjectBase[] _strategicObjects;
    [SerializeField]
    private LayerMask _objectLayerMask;
    [SerializeField] private float _minimalObjectDistance = 10f;

    private List<Vector3> _placedObjects = new();
    public void Init()
    {
        SetHeroPoint(_playerZone, _battleSettings.HumanStartPoint);
        SetHeroPoint(_enemyZone, _battleSettings.EnemyStartPoint);
        SetStrategicObject(_playerZone);
        SetStrategicObject(_enemyZone);
        SetStrategicObject(_neutralZone);
        SetStrategicObject(_neutralZone);
    }

    private void SetHeroPoint(MapZone zone, Transform point)
    {
        point.position = zone.GetRandomPoint();
        _placedObjects.Add(point.position);
    }
    private void SetStrategicObject(MapZone zone)
    {
        var so = _strategicObjects[Random.Range(0, _strategicObjects.Length)];
        Vector3 point = zone.GetRandomPoint();
        while (Physics2D.OverlapCircle(point, 3f, _objectLayerMask) != null
             && !PointIsFarFromOtherObjects(point))
        {
            point = zone.GetRandomPoint();
        }
        Instantiate(so, point, Quaternion.identity);
        _placedObjects.Add(point);
    }

    private bool PointIsFarFromOtherObjects(Vector3 expectedpoint)
    {
        foreach( var point in _placedObjects)
        {
            if(Vector3.Distance(expectedpoint,point) < _minimalObjectDistance)
            {
                return false;
            }
        }
        return true;
    }
}
