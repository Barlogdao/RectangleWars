using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsightDescriptionTooltip : MonoBehaviour
{
    SimpleTooltip tooltip;
    private const string INSIGHT_DESCRIPTION = "Hero.InsightDescription";

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
        tooltip.infoLeft = LocalizationManager.Localize(INSIGHT_DESCRIPTION);
    }
}
