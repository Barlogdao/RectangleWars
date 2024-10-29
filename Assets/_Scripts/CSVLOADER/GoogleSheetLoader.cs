using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CSVLoader))]
[RequireComponent(typeof(SheetProcessor))]
public class GoogleSheetLoader : MonoBehaviour
{
    public static event Action<Dictionary<string, (UnitStats, int)>> OnProcessData;
    [SerializeField] private string _sheetID;
    [SerializeField] private Dictionary<string, (UnitStats, int)> _unitStats;
    private CSVLoader _csvLoader;
    private SheetProcessor _sheetProcessor;

    private void Start()
    {
        _sheetProcessor = GetComponent<SheetProcessor>();
        _csvLoader = GetComponent<CSVLoader>();
        DownloadTable();
    }

    private void DownloadTable()
    {
        _csvLoader.DownloadTable(_sheetID, OnRawCSVLoaded);
    }

    private void OnRawCSVLoaded(string rawCSVText)
    {
        _unitStats = _sheetProcessor.ProcessData(rawCSVText);
        OnProcessData?.Invoke(_unitStats);
    }
}
