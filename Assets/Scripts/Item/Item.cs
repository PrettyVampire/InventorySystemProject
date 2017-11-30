using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 物品基类
/// </summary>
public class Item  {

    public int m_ID { get; set; }             //id
    public string m_name { get; set; }        //名字
    public ItemType m_type { get; set; }      //类型
    public ItemQuality m_quality { get; set; }//品质
    public string m_description { get; set; } //描述
    public int m_capacity { get; set; }       //容量
    public int m_buyPrice { get; set; }       //购买价格
    public int m_sellPrice { get; set; }      //销售价格
    public string m_sprite { get; set; }      //图标

    /// <summary>
    /// 空的构造函数，如果子类无构造函数，则调用父类这个空的构造函数
    /// </summary>
    public Item()
    {
        this.m_ID = -1;
    }

    //构造函数
    public Item(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite)
    {
        this.m_ID = id;
        this.m_name = name;
        this.m_type = type;
        this.m_quality = quality;
        this.m_description = des;
        this.m_capacity = capacity;
        this.m_buyPrice = buyPrice;
        this.m_sellPrice = sellPrice;
        this.m_sprite = sprite;
    }

    /// <summary>
    /// 物品类型
    /// </summary>
    public enum ItemType
    {
        Consumable, //消耗品
        Equipment,  //装备
        Weapon,     //武器
        Material    //材料
    }
    /// <summary>
    /// 品质
    /// </summary>
    public enum ItemQuality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Artifact
    }


    public virtual string GetToolTipText()
    {
        string color = "";
        switch (m_quality)
        {
            case ItemQuality.Common:
                color = "white";
                break;
            case ItemQuality.Uncommon:
                color = "lime";

                break;
            case ItemQuality.Rare:
                color = "navy";
                break;
            case ItemQuality.Epic:
                color = "magenta";
                break;
            case ItemQuality.Legendary:
                color = "orange";
                break;
            case ItemQuality.Artifact:
                color = "red";
                break;
            default:
                break;
        }
        string content = string.Format("<color={0}><size=13>{1}</size></color>\n" +
            "<color=green>购买价格：{2} 出售价格：{3}</color>", color, m_name, m_buyPrice, m_sellPrice);

        return content;
    }
}
