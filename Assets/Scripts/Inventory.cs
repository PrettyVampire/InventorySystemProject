using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

    private Slot[] m_slotArr;
    private List<Slot> m_slotList;
    // Use this for initialization
    virtual public void Start () {
        m_slotArr = GetComponentsInChildren<Slot>();
	}
	
    /// 先判断要存储的这个物品是否存在
	public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);

        return StoreItem(item);
    }
    
    // 卡槽中存入物品
    public bool StoreItem(Item item)
    {
        if(item == null)
        {
            Debug.LogWarning("要存储的物品id不存在");
            return false;
        }
        
        if(item.m_capacity == 1)
        {
            Slot slot = FindEmptySlot();
            if(slot == null)
            {
                Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                slot.StoreItem(item);
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(item);
            if(slot != null)
            {
                slot.StoreItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if(emptySlot == null)
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
                else
                {
                    emptySlot.StoreItem(item);
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 找到一个空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmptySlot()
    {
        foreach(Slot slot in m_slotArr) 
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }


    /// <summary>
    /// 查找所有物品槽，返回有相同类型的item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private Slot FindSameIdSlot(Item item)
    {
        foreach(Slot slot in m_slotArr)
        {
            if(slot.transform.childCount>=1 && slot.GetItemID()==item.m_ID && slot.IsFilled()==false)
            {
                return slot;
            }
        }
        return null;
    }
}
