using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBase : MonoBehaviour, IDropHandler
{
    [HideInInspector]
    public SlotItemBase Item;
    public virtual void OnDrop(PointerEventData eventData)
    {
        //Когда в инвентарь влетает итем из панели
        if (eventData.pointerDrag != null
            && eventData.pointerDrag.transform.TryGetComponent<SlotItemBase>(out SlotItemBase otherItem))
        {
            if (otherItem.Unit != null && !otherItem.Unit.BlankUnit && otherItem.gameObject.GetComponentInParent<ArmySlot>() != null) 
            {
                var armyslot = otherItem.gameObject.GetComponentInParent<ArmySlot>();
                Item.SetDataInSlot(armyslot.Item);
                armyslot.SetStandartUnit();
            }
            else if (otherItem.Spell != null)
            {
                Item.SwapSlotsData(otherItem);
            }

        }
    }

    protected void Awake()
    {
        Item = GetComponentInChildren<SlotItemBase>();
        OnAwake();
    }

    protected virtual void OnAwake() { }

    private void Start()
    {
        Item.RefershToolTip();
        OnStart();
    }

    protected virtual void OnStart() { }

}
