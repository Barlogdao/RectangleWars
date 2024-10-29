using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Redcode.Extensions;
using TMPro;
using Assets.SimpleLocalization;

public class UnitFullInfoTip : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    RectTransform perksInfo;
    [SerializeField]
    UnitStatBar health, attack, armor, attackSpeed;
    [SerializeField]
    PerkTooltip perkTooltipPrefab;
    [SerializeField]
    Image unitImage;
    [SerializeField] private Image _classIcon;
    [SerializeField]
    TextMeshProUGUI unitName, unitClass;


    public static Action<UnitDataSO> ShowInfo;
    private void Awake()
    {
        ShowInfo += SetUnit;
        unitClass.gameObject.AddComponent<SimpleTooltip>();
        gameObject.SetActive(false);
    }

    private void SetUnit(UnitDataSO unit)
    {
        gameObject.SetActive(true);
        unitName.text = unit.Name;
        unitClass.text = unit.GetClassText;
        unitClass.gameObject.GetComponent<SimpleTooltip>().infoLeft = LocalizationManager.Localize("Class." + unit.Class + ".Description");
        health.SetStat(unit.Health.ToString());
        attack.SetStat(unit.Attack.ToString());
        armor.SetStat(unit.Armor.ToString());
        attackSpeed.SetStat(unit.AttackSpeed.ToString());
        unitImage.sprite = unit.Image;
        _classIcon.sprite = GameLibrary.Instance.Fractions.GetClassConfig(unit.Class).ClassIcon;
        foreach( var perk in unit.PerkList)
        {
            var prefab = Instantiate(perkTooltipPrefab, perksInfo);
            prefab.SetPerk(perk);
        }

        
    }
    private void OnDisable()
    {
        perksInfo.DestroyChilds();
    }

    private void OnDestroy()
    {
        ShowInfo -= SetUnit;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            gameObject.SetActive(false);
    }
}
