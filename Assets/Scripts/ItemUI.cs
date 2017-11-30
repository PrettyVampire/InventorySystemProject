using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

#region 数据
    public Item m_item { get; set; }
	public int m_amount { get; set; }
#endregion


    #region 防止未初始化，就直接调用导致异常 ui组件
    private Text m_text;
    private Image m_image;

    private Text amountText
    {
        get
        {
            if(m_text == null)
            {
                m_text = GetComponentInChildren<Text>();
            }
            return m_text;
        }
    }
    private Image itemImage
    {
        get
        {
            if(m_image==null)
            {
                m_image = GetComponent<Image>();
            }
            return m_image;
        }
    }
    #endregion


    private void Start()
    {
        m_text = GetComponentInChildren<Text>();
        m_image = GetComponent<Image>();
    }

    public void SetItem(Item item, int amount = 1)
    {
        m_item = item;
        m_amount = amount;
        
       
        itemImage.sprite = Resources.Load<Sprite>(m_item.m_sprite);
        if(m_item.m_capacity > 1)
        {
            amountText.text = this.m_amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }
    

    public void AddAmount(int amount = 1)
    {
        this.m_amount += amount;
        if (m_item.m_capacity > 1)
        {
            amountText.text = this.m_amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }
    public void ReduceAmount(int amount)
    {
        this.m_amount -= amount;
        amountText.text = this.m_amount.ToString();

    }

    public void Show()
    {
        gameObject.active = true;
    }

    public void Hide()
    {
        gameObject.active = false;
    }

    public void SetLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }
}
