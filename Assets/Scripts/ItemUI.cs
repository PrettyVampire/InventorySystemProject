using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemUI : MonoBehaviour {

    #region item数据
    public Item m_item { get; private set; }
	public int m_amount { get; private set; }
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

    private Vector3 m_targetScale = new Vector3(0.65f, 0.65f, 0.65f);
    private Vector3 m_animationScale = Vector3.one;
    private float m_smoothing = 5f;

    private void Start()
    {
        m_text = GetComponentInChildren<Text>();
        m_image = GetComponent<Image>();
    }

    private void Update()
    {
        if (transform.lossyScale.x != m_targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, m_targetScale, m_smoothing * Time.deltaTime);
            if (Mathf.Abs(transform.lossyScale.x - m_targetScale.x) <= 0.02f)
            {
                transform.localScale = m_targetScale;
            }
        }
    }

    public void SetItem(Item item, int amount = 1)
    {
        m_item = item;
        m_amount = amount;
        
       
        itemImage.sprite = Resources.Load<Sprite>(m_item.m_sprite);
        if(m_item.m_capacity > 1)
        {
            if (m_amount > m_item.m_capacity)
            {
                m_amount = m_item.m_capacity;
            }
            amountText.text = this.m_amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
        transform.localScale = this.m_animationScale;
    }
    public void AddAmount(int amount = 1)
    {
        this.m_amount += amount;
        if (m_item.m_capacity > 1)
        {
            if(m_amount > m_item.m_capacity)
            {
                m_amount = m_item.m_capacity;
            }
            amountText.text = this.m_amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
        transform.localScale = this.m_animationScale;

    }
    public void ReduceAmount(int amount)
    {
        this.m_amount -= amount;
        if (m_item.m_capacity > 1)
        {
            amountText.text = this.m_amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
        transform.localScale = this.m_animationScale;
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
