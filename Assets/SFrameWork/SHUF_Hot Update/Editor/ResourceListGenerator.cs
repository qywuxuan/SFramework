using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using SLib;
using System;
using System.Text;

public static class ResourceListGenerator
{
    private static readonly string[] ignoreFliter = new string[] { ".manifest" };

    public static void GenerateResourceList(string path)
    {
        string[] filePaths = SPath.GetPathsByEX(path, true, ignoreFliter);
        ResourceList.Data[] datas = new ResourceList.Data[filePaths.Length];

        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = new ResourceList.Data(filePaths[i]);
        }

        ResourceList resourceList = new ResourceList(SHUFEditorWindow.TargetVersion ,datas);
        File.WriteAllText(SPath.Combine(path, SHUF_Config.ResourceListFullName), JsonMapper.ToJson(resourceList));
        Debug.LogFormat("ResourceList 文件 {0} 生成完毕，Path: {1}", SHUF_Config.ResourceListFullName, path);
    }
}
