using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
using System.IO;
using SLib;
using System;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    [SerializeField]
    private Text loadingDetailText;

    void Start()
    {
        ResourceCheck();
    }

    private void UpdateDownLoadDetail(int current, int count)
    {
        //进度条等
        UpdateText("资源正在更新..（{0}/{1}）", current, count);
    }

    private void UpdateText(string format, params object[] args)
    {
        loadingDetailText.text = string.Format(format, args);
        Debug.LogFormat(format, args);
    }

    private void ResourceCheck()
    {
        if (ResourceLoadHelper.Instance.ResourceListExist)
        {
            ResourceList localResourceList = ResourceLoadHelper.Instance.LoadResourceList();
            AssetBundleDownLoadManager.Instance.GetResourceList(
            () =>
            {
                ResourceList serverResourceList = AssetBundleDownLoadManager.Instance.ServerResourceList;
                if (localResourceList.Version == serverResourceList.Version)
                {
                    UpdateText("最新游戏版本");
                    GameStart();
                }
                else if (localResourceList.Version < serverResourceList.Version)
                {
                    UpdateText("当前版本：{0}，低于游戏版本：{1}", localResourceList.Version, serverResourceList.Version);
                    AssetBundleDownLoadManager.Instance.CheckAndDownLoadAll(UpdateDownLoadDetail, GameStart, UpdateText);
                }
                else if (localResourceList.Version > serverResourceList.Version)
                {
                    UpdateText("妈的智障");
                }
            },
            UpdateText);
        }
        else
        {
            UpdateText("游戏第一次启动，加载资源中...");
            AssetBundleDownLoadManager.Instance.GetResourceList(
                () =>
                {
                    AssetBundleDownLoadManager.Instance.CheckAndDownLoadAll(UpdateDownLoadDetail, GameStart, UpdateText);
                }, 
                UpdateText);
        }
    }

    private void GameStart()
    {
        UpdateText("资源已更新至最新版本");
        SceneManager.LoadScene(1);
    }
}
