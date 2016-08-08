using UnityEngine;
using System.Collections;
using System.IO;
using System;
using LitJson;
using SLib;
using System.Collections.Generic;

public class AssetBundleDownLoadManager : MonoSingleton<AssetBundleDownLoadManager>
{
    public ResourceList ServerResourceList
    {
        get
        {
            if(serverResourceList == null)
            {
                throw new Exception("未获取服务器 ResourceList");
            }
            else
            {
                //Do nothing
            }
            return (ResourceList)serverResourceList;
        }
    }

    private AssetBundleManifest mainfest;
    private readonly string savePath = SHUF_Config.PersistentDataPath;
    private ResourceList? serverResourceList;

    /// <summary>
    /// 对服务器 ResourceList（需要提前获取）中的差异文件进行对比下载，并同步ResourceList (使用协程)
    /// </summary>
    /// <param name="stepCallBack"></param>
    /// <param name="callBack"></param>
    /// <param name="wrongCallBack"></param>
    public void CheckAndDownLoadAll(StepDelegate stepCallBack, CallBackDelegate callBack, WrongDelegate wrongCallBack) //暂时同步后面用协程改
    {
        StartCoroutine(CheckAndDownLoadAllCoroutine(stepCallBack, callBack, wrongCallBack));
    }

    /// <summary>
    /// 获取服务器 ResourceList (使用协程)
    /// </summary>
    public void GetResourceList(CallBackDelegate callBack, WrongDelegate wrongCallBack)
    {
        if (serverResourceList != null)// || Coroutine is running)
        {
            if(callBack != null)
            {
                callBack();
            }
        }
        else
        {
            StartCoroutine(GetResourceListCoroutine(callBack, wrongCallBack));
        }
    }

    /// <summary>
    /// 使用服务器 ResourceList 对本地 ResourceList 进行同步
    /// </summary>
    private void SetResourceList()
    {
        if (serverResourceList == null)
        {
            throw new Exception("请先获取服务器 ResourceList 再下载");
        }

        File.WriteAllText(SHUF_Config.ResourceListLocalPath, JsonMapper.ToJson(serverResourceList));
        Debug.Log("ResourceList 已同步");
    }

    //private void DownLoad(string[] filefullNames, StepDelegate stepCallBack, CallBackDelegate callBack, WrongDelegate wrongCallBack)
    //{
    //    StartCoroutine(DownLoadCoroutine(filefullNames, stepCallBack, callBack, wrongCallBack));
    //}

    #region Coroutine
    IEnumerator GetResourceListCoroutine(CallBackDelegate callBack, WrongDelegate wrongCallBack)
    {
        WWW www = new WWW(SHUF_Config.ResourceListServerPath);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            if (wrongCallBack != null)
            {
                wrongCallBack("服务器连接失败");
            }
            yield break;
        }
        serverResourceList = JsonMapper.ToObject<ResourceList>(www.text);
        if(callBack != null)
        {
            callBack();
        }
    }

    IEnumerator DownLoadCoroutine(string[] fileFullNames, StepDelegate stepCallBack ,CallBackDelegate callBack, WrongDelegate wrongCallBack) //不要每次都返回，beforeCallBack?
    {
        for(int i = 0; i < fileFullNames.Length; i++)
        {
            WWW www = new WWW(SPath.Combine(SHUF_Config.DownloadURL, fileFullNames[i]));
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                if (wrongCallBack != null)
                {
                    wrongCallBack("文件下载失败，当前 ({0}/{1})",i + 1, fileFullNames.Length);
                }
                yield break;
            }

            File.WriteAllBytes(SPath.Combine(savePath, fileFullNames[i]), www.bytes);
            Debug.LogFormat("文件 {0} 成功下载至 {1}", fileFullNames[i], savePath);
            if(stepCallBack != null)
            {
                stepCallBack(i + 1 , fileFullNames.Length);
            }
        }

        if(callBack != null)
        {
            callBack();
        }
    }

    IEnumerator CheckAndDownLoadAllCoroutine(StepDelegate stepCallBack, CallBackDelegate callBack, WrongDelegate wrongCallBack)
    {
        List<string> filefullNamesWhichNeedToBeDownLoaded = new List<string>();
        int dataLength = ServerResourceList.Datas.Length;

        for (int i = 0; i < dataLength; i++)
        {
            string localPath = SPath.Combine(SHUF_Config.PersistentDataPath, ServerResourceList.Datas[i].Name);
            if (File.Exists(localPath))
            {
                string localMd5 = MD5Calculator.CalculateMD5(localPath);
                string serverMd5 = ServerResourceList.Datas[i].MD5;
                if (localMd5.Equals(serverMd5))
                {
                    //Do nothing
                }
                else
                {
                    filefullNamesWhichNeedToBeDownLoaded.Add(ServerResourceList.Datas[i].Name);
                }
            }
            else
            {
                filefullNamesWhichNeedToBeDownLoaded.Add(ServerResourceList.Datas[i].Name);
            }
        }

        yield return StartCoroutine(DownLoadCoroutine(
            filefullNamesWhichNeedToBeDownLoaded.ToArray(),
            stepCallBack,
            () =>
            {
                SetResourceList();
                if (callBack != null)
                {
                    callBack();
                }
            },
            wrongCallBack));
    }
    #endregion
}