using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 主角
/// </summary>
public class Character : Inventory {

    private Player m_player;
    public Text m_infoText;

    private static Character m_instance;
    public static Character Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.Find("CharacterPanel").GetComponent<Character>();
            }
            return m_instance;
        }
    }

    public override void Awake()
    {
        base.Awake();
        m_player = GameObject.Find("Player").GetComponent<Player>();
    }

    public override void Start()
    {
        base.Start();
        Hide();
    }

    //穿上装备
    public void PutOn(Item item)
    {
        Item exitItem = null;
        foreach(Slot slot in m_slotArr)
        {
            CharacterSlot characterSlot = (CharacterSlot)slot;
            if(characterSlot.IsRightItem(item))
            {
                //物品槽有item  替换或者增加数量
                if (characterSlot.transform.childCount > 0)
                {
                    ItemUI currentItemUI = characterSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    exitItem = currentItemUI.m_item;
                    currentItemUI.SetItem(item, 1);

                    Knapsack.Instance.StoreItem(exitItem);
                    
                }
                //物品槽为空  存储
                else
                {
                    characterSlot.StoreItem(item);
                }
            }
        }
        UpdateProperty();
    }

    //脱掉装备
    public void PutOff(ItemUI itemUI)
    {
        Knapsack.Instance.StoreItem(itemUI.m_item);

        InventoryManager.Instance.HideToolTip();
        DestroyImmediate(itemUI.gameObject);

        UpdateProperty();
    }

    //更新玩家基本信息
    public void UpdateProperty()
    {
        float strength = 0, intellect = 0, agility = 0, stamina = 0, damage = 0;
        Item item = null;
        Debug.Log("m_slotArr = " + m_slotArr);

        foreach (CharacterSlot slot in m_slotArr)
        {
            if(slot.transform.childCount > 0)
            {
                item = slot.transform.GetChild(0).GetComponent<ItemUI>().m_item;
                if(item is Equipment)
                {
                    Equipment equipItem = (Equipment)item;
                    strength += equipItem.m_strength;
                    intellect += equipItem.m_intellect;
                    agility += equipItem.m_agility;
                    stamina += equipItem.m_stamina;
                }
                else if(item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    damage += weapon.m_damage;
                }
            }
        }

        strength += m_player.BaseStrength;
        intellect += m_player.BaseIntellect;
        agility += m_player.BaseAgility;
        stamina += m_player.BaseStamina;
        damage += m_player.BaseDamage;
        m_infoText.text = string.Format("力量：{0}\n智力：{1}\n敏捷：{2}\n体力：{3}\n伤害：{4}", strength, intellect, agility, stamina, damage);
    }
}
