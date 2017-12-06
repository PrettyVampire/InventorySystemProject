using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 商店
/// </summary>
public class Vender : Inventory {

    private static Vender m_instance;
    public static Vender Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.Find("VenderPanel").GetComponent<Vender>();
            }
            return m_instance;
        }
    }

    public int[] m_itemIDArr;
    private Player m_player;

    public override void Awake()
    {
        base.Awake();
        m_player = GameObject.Find("Player").GetComponent<Player>();
    }

    public override void Start()
    {
        base.Start();
        foreach(int i in m_itemIDArr)
        {
            StoreItem(i);
        }

        Hide();
    }

    public void BuyItem(Item item)
    {
        if(m_player.ConsumeCoin(item.m_buyPrice))
        {
            Knapsack.Instance.StoreItem(item.m_ID);
        }
    }

    public void SellItem()
    {
        Item item = InventoryManager.Instance.PickedItem.m_item;
        m_player.EarnCoin(item.m_sellPrice);
        InventoryManager.Instance.ThrowPickedItem();
    }
}
