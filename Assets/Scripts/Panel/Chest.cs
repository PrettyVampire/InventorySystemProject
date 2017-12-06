using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箱子系统
/// </summary>
public class Chest : Inventory {

    private static Chest m_instance;

    public static Chest Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.Find("ChestPanel").GetComponent<Chest>();
            }
            return m_instance;
        }
    }
}
