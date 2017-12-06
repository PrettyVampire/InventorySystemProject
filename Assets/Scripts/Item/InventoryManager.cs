using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 物品管理系统
/// </summary>
public class InventoryManager : MonoBehaviour
{

    #region 单例模式
    private static InventoryManager m_instance;

    /// <summary>
    /// 静态方法中只能调用静态字段，Find为静态方法,虽然比较好性能，但只查找一次无妨
    /// </summary>
    public static InventoryManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();   //
            }
            return m_instance;
        }
    }
    #endregion

    
    #region ToolTip
    private ToolTip m_toolTip;
    private bool m_isToolTipShow = false;
    #endregion


    #region PickedItem
    //拖拽时显示的物品
    private ItemUI m_pickedItem = null;
    public ItemUI PickedItem
    {
        get
        {
            return m_pickedItem;
        }
        set
        {
        }
    }

    //鼠标上是否有物品
    private bool m_isPickedItem = false;
    public bool IsPickedItem
    {
        get
        {
            return m_isPickedItem;
        }
        set
        {
            m_isPickedItem = value;
        }
    }

    //
    public Slot m_changeSlot = null;
    #endregion


    private Canvas m_canvas;
    private List<Item> m_itemList;


    private void Awake()
    {
        ParseItemJson();
    }

    private void Start()
    {
        m_toolTip = GameObject.FindObjectOfType<ToolTip>();
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        m_pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        GameObject.Find("PickedItem").active = false;
        
    }

    private void Update()
    {
        //鼠标捡起的物品跟随移动
        if (m_isPickedItem)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform, Input.mousePosition, null, out position);
            m_pickedItem.SetLocalPosition(position);
        }
        //控制面板跟随鼠标移动
        else if (m_isToolTipShow)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform, Input.mousePosition, null, out position);
            m_toolTip.SetLocalPosition(position + new Vector2(10, -10));
        }

        //丢弃物品   手上有物品、按下左键、鼠标下没有任何物品
        //UnityEngine.EventSystems.EventSystem 事件系统
        if (m_isPickedItem && Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject(-1))
        {
            this.ThrowPickedItem();
        }
    }


    /// <summary>
    /// 解析物品信息
    /// </summary>
    void ParseItemJson()
    {
        m_itemList = new List<Item>();

        //加载文本  TextAsset  
        TextAsset itemText = Resources.Load<TextAsset>("Items");
        string itemsJson = itemText.text;
        JsonData jsonObject =  JsonMapper.ToObject(itemsJson);
         
        for(int i=0; i<jsonObject.Count; i++)
        {
            int id = (int)jsonObject[i]["id"];
            string name = (string)jsonObject[i]["name"];
            string description = (string)jsonObject[i]["description"];
            int buyprice = (int)jsonObject[i]["buyprice"];
            int capacity = (int)jsonObject[i]["capacity"];
            int sellprice = (int)jsonObject[i]["sellprice"];
            string sprite = (string)jsonObject[i]["sprite"];


            string typeStr = (string)jsonObject[i]["type"];
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), typeStr);

            string qualityStr = (string)jsonObject[i]["quality"];
            Item.ItemQuality quality = (Item.ItemQuality)System.Enum.Parse(typeof(Item.ItemQuality), qualityStr);

            Item item = null;
            switch (type)
            {
                case Item.ItemType.Consumable:
                    int hp = (int)jsonObject[i]["hp"];
                    int mp = (int)jsonObject[i]["mp"];
                    item = new Consumable(id, name, type, quality, description, capacity, buyprice, sellprice, sprite, hp, mp);

                    break;
                case Item.ItemType.Equipment:
                    int strength = (int)jsonObject[i]["strength"];
                    int intellect = (int)jsonObject[i]["intellect"];
                    int agility = (int)jsonObject[i]["agility"];
                    int stamina = (int)jsonObject[i]["stamina"];
                    string equipTypeStr = (string)jsonObject[i]["equipType"];
                    Equipment.EquipmentType equipType = (Equipment.EquipmentType)System.Enum.Parse(typeof(Equipment.EquipmentType), equipTypeStr);

                    item = new Equipment(id, name, type, quality, description, capacity, buyprice, sellprice, sprite, 
                        strength, intellect, agility, stamina, equipType);
                    break;
                case Item.ItemType.Material:
                    item = new Material(id, name, type, quality, description, capacity, buyprice, sellprice, sprite);
                    break;
                case Item.ItemType.Weapon:
                    int damage = (int)jsonObject[i]["damage"];
                    string weaponTypeStr = (string)jsonObject[i]["weaponType"];
                    Weapon.WeaponType weaponType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), weaponTypeStr);

                    item = new Weapon(id, name, type, quality, description, capacity, buyprice, sellprice, sprite,
                        damage, weaponType);

                    break;

            }

            m_itemList.Add(item); Debug.Log(item);
        }
    }

    public Item GetItemById(int id)
    {
        foreach(Item item in m_itemList)
        {
            if(item.m_ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTip(string content)
    {
        if(!m_isPickedItem)
        {
            m_toolTip.Show(content);
            m_isToolTipShow = true;
        }
    }

    public void HideToolTip()
    {
        m_isToolTipShow = false;
        m_toolTip.Hide();
    }
    

    public void PickupItem(Item item, int amount)
    {
        m_pickedItem.SetItem(item, amount);
        m_isPickedItem = true;
        m_pickedItem.Show();

        m_toolTip.Hide();
    }

    public void RemoveItemByAmount(int amount)
    {
        PickedItem.ReduceAmount(amount);
        if(PickedItem.m_amount <= 0)
        {
            PickedItem.Hide();
            m_isPickedItem = false;
        }
    }

    public void PutDownItem()
    {
        m_isPickedItem = false;
        m_pickedItem.Hide();

    }

    public void ExchangeItem(ItemUI targetItemUI)
    {
        m_changeSlot.StoreItem(targetItemUI.m_item, targetItemUI.m_amount);
        targetItemUI.SetItem(m_pickedItem.m_item, m_pickedItem.m_amount);

        m_isPickedItem = false;
        m_pickedItem.Hide();
    }

    //扔掉物品
    public void ThrowPickedItem()
    {
        m_pickedItem.Hide();
        m_isPickedItem = false;
    }

    public void SaveInventory()
    {
        Character.Instance.SaveInventory();
        Chest.Instance.SaveInventory();
        Forget.Instance.SaveInventory();
        Knapsack.Instance.SaveInventory();
        Vender.Instance.SaveInventory();

        PlayerPrefs.SetFloat("coinAmount", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount);
    }

    public void LoadInventory()
    {
        Character.Instance.LoadInventory();
        Chest.Instance.LoadInventory();
        Forget.Instance.LoadInventory();
        Knapsack.Instance.LoadInventory();
        Vender.Instance.LoadInventory();

        if(PlayerPrefs.HasKey("coinAmount"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount = PlayerPrefs.GetFloat("coinAmount");
        }
    }
    
}

