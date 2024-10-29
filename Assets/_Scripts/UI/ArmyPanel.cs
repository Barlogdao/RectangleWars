using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmyPanel : MonoBehaviour
{

    [SerializeField]
    private UnitInfo unitInfoPrefab;
    private List<UnitInfo> _armyList;
    private UnitInfo _selectedUnit;
    private HumanPlayer _player;

    private readonly KeyCode[] _keyarray = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U };

    public void InitBar(Hero hero, HumanPlayer player)
    {
        _player = player;
        _armyList = new List<UnitInfo>();

        //CreateWorkerBar(hero, player);
        //CreateUnitBars(hero, player);
        InitUnitBars(hero, player);
        if (_armyList.Count > 0)
        {
            _armyList.ForEach(info => info.UnitInfoSelected += OnUnitInfoSelected);
            PlayerInputController.Instance.SummonUnitPressed += OnSummonUnitPressed;
            PlayerInputController.Instance.UnitSelectionPressed += OnUnitSelectionPressed;
            _selectedUnit = _armyList[0];
            _selectedUnit.SelectUnit();
        }

    }

    public void OnSummonUnitPressed()
    {
        _player.BuyUnit(_selectedUnit.UnitData);
    }

    private void OnUnitInfoSelected(UnitInfo selected)
    {
        if (selected == null) return;
        if (_selectedUnit != selected) _selectedUnit = selected;
        foreach (var unitInfo in _armyList)
        {
            if (unitInfo != _selectedUnit)
            {
                unitInfo.DeselectUnit();
            }
        }
    }

    private void OnUnitSelectionPressed(int value)
    {
        _selectedUnit.DeselectUnit();
        if (value > 0)
        {
            _selectedUnit = _armyList[(GetIndexOfUnit(_selectedUnit) + 1) % _armyList.Count];
        }
        else
        {
            _selectedUnit = _armyList[GetIndexOfUnit(_selectedUnit) - 1 >= 0 ?
                GetIndexOfUnit(_selectedUnit) - 1 :
                _armyList.Count - 1];
        }

        _selectedUnit.SelectUnit();
    }
    private int GetIndexOfUnit(UnitInfo unit)
    {
        return _armyList.IndexOf(unit);
    }

    private void InitUnitBars(Hero hero, HumanPlayer player)
    {
        var units = hero.GetHeroUnits();
        for (int i = 0; i < units.Count; i++)
        {
            var prefab = Instantiate(unitInfoPrefab, transform);
            prefab.InitUnitBar(_keyarray[i], units[i], player);
            _armyList.Add(prefab);
        }
    }

    private void CreateWorkerBar(Hero hero, HumanPlayer player)
    {
        UnitInfo worker = Instantiate(unitInfoPrefab, transform);
        worker.InitUnitBar(_keyarray[0], GameLibrary.Instance.Fractions.GetWorker(hero), player);
        _armyList.Add(worker);

    }
    private void CreateUnitBars(Hero hero, HumanPlayer player)
    {
        for (int i = 0; i < hero.StartUnit.Count; i++)
        {
            var prefab = Instantiate(unitInfoPrefab, transform);
            prefab.InitUnitBar(_keyarray[i + 1], hero.StartUnit[i], player);
            _armyList.Add(prefab);
        }
    }
    //private void OnDestroy()
    //{
    //    _armyList.ForEach(info => info.UnitInfoSelected -= OnUnitInfoSelected);
    //}
}
