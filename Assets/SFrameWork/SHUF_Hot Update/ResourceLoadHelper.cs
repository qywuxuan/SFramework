using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLib;
using LitJson;
using System.IO;
using System;

public class ResourceLoadHelper : Singleton<ResourceLoadHelper>
{
    public bool ResourceListExist
    {
        get
        {
            return File.Exists(ResourceListPath);
        }
    }

    public AssetBundleManifest BundleManifest
    {
        get
        {
            if(bundleManifest == null)
            {
                bundleManifest = LoadAssetBundle(SHUF_Config.RootAssetBundleName).LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            }
            return bundleManifest;
        }
    }

    private readonly string ResourceListPath = SPath.Combine(SHUF_Config.PersistentDataPath, SHUF_Config.ResourceListFullName);
    private Dictionary<string, AssetBundle> bundleBuffer; //这里可以考虑对 AssetBundle 进行一层包装加上其调用时间，每次调用刷新，在固定时间间隔对List遍历，Unload最不常用的
    private AssetBundleManifest bundleManifest;

    private ResourceLoadHelper()
    {
        bundleBuffer = new Dictionary<string, AssetBundle>();
    }

    public ResourceList LoadResourceList() //后面再改成异步的吧？（或许同步更好？）
    {
        if (ResourceListExist)
        {
            //Just Go Ahead
        }
        else
        {
            throw new Exception("不存在资源列表文件");
        }

        ResourceList resourceList;
        string json = File.ReadAllText(ResourceListPath);
        resourceList = JsonMapper.ToObject<ResourceList>(json);
        Debug.LogFormat("ResourceList 文件加载完毕，当前版本：{0}", resourceList.Version);
        return resourceList;
    }

    /// <summary>
    /// 从本地加载一个Bundle，不需要手动调用
    /// </summary>
    /// <param name="bundleName">Bundle名</param>
    /// <returns></returns>
    private AssetBundle LoadAssetBundle(string bundleName)
    {
        AssetBundle assetBundle;
        string bundlePath = SPath.Combine(SHUF_Config.PersistentDataPath, bundleName);
        assetBundle = AssetBundle.LoadFromFile(bundlePath);
        bundleBuffer.Add(bundleName, assetBundle);
        return assetBundle;
    }

    /// <summary>
    /// 由ResourceLoadHelper从缓冲或本地提供一个给定的Bundle
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    private AssetBundle GetAssetBundle(string bundleName)
    {
        if(bundleBuffer.ContainsKey(bundleName))
        {
            return bundleBuffer[bundleName];
        }
        else
        {
            return LoadAssetBundle(bundleName);
        }
    }

    public UnityEngine.Object LoadAsset(string bundleName, string assetName)
    {
        string[] dependencies = GetDependencies(bundleName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            GetAssetBundle(dependencies[i]);
        }
        return GetAssetBundle(bundleName).LoadAsset(assetName);
    }

    private string[] GetDependencies(string bundleName)
    {    
        string[] dependencies = BundleManifest.GetAllDependencies(bundleName);
        return dependencies;
    }

    //IEnumerator GetDependenciesCoroutine(Action callBack)
    //{
    //    string manifestURL = SPath.Combine(SHUF_Config.DownloadURL, SHUF_Config.RootAssetBundleName);
    //    WWW www = new WWW(manifestURL);
    //    yield return www;
    //    AssetBundleManifest manifest = www.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
    //    //File.WriteAllBytes(SPath.Combine(savePath, fileName), www.bytes);
    //    //Debug.LogFormat("文件 {0} 成功下载至 {1}", fileName, savePath);
    //    //AssetBundle assetBundle = AssetBundle.CreateFromFile()
    //    //var manifest = www.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    //    string[] a = manifest.GetAllAssetBundles();
    //    Debug.Log(manifest.GetAllAssetBundles()[0]);
    //    foreach (string s in (a))
    //    {
    //        Debug.Log(s);
    //    }

    //    if (callBack != null)
    //    {
    //        callBack();
    //    }
    //}
}
