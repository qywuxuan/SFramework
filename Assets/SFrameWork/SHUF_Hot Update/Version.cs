using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// 基于版本对象的封装类
/// </summary>
public class Version : IComparable<Version>
{
    /// <summary>
    /// 表示版本号的int型数组（长度为4）
    /// </summary>
    public int[] VersionNumber { get; set; }

    private static readonly Regex regex = new Regex(@"(\d+\.){3}\d+");

    /// <summary>
    /// 版本对象构造函数
    /// </summary>
    /// <param name="arg0">主版本号</param>
    /// <param name="arg1">子版本号</param>
    /// <param name="arg2">编译版本号</param>
    /// <param name="arg3">修正版本号</param>
    public Version(int arg0, int arg1, int arg2, int arg3)
    {
        VersionNumber = new int[4];
        VersionNumber[0] = arg0;
        VersionNumber[1] = arg1;
        VersionNumber[2] = arg2;
        VersionNumber[3] = arg3;
    }

    public Version() : this(0, 0, 0 ,1)
    {

    }

    //public Version(Vector4 vector4)
    //{
    //    VersionNumber = new int[4];
    //    VersionNumber[0] = (int)vector4.x;
    //    VersionNumber[1] = (int)vector4.x;
    //    VersionNumber[2] = (int)vector4.x;
    //    VersionNumber[3] = (int)vector4.x;
    //}

    /// <summary>
    /// 版本对象转String
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("{0}.{1}.{2}.{3}", VersionNumber[0], VersionNumber[1], VersionNumber[2], VersionNumber[3]);
    }

    /// <summary>
    /// String转版本对象
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Version Parse(string value)
    {
        value = value.Replace(',', '.');
        if (regex.IsMatch(value))
        {
            string[] values = value.Split('.');
            return new Version(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]));
        }
        else
        {
            return new Version(0, 0, 0, 1);
        }            
    }

    /// <summary>
    /// IComparable接口实现
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Version other)
    {
        for (int i = 0; i < 4; i++)
        {
            if (VersionNumber[i] > other.VersionNumber[i])
            {
                return 1;
            }
            else if (VersionNumber[i] < other.VersionNumber[i])
            {
                return -1;
            }
        }
        return 0;
    }

    public static bool operator ==(Version version1, Version version2)
    {
        bool version1isNull = Equals(null, version1);
        bool version2isNull = Equals(null, version2);

        if(version1isNull || version2isNull)
        {
            return version1isNull && version2isNull;
        }
        else
        {
            return version1.CompareTo(version2) == 0;
        }
    }

    public static bool operator !=(Version version1, Version version2)
    {
        return !(version1 == version2);
    }

    public static bool operator >(Version version1, Version version2)
    {
        return version1.CompareTo(version2) == 1;
    }

    public static bool operator <(Version version1, Version version2)
    {
        return version1.CompareTo(version2) == -1;
    }

    public override bool Equals(object obj)
    {
        Debug.LogWarning("Equals Func here never be overrided");
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        Debug.LogWarning("GetHashCode Func here never be overrided");
        return base.GetHashCode();
    }
}
