using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


/// <summary>
/// 锻造系统
/// </summary>
public class Forget : Inventory {

    private List<Formula> m_formulaList;

    private static Forget m_instance;
    public static Forget Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.Find("ForgetPanel").GetComponent<Forget>();
            }
            return m_instance;
        }
    }

    private void Start()
    {
        ParseFormulaJson();
    }

    /// <summary>
    /// 解析锻造秘方信息
    /// </summary>
    void ParseFormulaJson()
    {
        Debug.Log("解析锻造秘方信息");
        m_formulaList = new List<Formula>();

        //加载文本  TextAsset  
        TextAsset formulaText = Resources.Load<TextAsset>("Formula");
        string formulaJson = formulaText.text;
        JsonData jsonObject = JsonMapper.ToObject(formulaJson);

        for (int i = 0; i < jsonObject.Count; i++)
        {
            int item1ID = (int)jsonObject[i]["item1ID"];
            int item1Amount = (int)jsonObject[i]["item1Amount"];
            int item2ID = (int)jsonObject[i]["item2ID"];
            int item2Amount = (int)jsonObject[i]["item2Amount"];
            int resultID = (int)jsonObject[i]["resultID"];

            Formula formula = new Formula(item1ID, item1Amount, item2ID, item2Amount, resultID);
            m_formulaList.Add(formula);
        }
    }

    /// <summary>
    /// 开始锻造材料   根据id按需匹配，便于日后锻造卡槽的拓展
    /// 
    /// 1.获取所有材质  2.材质匹配秘籍
    /// </summary>
    public void ForgetMaterial()
    {
        List<int> itemidList = new List<int>();
        foreach (Slot slot in m_slotArr)
        {
            if(slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < itemUI.m_amount; i++)
                {
                    itemidList.Add(itemUI.m_item.m_ID);
                }
            }
        }

        Formula tempFormula = null;
        foreach (Formula formula in m_formulaList)
        {
            if (formula.IsMatchID(itemidList))
            {
                tempFormula = formula;
                break;
            }
        }

        //锻造好的物品存在背包中
        if(tempFormula != null)
        {
            Knapsack.Instance.StoreItem(tempFormula.m_resultID);
            DealForgetSlot(tempFormula);
        }
    }

    //锻造成功，移除卡槽内对应数量的物品
    void DealForgetSlot(Formula formule)
    {
        foreach(int id in formule.NeedList)
        {
            foreach (Slot slot in m_slotArr)
            {
                if (slot.transform.childCount > 0)
                {
                    ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                    if(id == itemUI.m_item.m_ID)
                    {
                        itemUI.ReduceAmount(1);
                        if(itemUI.m_amount <= 0)
                        {
                            DestroyImmediate(itemUI.gameObject);
                        }
                        break;
                    }
                }
            }
        }
    }
}
