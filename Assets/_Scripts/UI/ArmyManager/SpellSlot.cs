using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellSlot : SlotBase
{
    
    
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null 
            && eventData.pointerDrag.transform.TryGetComponent<SlotItemBase>(out SlotItemBase otherItem)
            && otherItem.Spell != null)
        {
            Item.SwapSlotsData(otherItem);
        }
    }


}
