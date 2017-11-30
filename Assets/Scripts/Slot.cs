using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    
    public GameObject m_itemPerfab;


    private void Start()
    {
       
    }

    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面有item了，amountPicked++
    /// 如果没有，根据itemPrefab去实例化一个item，放在下面
    /// </summary>
    /// <param name="item"></param>
	public void StoreItem(Item item, int amount = 1)
    {
        if(transform.childCount == 0)
        {
            GameObject itemObj = GameObject.Instantiate(m_itemPerfab) as GameObject;
            itemObj.transform.parent = this.transform;
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localScale = Vector3.one;
            itemObj.GetComponent<ItemUI>().SetItem(item, amount);
        }
        else
        {
            ItemUI itemObj = transform.GetChild(0).GetComponent<ItemUI>();
            itemObj.AddAmount();
        }
    }

    public int GetItemID()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().m_item.m_ID;
    }

    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        if (itemUI.m_amount == itemUI.m_item.m_capacity)
        {
            return true;
        }
        return false;
    }


    #region UGUI系统触发事件  
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            string content = transform.GetChild(0).GetComponent<ItemUI>().m_item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(content);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTip();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        ItemUI pickedItem = InventoryManager.Instance.PickedItem;
        //自身不为空
        if (transform.childCount>0)
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            //当前捡起任何物品
            if (!InventoryManager.Instance.IsPickedItem)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int amountPicked = (currentItem.m_amount + 1) / 2;
                    InventoryManager.Instance.PickupItem(currentItem.m_item, amountPicked);
                    InventoryManager.Instance.m_changeSlot = this;

                    if (currentItem.m_amount - amountPicked == 0)
                    {
                        Destroy(currentItem.gameObject);
                    }
                    else
                    {
                        currentItem.ReduceAmount(amountPicked);
                    }
                }
                else
                {
                    InventoryManager.Instance.PickupItem(currentItem.m_item, currentItem.m_amount);
                    InventoryManager.Instance.m_changeSlot = this;
                    Destroy(currentItem.gameObject);
                }
            }
            else
            {
                //捡起的物品与点击的武器id判断
                if(currentItem.m_item.m_ID == InventoryManager.Instance.PickedItem.m_item.m_ID)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if(currentItem.m_item.m_capacity > currentItem.m_amount)
                        {
                            currentItem.AddAmount();
                            InventoryManager.Instance.RemoveItemByAmount(1);
                        }
                    }
                    else
                    {
                        if (currentItem.m_item.m_capacity > currentItem.m_amount)
                        {
                            //当前物品槽剩余的空间
                            int amountRemain = currentItem.m_item.m_capacity - currentItem.m_amount;
                            
                            //可以完全放下
                            if (amountRemain > pickedItem.m_amount)
                            {
                                InventoryManager.Instance.PutDownItem();
                                currentItem.AddAmount(pickedItem.m_amount);
                            }
                            else
                            {
                                currentItem.AddAmount(amountRemain);
                                InventoryManager.Instance.RemoveItemByAmount(amountRemain);

                            }
                        }
                    }
                }
                else
                {
                    InventoryManager.Instance.ExchangeItem(currentItem);
                }
            }
        }
        else
        {
            if (InventoryManager.Instance.IsPickedItem)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    StoreItem(pickedItem.m_item);
                    InventoryManager.Instance.RemoveItemByAmount(1);

                }
                else
                {
                    StoreItem(pickedItem.m_item, InventoryManager.Instance.PickedItem.m_amount);
                    InventoryManager.Instance.PutDownItem();
                }
            }
        }
    }
    #endregion
}
