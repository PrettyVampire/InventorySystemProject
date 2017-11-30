using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备类
/// </summary>
public class Equipment : Item {

    public int m_strength { get; set; } //力量
    public int m_intellect { get; set; }//智力
    public int m_agility { get; set; }   //敏捷
    public int m_stamina { get; set; }    //体力
    public EquipmentType m_equipType { get; set; }

    //构造函数  base继承父类
    public Equipment(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, 
        int strength, int intellect, int agility, int stamina, EquipmentType equipType)
        :base(id,name,type, quality, des, capacity, buyPrice, sellPrice, sprite)
    {
        this.m_strength = strength;
        this.m_intellect = intellect;
        this.m_agility = agility;
        this.m_stamina = stamina;
        this.m_equipType = equipType;
    }

    /// <summary>
    /// 装备类型
    /// </summary>
    public enum EquipmentType
    {
        head, //头部
        neck,//脖子
        chest,//胸部
        ring,//戒指
        leg,//腿
        bracer,//护腕
        boot,//靴子
        trinket,//饰品
        shoulder,//肩膀
        belt,//腰带
        offhand//副手
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string equipTypeText = "";
        switch (m_equipType)
        {
            case EquipmentType.head:
                equipTypeText = "头部";
                break;
            case EquipmentType.neck:
                equipTypeText = "脖子";
                break;
            case EquipmentType.chest:
                equipTypeText = "胸部";
                break;
            case EquipmentType.ring:
                equipTypeText = "戒指";
                break;
            case EquipmentType.leg:
                equipTypeText = "大腿";
                break;
            case EquipmentType.bracer:
                equipTypeText = "护腕";
                break;
            case EquipmentType.boot:
                equipTypeText = "靴子";
                break;
            case EquipmentType.trinket:
                equipTypeText = "饰品";
                break;
            case EquipmentType.shoulder:
                equipTypeText = "头部";
                break;
            case EquipmentType.belt:
                equipTypeText = "腰带";
                break;
            case EquipmentType.offhand:
                equipTypeText = "副手";
                break;
            default:
                break;
        }
        string content = string.Format("{0}\n\n<color=blue>力量：{1}\n智力：{2}\n敏捷：{3}\n体力：{4}\n设备类型：{5}</color>",
            text, m_strength, m_intellect, m_agility, m_stamina, equipTypeText);

        return content;
    }
}
