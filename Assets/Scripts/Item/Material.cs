using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 材质类
/// </summary>
public class Material : Item {

    //构造函数  base继承父类
    public Material(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite)
        :base(id,name,type, quality, des, capacity, buyPrice, sellPrice, sprite)
    {

    }
    
}
