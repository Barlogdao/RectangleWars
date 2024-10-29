using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventFloatSO", menuName = "ScriptableObjects/FloatEvent")]
public class EventFloatSO : ScriptableObject
{
    private Action<float> action;
    [field:SerializeField]
    public string PrefsTag { get; private set; }

    public void RaiseEvent(float value)
    {
        action?.Invoke(value);
    }
    public void AddListener(Action<float> callback)
    {
        action += callback;
    }
    public void RemoveListener(Action<float> callback)
    {
        action -= callback;
    }
    
}