using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 锻造秘籍类  继承MonoBehaviour的类不能new
/// </summary>
public class Formula {

    public int m_item1ID { get; private set; }
    public int m_item2ID { get; private set; }
    public int m_item1Amount { get; private set; }
    public int m_item2Amount { get; private set; }

    public int m_resultID { get; private set; }

    private List<int> m_needList = new List<int>();//所需要物品的id
    public List<int> NeedList
    {
        get
        {
            return m_needList;
        }
    }

    public Formula(int item1ID, int item1Amount, int item2ID, int item2Amount, int resultID)
    {
        m_item1ID = item1ID;
        m_item1Amount = item1Amount;
        m_item2Amount = item2Amount;
        m_item2ID = item2ID;
        m_resultID = resultID;

        for(int i=0; i<m_item1Amount; i++)
        {
            m_needList.Add(m_item1ID);
        }
        for (int i = 0; i < m_item2Amount; i++)
        {
            m_needList.Add(m_item2ID);
        }
    }


    /// <summary>
    /// 与秘籍id数组匹配
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public bool IsMatchID(List<int> idList)
    {
        List<int> tempList = new List<int>(idList);
        foreach(int id in m_needList)
        {
            bool isSuccess = tempList.Remove(id);
            
            if(!isSuccess)
            {
                return false;
            }
        }

        return true;
    }
}
