using UnityEngine;

/// <summary>
/// Use OnAwake, OnDisableHandler and OnDestroyHandler if you want add logic in Awake,OnDisable and OnDestroy
/// </summary>
/// <typeparam name="T"></typeparam>

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get => instance; }
    private static T instance = null;
    private bool _instanceHasSpawned = false;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = (T)this;
        _instanceHasSpawned = true;
        Instance.OnAwake();
    }
    private void OnDisable()
    {
        if (!_instanceHasSpawned) return;
        OnDisableHandler();
    }
    private void OnDestroy()
    {
        if (!_instanceHasSpawned) return;
        OnDestroyHandler();
    }

    protected virtual void OnDisableHandler() { }
    protected virtual void OnDestroyHandler() { }
    protected virtual void OnAwake() { }
}

/// <summary>
/// Use OnAwake, OnDisableHandler and OnDestroyHandler if you want add logic in Awake,OnDisable and OnDestroy
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonPersistent<T> : MonoBehaviour where T : SingletonPersistent<T>
{
    public static T Instance { get => instance; }
    private static T instance = null;
    private bool _instanceHasSpawned = false;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = (T)this;
        _instanceHasSpawned = true;
        DontDestroyOnLoad(gameObject);
        Instance.OnAwake();
    }
    private void OnDisable()
    {
        if (!_instanceHasSpawned) return;
        OnDisableHandler();
    }
    private void OnDestroy()
    {
        if (!_instanceHasSpawned) return;
        OnDestroyHandler();
    }

    protected virtual void OnDisableHandler() { }
    protected virtual void OnDestroyHandler() { }
    protected virtual void OnAwake() { }
}

