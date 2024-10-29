using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsightToolTip : MonoBehaviour
{
   
    public SpellSO Insight;
    public Hero Hero;
    public Image InsightIcon;
    SimpleTooltip tooltip;

    private void Awake()
    {
        tooltip = gameObject.AddComponent<SimpleTooltip>();
        InsightIcon = GetComponent<Image>();
        LocalizationManager.LocalizationChanged += Localize;
    }
  
    public void Init(Hero hero,SpellSO insight, Sprite sprite)
    {
        Hero = hero;
        Insight = insight;
        InsightIcon.sprite = sprite;
        Localize();
    }


    public void OnDestroy()
    {
        LocalizationManager.LocalizationChanged -= Localize;
    }

    private void Localize()
    {
        tooltip.infoLeft = Insight.Description(Hero);
    }
}
