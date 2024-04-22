using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 挂载式 继承Mono的单例模式基类
/// 请注意：Mono单例脚本，可能是全局存在的，也可能是切换场景会销毁的
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

                        //我希望每个场景都可以重新创建一个新的单例来管理这个场景
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

            //我希望每个场景都可以重新创建一个新的单例来管理这个场景
            //DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            //throw new System.Exception(string.Format("检测到重复单例模式实例{0}，已自动删除{1}。", GetType().FullName, ToString()));
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
