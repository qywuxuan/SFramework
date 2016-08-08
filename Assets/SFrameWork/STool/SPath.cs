using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Linq;

/// <summary>
/// 路径相关
/// </summary>
public static class SPath
{
    /// <summary>
    /// 简单实现了Path类不具备的Http地址连接
    /// </summary>
    public static string Combine(params string[] args)
    {
        if(args.Length < 2)
        {
            throw new ArgumentException("请输入至少两条路径");
        }
        else
        {
            string output = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                output = Path.Combine(output, args[i]);
            }
            output = output.Replace('\\', '/');
            return output;
        }
    }

    /// <summary>
    /// 获得指定路径下包含（或不包含）某些扩展名的所有文件名
    /// </summary>
    /// <param name="path">指定路径</param>
    /// <param name="filter">是否为过滤模式（不包含某些扩展名）</param>
    /// <param name="fullpath">是否返回完全路径</param>
    /// <param name="Exs">指定的扩展名（可选参数, 例如new string[] filter{".jpg"}）</param>
    /// <returns></returns>
    public static string[] GetPathsByEX(string path, bool filter, params string[] Exs)
    {
        if(Exs.Length == 0)
        {
            throw new Exception("请输入至少一个需要过滤的扩展名");
        }
                                                                                  //DirectoryInfo resourceFolder = new DirectoryInfo(path);
        var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories). //FileInfo[] fileInfos = resourceFolder.GetFiles("*", SearchOption.AllDirectories);
            Where(s =>
            {
                for (int i = 0; i < Exs.Length; i++)
                {
                    if(s.EndsWith(Exs[i]))
                    {
                        return !filter;
                    }
                }
                return filter;
            });
        return files.ToArray();
    }

    /// <summary>
    /// 通过完全路径获取文件名（含扩展名）
    /// </summary>
    /// <param name="path">文件完全路径</param>
    /// <returns></returns>
    public static string GetFullNameByPath(string path)
    {
        path = path.Replace('\\', '/');
        int lastCharIndex = path.LastIndexOf('/');
        int fullPathLength = path.Length;

        return path.Substring(lastCharIndex + 1, fullPathLength - lastCharIndex - 1);
    }
}
