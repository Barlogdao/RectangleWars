using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerkTooltip : MonoBehaviour
{
    UnitPerksSO perk;
    SimpleTooltip tooltip;
    TextMeshProUGUI perkName;
    private void Awake()
    {
        perkName = GetComponent<TextMeshProUGUI>();
        tooltip = gameObject.AddComponent<SimpleTooltip>();
    }
    public void SetPerk(UnitPerksSO perk)
    {
        this.perk = perk;
        perkName.text = perk.Name;
        tooltip.infoLeft = perk.Description;
    }

}
