using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

/// <summary>
/// 单例模式抽象基类，必须手动实现子类的private构造函数（否则会在运行时对New进行报错，虽然理论上也不支持New操作）
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : Singleton<T>
{
    public static readonly object SyncObject = new object();

    private static T instance = null;

    /// <summary>
    /// 类单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null) //没有第一重 singleton == null 的话，每一次有线程进入 GetInstance()时，均会执行锁定操作来实现线程同步，非常耗费性能 增加第一重singleton ==null 成立时的情况下执行一次锁定以实现线程同步
            {
                lock (SyncObject)
                {
                    if (instance == null) //Double-Check Locking 双重检查锁定
                    {
                        instance = (T)Activator.CreateInstance(typeof(T), true); 
                    }
                }
            }
            return instance;
        }
    }

    protected Singleton()
    {
        if(typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length == 0)
        {
            throw new Exception("Singleton子类没有实现private构造函数");
        }
    }
}
