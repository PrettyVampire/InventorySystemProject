using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器类
/// </summary>
public class Weapon : Item {
    public float m_damage { set; get; }
    public WeaponType m_weaponType { set; get; }

    //构造函数  base继承父类
    public Weapon(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite,
        int damage, WeaponType weaponType)
        :base(id,name,type, quality, des, capacity, buyPrice, sellPrice, sprite)
    {
        this.m_damage = damage;
        this.m_weaponType = weaponType;
    }



    /// <summary>
    /// 武器类型
    /// </summary>
    public enum WeaponType
    {
        offHand,
        mainHand
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string wpTypeText = "";
        switch (m_weaponType)
        {
            case WeaponType.offHand:
                wpTypeText = "副手";
                break;
            case WeaponType.mainHand:
                wpTypeText = "主手";
                break;
            default:
                break;
        }
        string content = string.Format("{0}\n\n<color=blue>伤害：{1}\n武器类型：{2}</color>", text, m_damage, wpTypeText);

        return content;
    }
}
