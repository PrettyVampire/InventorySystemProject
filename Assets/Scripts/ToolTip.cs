using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 显示物品信息
/// </summary>
public class ToolTip : MonoBehaviour {

    private Text m_tooTipText;
    private Text m_contentText;
    private CanvasGroup m_canvasGroup;

    public float m_targetAlpha = 0;
    public float m_smoothing = 5;

    // Use this for initialization
    void Start () {
        m_tooTipText = GetComponent<Text>();
        m_contentText = transform.Find("Content").GetComponent<Text>();
        m_canvasGroup = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
		if(m_canvasGroup.alpha != m_targetAlpha)
        {
            m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, m_targetAlpha, m_smoothing * Time.deltaTime);
            if(Mathf.Abs(m_canvasGroup.alpha - m_targetAlpha) < 0.02f)
            {
                m_canvasGroup.alpha = m_targetAlpha;
            }
        }
	}

    public void Show(string str)
    {
        m_tooTipText.text = str;
        m_contentText.text = str;

        m_targetAlpha = 1;
    }

    public void Hide()
    {
        m_targetAlpha = 0;
    }

    public void SetLocalPosition(Vector2 pos)
    {
        transform.localPosition = pos;
    }
}
