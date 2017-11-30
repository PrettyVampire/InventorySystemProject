using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消耗品
/// </summary>
public class Consumable : Item {
    public int m_HP { get; set; }
    public int m_MP { get; set; }

    //构造函数  base继承父类
    public Consumable(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, int hp, int mp)
        :base(id,name,type, quality, des, capacity, buyPrice, sellPrice, sprite)
    {
        this.m_HP = hp;
        this.m_MP = mp;
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string content = string.Format("{0}\n\n <color=blue>加血：{1}  加蓝：{2}</color>", text, m_HP, m_MP);

        return content;
    }

    public override string ToString()
    {
        string s = "";
        s += m_ID.ToString();
        s += m_type;
        s += m_quality;
        s += m_name;
        s += m_description;
        s += m_capacity.ToString();
        s += m_buyPrice;
        s += m_sellPrice;
        s += m_sprite;
        s += m_HP;
        s += m_MP;

        return s;

    }
}
