//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class UIManager : BaseSingleton<UIManager>
//{
//    //场景中的Canvas对象脚本，用于设置为面板的父对象
//    private Transform canvasTrans;
//    public EventSystem eventSystem;
//    //用于存储存在着的面板的字典 每一次显示一个面板都会录入该字典
//    //需要隐藏面板时 直接获取字典中的对应面板进行隐藏
//    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
//    private UIManager()
//    {
//        //动态创建Canvas
//        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("Kun/UI/Canvas"));
//        eventSystem = canvas.GetComponentInChildren<EventSystem>();
//        //得到Canvas的Transform
//        canvasTrans = canvas.transform;
//        //过场景不移除Canvas 保证Canvas的唯一性
//        GameObject.DontDestroyOnLoad(canvas);
//    }

//    /// <summary>
//    /// 显示面板方法
//    /// </summary>
//    /// <typeparam name="T">面板类型</typeparam>
//    /// <returns></returns>
//    public T ShowPanel<T>() where T : BasePanel
//    {
//        //规则：泛型T的类型名字要与预设体名字保持一致
//        string panelName = typeof(T).Name;

//        //判断字典中是否存在，若已存在则直接返回
//        if (panelDic.ContainsKey(panelName)) return panelDic[panelName] as T;

//        //根据面板名字 动态创建预设体 设置父对象
//        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Kun/UI/" + panelName));
//        //设置父对象
//        panelObj.transform.SetParent(canvasTrans, false);

//        //得到面板组件
//        T panelScript = panelObj.GetComponent<T>();
//        //将面板存入字典
//        panelDic.Add(panelName, panelScript);
//        //显示面板
//        panelScript.ShowMe();

//        return panelScript;
//    }

//    /// <summary>
//    /// 隐藏面板方法
//    /// </summary>
//    /// <typeparam name="T">面板类型</typeparam>
//    /// <param name="_needFade">是否需要淡出效果，默认否</param>
//    public void HidePanel<T>(bool _needFade = false) where T : BasePanel
//    {
//        //规则：泛型T的类型名字要与预设体名字保持一致
//        string panelName = typeof(T).Name;
//        //判断字典中有无想要隐藏的面板
//        if (panelDic.ContainsKey(panelName))
//        {
//            //判断是否需要淡出效果
//            if (_needFade)
//            {
//                //淡出面板
//                panelDic[panelName].HideMe(() =>
//                {
//                    GameObject.Destroy(panelDic[panelName].gameObject);
//                    //删除字典中的该面板
//                    panelDic.Remove(panelName);
//                });
//            }
//            else if (!_needFade)
//            {
//                //直接删除面板
//                GameObject.Destroy(panelDic[panelName].gameObject);
//                //删除字典中的该面板
//                panelDic.Remove(panelName);
//            }
//        }
//    }

//    public T GetPanel<T>() where T : BasePanel
//    {
//        //规则：泛型T的类型名字要与预设体名字保持一致
//        string panelName = typeof(T).Name;
//        //判断字典中有无想要得到的面板，若有则返回，若无则返回空
//        if (panelDic.ContainsKey(panelName)) return panelDic[panelName] as T;
//        return null;
//    }
//}
