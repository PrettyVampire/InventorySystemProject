using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VenderSlot : Slot {


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !InventoryManager.Instance.IsPickedItem)
        {
            if (transform.childCount > 0)
            {
                ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Vender.Instance.BuyItem(itemUI.m_item);
                InventoryManager.Instance.HideToolTip();

                itemUI.ReduceAmount(1);
                if(itemUI.m_amount <= 0)
                {
                    Destroy(itemUI.gameObject);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left && InventoryManager.Instance.IsPickedItem)
        {
            Vender.Instance.SellItem();
        }
    }
}
