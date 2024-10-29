using UnityEngine;
using UnityEngine.Pool;

public class DamageManager : MonoBehaviour
{
    [SerializeField]
    private DamagePopup _damagePopupPrefab;
    public static IObjectPool<DamagePopup> PopupPool;

    private void Awake()
    {
        PopupPool = new ObjectPool<DamagePopup>(CreatePopup, OnGetPopup, OnReleasePopup, OnDestroyPopup, true, 10, 10);
    }
    #region PoolAPI
    private DamagePopup CreatePopup()
    {
        DamagePopup popup = Instantiate(_damagePopupPrefab, transform.position, Quaternion.identity, transform);
        popup.gameObject.SetActive(false);
        return popup;
    }
    private void OnGetPopup(DamagePopup popup)
    {
        popup.gameObject.SetActive(true);
    }
    private void OnReleasePopup(DamagePopup popup)
    {
        popup.gameObject.SetActive(false);
    }
    private void OnDestroyPopup(DamagePopup popup)
    {
        Destroy(popup.gameObject);
    }
    #endregion

    private void OnDamagableHealed(int healAmount, Transform target)
    {
        DamagePopup pop = PopupPool.Get();
        pop.HealSetup(healAmount, target);
    }

    private void OnDamagableGetDamage(int damageAmount, Transform target)
    {
        DamagePopup pop = PopupPool.Get();
        pop.DamageSetup(damageAmount, target);
    }
    private void OnEnable()
    {
        EventBus.DamagableDamaged += OnDamagableGetDamage;
        EventBus.DamagableHealed += OnDamagableHealed;
    }

    private void OnDisable()
    {
        EventBus.DamagableDamaged -= OnDamagableGetDamage;
        EventBus.DamagableHealed -= OnDamagableHealed;
        PopupPool?.Clear();
    }
}
