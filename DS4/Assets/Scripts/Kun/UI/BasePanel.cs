using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum E_PanelState
{
    FadeIn,
    FadeOut,
    Normal,
}
public abstract class BasePanel : MonoBehaviour
{
    //�������͸���ȵ����
    private CanvasGroup canvasGroup;
    //���뵭�����ٶ�
    private float fadeSpeed = 10f;
    //���״̬
    private E_PanelState panelState;
    //����������¼�
    private UnityAction hideMeCallback = null;
    protected virtual void Awake()
    {
        //һ��ʼ�ͻ�ȡ����Ϲ��ص�CanvasGroup���
        canvasGroup = GetComponent<CanvasGroup>();
        //���û�У������һ��
        if (canvasGroup == null) canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// ע��ؼ��¼��ķ�������������嶼��Ҫע��ؼ�
    /// ����д�ɳ��󷽷� ���������ȥʵ��
    /// </summary>
    public abstract void Init();
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        panelState = E_PanelState.FadeIn;
    }
    public virtual void HideMe(UnityAction _callback)
    {
        canvasGroup.alpha = 1;
        panelState = E_PanelState.FadeOut;
        hideMeCallback = _callback;
    }
    protected virtual void Update()
    {
        //�������״ִ̬�в�ͬ�߼�
        switch (panelState)
        {
            //����
            case E_PanelState.FadeIn:
                if (canvasGroup.alpha != 1)
                {
                    canvasGroup.alpha += fadeSpeed * Time.deltaTime;
                    if (canvasGroup.alpha >= 1)
                    {
                        panelState = E_PanelState.Normal;
                        canvasGroup.alpha = 1;
                    }
                }
                break;
            //����
            case E_PanelState.FadeOut:
                if (canvasGroup.alpha != 0)
                {
                    canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                    if (canvasGroup.alpha <= 0)
                    {
                        panelState = E_PanelState.Normal;
                        canvasGroup.alpha = 0;
                        //��嵭�������� ִ���¼�
                        hideMeCallback?.Invoke();
                    }
                }
                break;
            //ƽʱ
            case E_PanelState.Normal:
                break;
        }
    }
}
