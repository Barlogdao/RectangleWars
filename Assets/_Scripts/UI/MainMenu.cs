using UnityEngine;
using DG.Tweening;
using Assets.SimpleLocalization;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Transform label;
    [SerializeField]
    private Button _starrButton;

    private void Start()
    {
        label.DOScale(0, 1f).SetEase(Ease.OutSine).From();
        _starrButton.Select();
    }
    public void ChangeLanguage(string language)
    {
        LocalizationManager.Language = language;
    }

    
}
