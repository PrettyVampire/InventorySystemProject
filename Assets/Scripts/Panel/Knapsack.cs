using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knapsack : Inventory {

    private static Knapsack m_instance;

    public static Knapsack Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = GameObject.Find("KnapsackPanel").GetComponent<Knapsack>();
            }
            return m_instance;
        }
    }
}
