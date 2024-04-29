using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ����ģʽ����
/// ���䲿�ֹ�����򣺼̳д���Ľű���Ҫ����һ��˽���޲ι��캯��
/// ע�⣺���䲿����һ����������ģ�����������ʡ��
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseSingleton<T> where T : class//, new()
{
    private static T instance;
    //���̼߳���
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
                        //���÷���������˽���޲ι��캯��
                        Type _type = typeof(T);
                        ConstructorInfo _constructor = _type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

                        if (_constructor != null)
                        {
                            instance = _constructor.Invoke(null) as T;
                        }
                        else
                        {
                            Debug.LogError("����ģʽʵ�� " + _type.ToString() + "���޷��õ���Ӧ��˽���޲ι��캯������ȷ����ʵ���Ľű�����˽���޲ι��캯����");
                        }
                    }
                }
            }
            return instance;
        }
    }
}
