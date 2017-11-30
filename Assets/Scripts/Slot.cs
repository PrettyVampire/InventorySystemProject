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
	public void StoreItem(Item item)
    {
        if(transform.childCount == 0)
        {
            GameObject itemObj = GameObject.Instantiate(m_itemPerfab) as GameObject;
            itemObj.transform.parent = this.transform;
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localScale = Vector3.one;
            itemObj.GetComponent<ItemUI>().SetItem(item, 1);
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
            Debug.Log("OnPointerEnter");
            string content = transform.GetChild(0).GetComponent<ItemUI>().m_item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(content);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            Debug.Log("OnPointerExit");

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
            //当前没有选择任何物品
            if (!InventoryManager.Instance.IsPickedItem)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int amountPicked = (currentItem.m_amount + 1) / 2;
                    InventoryManager.Instance.PickupItem(currentItem.m_item, amountPicked);

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
                    Destroy(currentItem.gameObject);
                }
            }
            else
            {
                Debug.Log("OnPointerDown");

                currentItem.SetItem(pickedItem.m_item, pickedItem.m_amount);
            }
        }
        else
        {

        }
    }
    #endregion
}
