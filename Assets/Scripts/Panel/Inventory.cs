using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 仓库基类，提供物品槽的查找以及，物品的存储功能
/// </summary>
public class Inventory : MonoBehaviour {

    protected Slot[] m_slotArr;//存储背包下所有的slot
    private List<Slot> m_slotList;

    private float m_smoothing = 5f;
    private float m_targetAlpha = 1;
    private CanvasGroup m_canvasGroup;

    virtual public void Awake()
    {
        m_slotArr = GetComponentsInChildren<Slot>();//检索所有孩子节点包括孙子
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    // Use this for initialization
    virtual public void Start () {
       
        
    }

    private void Update()
    {
        m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, m_targetAlpha, m_smoothing * Time.deltaTime);
        if(Mathf.Abs(m_canvasGroup.alpha - m_targetAlpha) <= 0.02f)
        {
            m_canvasGroup.alpha = m_targetAlpha;
        }
    }

    /// 先判断要存储的这个物品是否存在
    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);

        return StoreItem(item);
    }
    
    // 卡槽中存入物品
    virtual public bool StoreItem(Item item)
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
    protected Slot FindEmptySlot()
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
    protected Slot FindSameIdSlot(Item item)
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

    public void Show()
    {
        m_targetAlpha = 1;
        m_canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        m_targetAlpha = 0;
        m_canvasGroup.blocksRaycasts = false;
    }

    public void SwitchAlpha()
    {
        if(m_targetAlpha==1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    #region Save and Load
    //"-"区分不同的物品  “0”代表卡槽为空
    public void SaveInventory()
    {
        StringBuilder info = new StringBuilder();
        foreach (Slot slot in m_slotArr)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                info.Append(itemUI.m_item.m_ID + "," + itemUI.m_amount + "-");
            }
            else
            {
                info.Append("0-");
            }
        }
        Debug.Log("info = " + info.ToString());

        PlayerPrefs.SetString(this.gameObject.name, info.ToString());
    }

    public void LoadInventory()
    {
        if(PlayerPrefs.HasKey(gameObject.name))
        {
            string info = PlayerPrefs.GetString(gameObject.name);
            Debug.Log("info = " + info);
                string[] itemInfoArr = info.Split('-');//字符串切割  得到所有物品存放信息
                for(int i=0; i< itemInfoArr.Length - 1; i++)//字符串切割时   "-"后面会多一个空字符串,so Length - 1
                {
                    if(itemInfoArr[i] != "0")
                    {
                        string[] detailInfoArr = itemInfoArr[i].Split(',');
                        int id = int.Parse(detailInfoArr[0]);
                        int amount = int.Parse(detailInfoArr[1]);
                        Item item = InventoryManager.Instance.GetItemById(id);
                        m_slotArr[i].StoreItem(item, amount);
                    }
                }
        }
    }
    #endregion
}
