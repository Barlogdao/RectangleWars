using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CSVLoader : MonoBehaviour
{
    private const string UrlPattern = "https://docs.google.com/spreadsheets/d/*/export?format=csv";

    public void DownloadTable(string sheetID, Action <string> onSheetLoadedAction)
    {
        string actualURL = UrlPattern.Replace("*", sheetID);
        StartCoroutine (DownloadCSVTable(actualURL,onSheetLoadedAction));
    }
    private IEnumerator DownloadCSVTable(string actualUrl, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(actualUrl))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
        yield return null;
    }
}
