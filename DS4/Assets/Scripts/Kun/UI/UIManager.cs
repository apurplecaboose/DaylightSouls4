//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class UIManager : BaseSingleton<UIManager>
//{
//    //�����е�Canvas����ű�����������Ϊ���ĸ�����
//    private Transform canvasTrans;
//    public EventSystem eventSystem;
//    //���ڴ洢�����ŵ������ֵ� ÿһ����ʾһ����嶼��¼����ֵ�
//    //��Ҫ�������ʱ ֱ�ӻ�ȡ�ֵ��еĶ�Ӧ����������
//    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
//    private UIManager()
//    {
//        //��̬����Canvas
//        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("Kun/UI/Canvas"));
//        eventSystem = canvas.GetComponentInChildren<EventSystem>();
//        //�õ�Canvas��Transform
//        canvasTrans = canvas.transform;
//        //���������Ƴ�Canvas ��֤Canvas��Ψһ��
//        GameObject.DontDestroyOnLoad(canvas);
//    }

//    /// <summary>
//    /// ��ʾ��巽��
//    /// </summary>
//    /// <typeparam name="T">�������</typeparam>
//    /// <returns></returns>
//    public T ShowPanel<T>() where T : BasePanel
//    {
//        //���򣺷���T����������Ҫ��Ԥ�������ֱ���һ��
//        string panelName = typeof(T).Name;

//        //�ж��ֵ����Ƿ���ڣ����Ѵ�����ֱ�ӷ���
//        if (panelDic.ContainsKey(panelName)) return panelDic[panelName] as T;

//        //����������� ��̬����Ԥ���� ���ø�����
//        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Kun/UI/" + panelName));
//        //���ø�����
//        panelObj.transform.SetParent(canvasTrans, false);

//        //�õ�������
//        T panelScript = panelObj.GetComponent<T>();
//        //���������ֵ�
//        panelDic.Add(panelName, panelScript);
//        //��ʾ���
//        panelScript.ShowMe();

//        return panelScript;
//    }

//    /// <summary>
//    /// ������巽��
//    /// </summary>
//    /// <typeparam name="T">�������</typeparam>
//    /// <param name="_needFade">�Ƿ���Ҫ����Ч����Ĭ�Ϸ�</param>
//    public void HidePanel<T>(bool _needFade = false) where T : BasePanel
//    {
//        //���򣺷���T����������Ҫ��Ԥ�������ֱ���һ��
//        string panelName = typeof(T).Name;
//        //�ж��ֵ���������Ҫ���ص����
//        if (panelDic.ContainsKey(panelName))
//        {
//            //�ж��Ƿ���Ҫ����Ч��
//            if (_needFade)
//            {
//                //�������
//                panelDic[panelName].HideMe(() =>
//                {
//                    GameObject.Destroy(panelDic[panelName].gameObject);
//                    //ɾ���ֵ��еĸ����
//                    panelDic.Remove(panelName);
//                });
//            }
//            else if (!_needFade)
//            {
//                //ֱ��ɾ�����
//                GameObject.Destroy(panelDic[panelName].gameObject);
//                //ɾ���ֵ��еĸ����
//                panelDic.Remove(panelName);
//            }
//        }
//    }

//    public T GetPanel<T>() where T : BasePanel
//    {
//        //���򣺷���T����������Ҫ��Ԥ�������ֱ���һ��
//        string panelName = typeof(T).Name;
//        //�ж��ֵ���������Ҫ�õ�����壬�����򷵻أ������򷵻ؿ�
//        if (panelDic.ContainsKey(panelName)) return panelDic[panelName] as T;
//        return null;
//    }
//}
