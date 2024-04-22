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
    //控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float fadeSpeed = 10f;
    //面板状态
    private E_PanelState panelState;
    //隐藏面板后的事件
    private UnityAction hideMeCallback = null;
    protected virtual void Awake()
    {
        //一开始就获取面板上挂载的CanvasGroup组件
        canvasGroup = GetComponent<CanvasGroup>();
        //如果没有，则添加一个
        if (canvasGroup == null) canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 注册控件事件的方法，所有子面板都需要注册控件
    /// 所以写成抽象方法 让子类必须去实现
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
        //根据面板状态执行不同逻辑
        switch (panelState)
        {
            //淡入
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
            //淡出
            case E_PanelState.FadeOut:
                if (canvasGroup.alpha != 0)
                {
                    canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                    if (canvasGroup.alpha <= 0)
                    {
                        panelState = E_PanelState.Normal;
                        canvasGroup.alpha = 0;
                        //面板淡出结束后 执行事件
                        hideMeCallback?.Invoke();
                    }
                }
                break;
            //平时
            case E_PanelState.Normal:
                break;
        }
    }
}
