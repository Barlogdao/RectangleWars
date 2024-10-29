using Assets.SimpleLocalization;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroStatTooltip : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    HeroStatSO Stat;
    SimpleTooltip tooltip;
    public static event Action <HeroStatSO> HeroStatClicked;

    private void Awake()
    {
        tooltip = gameObject.AddComponent<SimpleTooltip>();
    }
    void Start()
    {
        Localize();
        LocalizationManager.LocalizationChanged += Localize;
    }


    public void OnDestroy()
    {
        LocalizationManager.LocalizationChanged -= Localize;
    }

    private void Localize()
    {
        tooltip.infoLeft = Stat.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HeroStatClicked?.Invoke(Stat);
    }
}
