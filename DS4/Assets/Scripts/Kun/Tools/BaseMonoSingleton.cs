using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ʽ �̳�Mono�ĵ���ģʽ����
/// ��ע�⣺Mono�����ű���������ȫ�ִ��ڵģ�Ҳ�������л����������ٵ�
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object instanceLock = new object();
    private static bool quitting = false;
    public static T Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null && !quitting)
                {
                    instance = GameObject.FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject gameObject = new GameObject(typeof(T).ToString());
                        instance = gameObject.AddComponent<T>();

                        //��ϣ��ÿ���������������´���һ���µĵ����������������
                        //DontDestroyOnLoad(instance.gameObject);
                    }
                }
                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<T>();

            //��ϣ��ÿ���������������´���һ���µĵ����������������
            //DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            //throw new System.Exception(string.Format("��⵽�ظ�����ģʽʵ��{0}�����Զ�ɾ��{1}��", GetType().FullName, ToString()));
        }

        Init();
    }
    protected virtual void OnApplicationQuit()
    {
        quitting = true;
    }

    protected virtual void Init() { }

    protected virtual void OnDestroy() 
    {
        instance = null;
    }
}
