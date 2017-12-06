using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    #region 基本属性
    private int m_baseStrength = 10;
    private int m_baseIntellect = 10;
    private int m_baseAgility = 10;
    private int m_baseStamina = 10;
    private int m_baseDamage = 10;

    public int BaseStrength
    {
        get
        {
            return m_baseStrength;
        }
    }
    public int BaseIntellect
    {
        get
        {
            return m_baseIntellect;
        }
    }
    public int BaseAgility
    {
        get
        {
            return m_baseAgility;
        }
    }
    public int BaseStamina
    {
        get
        {
            return m_baseStamina;
        }
    }
    public int BaseDamage
    {
        get
        {
            return m_baseDamage;
        }
    }
    #endregion

    public Text m_coinText;
    private float m_coinAmount = 2000;
    public float CoinAmount
    {
        get
        {
            return m_coinAmount;
        }
        set
        {
            m_coinAmount = value;
            m_coinText.text = m_coinAmount.ToString();
        }
    }

    private void Awake()
    {    }

    private void Start()
    {
        Character.Instance.UpdateProperty();

        m_coinText.text = m_coinAmount.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            int id = Random.Range(1, 20);
            Knapsack.Instance.StoreItem(id);
        }
       

        if (Input.GetKeyDown(KeyCode.X))
        {
            Chest.Instance.SwitchAlpha();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Character.Instance.SwitchAlpha();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vender.Instance.SwitchAlpha();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Forget.Instance.SwitchAlpha();
        }
    }

    public bool ConsumeCoin(float amount)
    {
        if(this.m_coinAmount > amount)
        {
            m_coinAmount -= amount;
            m_coinText.text = m_coinAmount.ToString();

            return true;
        }
        return false;
    }

    public void EarnCoin(float amount)
    {
        m_coinAmount += amount;
        m_coinText.text = m_coinAmount.ToString();
    }
}
