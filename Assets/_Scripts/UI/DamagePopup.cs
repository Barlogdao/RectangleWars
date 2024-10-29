using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    // Component to display Damage
    [SerializeField]
    private TextMeshPro _damageTextWindow;
    [SerializeField]
    float liveTime = 0.5f;
    [SerializeField]
    Color damageColor, healColor;
    // How far popup will move vertically
    [SerializeField]
    float _distanse;
    // Startscale of object, which twin to origin scale
    [Min(1f)]
    [SerializeField]
    float _startSize;
    

    public void DamageSetup(int damageAmount, Transform place)
    {
        _damageTextWindow.color = damageColor;
        GeneralSetup(damageAmount, place);
    }

    public void HealSetup(int healAmount, Transform place)
    {
        _damageTextWindow.color = healColor;
        GeneralSetup(healAmount, place);
    }
    private void GeneralSetup(int amount, Transform place)
    {
        _damageTextWindow.SetText(amount.ToString());
        transform.position = new Vector2(place.position.x + Random.Range(-0.3f, 0.3f), place.position.y + Random.Range(0.7f, 0.8f));
        transform.DOScale(_startSize, liveTime).From().SetEase(Ease.OutSine);
        transform.DOMoveY(_distanse, liveTime).SetRelative().OnComplete(() => { DamageManager.PopupPool.Release(this); });
    }
}
