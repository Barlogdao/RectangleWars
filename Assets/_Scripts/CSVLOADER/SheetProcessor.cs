using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SheetProcessor : MonoBehaviour
{

    private const int _unitName = 0;
    private const int _health = 1;
    private const int _attack = 2;
    private const int _armor = 3;
    private const int _attackSpeed = 4;
    private const int _speed = 5;
    private const int _cost = 6;

    private const char _cellSeparator = ',';
    private const int _inCellSeparator = ';';

    public Dictionary<string, (UnitStats, int)> ProcessData (string csvRawData)
    {
        char lineEndings = GetPlatformSpecificLineEnd();
        string[] rows = csvRawData.Split(lineEndings);
        int dataStartRawIndex = 1;
        Dictionary<string, (UnitStats,int)> statDictionary = new();
        for (int i = dataStartRawIndex; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(_cellSeparator);
            string unitName = cells[_unitName];
            int health = ParseInt(cells[_health]);
            int attack = ParseInt(cells[_attack]);
            int attackSpeed = ParseInt(cells[_attackSpeed]);
            int armor = ParseInt(cells[_armor]);
            float speed = ParseFloat(cells[_speed]);
            int cost = ParseInt(cells[_cost]);
            statDictionary.Add(unitName, (new UnitStats(health, attack, attackSpeed, armor, speed), cost));
        }
        return statDictionary;
    }

    private int ParseInt(string s)
    {
        int result = -1;
        if(!int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.Log("Cant parse int, wrong text");
        }
        return result;
    }
    private float ParseFloat(string s)
    {
        float result = -1;
        if (!float.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.Log("Cant parse float, wrong text");
        }
        return result;
    }


    private char GetPlatformSpecificLineEnd()
    {
        char lineendings = '\n';
        return lineendings;
    }
}

