using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using Redcode.Extensions;

public class MapManager : MonoBehaviour
{
    public Tilemap ground,sands,road,mug,water,forest,cureFields,wall;
    public TileBase[] groundMap, sandsMap, roadMap,mugMap,waterMap,forestMap,cureFieldsMap,wallMap;
    public List<Vector3> groundPos, sandsPos, roadPos,mugPos,waterPos, forestPos,cureFieldsPos,wallPos;
    
    [Space(10)]
    private List<Vector3> _avaliablePositions;
    [SerializeField]
    private LayerMask notoccupiedMask;
    [SerializeField]
    private LayerMask _wallsAndUnitsMask;
    public List<Vector3> AvaliablePositions => _avaliablePositions;

    private void Start()
    {
        
        groundPos = CalcArea(ground);
        sandsPos = CalcArea(sands);
        roadPos = CalcArea(road);
        mugPos = CalcArea(mug);
        waterPos = CalcArea(water);
        forestPos = CalcArea(forest);
        cureFieldsPos = CalcArea(cureFields);
        wallPos = CalcArea(wall);
        _avaliablePositions = groundPos.Except(wallPos).ToList();
    }

    private List<Vector3> CalcArea(Tilemap tm)
    {
       List<Vector3> list = new();
        foreach (var pos in tm.cellBounds.allPositionsWithin)
        {
            Vector3Int gridPlace = new (pos.x, pos.y, pos.z);
            if (tm.HasTile(gridPlace))
            {
               Vector3 worldPlace = ground.GetCellCenterWorld(gridPlace);
               list.Add(worldPlace);
            }
        }
        return list;
    }
    /// <summary>
    /// Возвращает список тайлов, доступных для размещения
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetFreeTiles()
    {
       return _avaliablePositions.Where(x => (Physics2D.OverlapPoint(x, notoccupiedMask) == null)).ToList();
    }

    public List<Vector3> GetFreeTiles(Vector3 position, float radius)
    {
        var b = Physics2D.OverlapCircle(position, radius);
        List<Vector3> a = _avaliablePositions.Where(x => Vector3.Distance(position,x)<radius).ToList();
        return _avaliablePositions.Where(x => (Physics2D.OverlapPoint(x, notoccupiedMask) == null)).Intersect(a).ToList();
    }



}
