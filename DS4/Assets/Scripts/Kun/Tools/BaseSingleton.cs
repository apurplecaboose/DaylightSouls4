using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 单例模式基类
/// 反射部分共存规则：继承此类的脚本需要含有一个私有无参构造函数
/// 注意：反射部分有一点点性能消耗，独立开发可省略
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseSingleton<T> where T : class//, new()
{
    private static T instance;
    //多线程加锁
    protected static readonly object lockObject = new object();
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        //instance = new T();
                        //利用反射来调用私有无参构造函数
                        Type _type = typeof(T);
                        ConstructorInfo _constructor = _type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

                        if (_constructor != null)
                        {
                            instance = _constructor.Invoke(null) as T;
                        }
                        else
                        {
                            Debug.LogError("单例模式实例 " + _type.ToString() + "：无法得到对应的私有无参构造函数。请确保该实例的脚本含有私有无参构造函数！");
                        }
                    }
                }
            }
            return instance;
        }
    }
}
