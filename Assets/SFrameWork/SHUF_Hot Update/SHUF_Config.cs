using UnityEngine;
using System.IO;

/// <summary>
/// <para>热更框架的配置类，目前需要根据项目更改的只有 ServerURL, CurrentVersion</para>
/// <para>Path：C:\ShowRays.cs</para>
/// <para>FullName：ShowRays.cs</para>
/// <para>Name：ShowRays</para>
/// </summary>
public static class SHUF_Config
{
#if UNITY_EDITOR
    #region EditorPrefNames
    public const string SHUFEditorWindowVersion = "SHUFEditorWindowVersion";
    #endregion
#endif

    #region URL相关
    public const string RootAssetBundleName = "StalkerME";
    public const string ResourceListName = "ResourceList";
    public const string ServerURL = "http://10.0.10.137";    
    public const string CurrentVersionKey = "CurrentVersion";

    public static readonly string ResourceListFullName = ResourceListName + ResourceListEX;
    public static readonly string PersistentDataPath = Application.persistentDataPath;
    public static readonly string ResourceListServerPath = SPath.Combine(DownloadURL, ResourceListFullName);
    public static readonly string ResourceListLocalPath = SPath.Combine(PersistentDataPath, ResourceListFullName);

    /// <summary>
    /// 因为不能Using UnityEidotr，所以这里使用了硬编码的URL
    /// </summary>
    public static string DownloadURL
    {
        get
        {
            if (downloadURL == null)
            {
#if UNITY_STANDALONE_WIN
                downloadURL = SPath.Combine(ServerURL, "StandaloneWindows"); //BuildTarget.StandaloneWindows.ToString()
#elif UNITY_ANDROID
                downloadURL = SPath.Combine(ServerURL, "Android"); //BuildTarget.Android.ToString()
#elif UNITY_IPHONE
                downloadURL = SPath.Combine(ServerURL, "iOS"); //BuildTarget.iOS.ToString()
#endif
                downloadURL = SPath.Combine(downloadURL, RootAssetBundleName);
                downloadURL = downloadURL.Replace('\\', '/');
            }
            return downloadURL;
        }

        private set
        {
            downloadURL = value;
        }
    }

    private static string downloadURL;
    #endregion

    #region 扩展名相关
    private const string ResourceListEX = ".json";
    #endregion
}