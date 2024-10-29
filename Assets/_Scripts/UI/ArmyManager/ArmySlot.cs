using Assets.SimpleLocalization;
using Coffee.UIEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

[RequireComponent(typeof(Image))]
[RequireComponent (typeof(UIGradient))]
public class ArmySlot : SlotBase
{
    [SerializeField] private ClassType _class;
    [SerializeField] private RectTransform _classIcon;
    
    private Image _classIconImage;
    private UIGradient _uiGradient;
    public ClassType Class => _class;
    

    protected override void OnAwake()
    {
        _classIconImage = _classIcon.GetComponent<Image>();
        
        _uiGradient = GetComponent<UIGradient>();
        if (IsUnitUnlocked())
        {
            _classIcon.gameObject.AddComponent<SimpleTooltip>().infoLeft = LocalizationManager.Localize("Class." + _class + ".Description");
        }
        _classIconImage.color = IsUnitUnlocked()? Color.white : Color.gray;
         
    }
    protected override void OnStart()
    {
        var heroManager = GetComponentInParent<HeroManager>();
        heroManager.ItemStartDrag += OnItemStartDrag;
        heroManager.ItemEndDrag += OnItemEndDrag;
        _uiGradient.offset = 1f;

    }


    private void OnItemStartDrag(SlotItemBase item)
    {
        if (item != Item && item.Unit != null && item.Unit.Class == Class && IsUnitUnlocked())
        {
            DOTween.To(() =>_uiGradient.offset, x =>_uiGradient.offset = x, -1f, 0.3f);
        }
    }


    private void OnItemEndDrag(SlotItemBase item)
    {
        _uiGradient.offset = 1f;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null
            && eventData.pointerDrag.transform.TryGetComponent<SlotItemBase>(out SlotItemBase otherItem)
            && otherItem.Unit != null
            && otherItem.Unit.Class == _class
            && IsUnitUnlocked()
            && Item != otherItem)
        {
            if (Item.Unit.BlankUnit)
            {
                Item.SetDataInSlot(otherItem);
                otherItem.ClearSlot();
            }
            else if (!Item.Unit.BlankUnit) 
            {
                Item.SwapSlotsData(otherItem);
            }

        }
    }

    public void SetStandartUnit()
    {
        Item.Unit = GameLibrary.Instance.Fractions.GetStandartUnit(Class);
        Item.ItemImage.sprite = GameLibrary.Instance.Fractions.GetStandartUnit(Class).Image;
        Item.RefershToolTip();
        Item.ItemImage.enabled = true;
    }

    public void SetUnitInSlot(List<UnitDataSO> startUnits)
    {
        if (!IsUnitUnlocked())
        {
            Item.ItemImage.enabled = false;
            return;
        }
        UnitDataSO unit;
        if (startUnits.Exists(u => u.Class == _class))
        {
            unit = startUnits.Find(u => u.Class == _class);
        }
        else
        {
            unit = GameLibrary.Instance.Fractions.GetClassConfig(_class).StandartUniit;
        }
        Item.Unit = unit;
        Item.ItemImage.enabled = true;
        Item.ItemImage.sprite = unit.Image;
        Item.RefershToolTip();
    }

    public bool IsUnitUnlocked()
    {
        if (GameManager.Instance.Hero.level < 3 && _class == ClassType.Commander)
            return false;
        if (GameManager.Instance.Hero.level < 2 && _class == ClassType.Wizard)
            return false;
        return true;
    }

    
}
