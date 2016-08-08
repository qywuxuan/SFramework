using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text;
using System.Reflection;

public class SHUFEditorWindow : EditorWindow
{
    private enum AdditionalOption
    {
        EliminateManifest = 1,
        AutoVersion = 2,
        //4, 8, 16, 32...etc
    }

    private const string toolName = "SHUF AssetBundle 生成工具";
    private const string menu = "AssetBundle/";
    private static readonly Vector2 RectSize = new Vector2(300, 162);
    private static readonly Vector2 RectPos = new Vector2(Screen.width - RectSize.x, Screen.height - RectSize.y) / 2f;
    private static readonly Rect wr = new Rect(RectPos, RectSize);
    private static readonly Type type = Type.GetType("SHUFEditorWindow");

    #region UI用变量
    public static Version TargetVersion
    {
        get
        {
            if(versionField == null)
            {
                if (EditorPrefs.HasKey(SHUF_Config.SHUFEditorWindowVersion))
                {
                    versionField = Version.Parse(EditorPrefs.GetString(SHUF_Config.SHUFEditorWindowVersion));
                }
                else
                {
                    versionField = new Version();
                }
            }
            return versionField;
        }

        private set
        {
            versionField = value;
            EditorPrefs.SetString(SHUF_Config.SHUFEditorWindowVersion, value.ToString());
            Debug.LogFormat("打包版本变更 -> {0}", value);
        }
    }

    private static bool bundleSetFold
    {
        get
        {
            if (bundleSetFoldField == null)
            {
                bundleSetFoldField = true;
            }
            return (bool)bundleSetFoldField;
        }

        set
        {
            bundleSetFoldField = value;
        }
    }

    private static bool versionFold
    {
        get
        {
            if (versionFoldField == null)
            {
                versionFoldField = true;
            }
            return (bool)versionFoldField;
        }

        set
        {
            versionFoldField = value;
        }
    }

    private static BuildTarget buildTarget
    {
        get
        {
            if (buildTargetField == null)
            {
                buildTargetField = BuildTarget.StandaloneWindows;
            }
            return (BuildTarget)buildTargetField;
        }

        set
        {
            buildTargetField = value;
        }
    }

    private static BuildAssetBundleOptions buildAssetBundleOptions
    {
        get
        {
            if (buildAssetBundleOptionsField == null)
            {
                buildAssetBundleOptionsField = BuildAssetBundleOptions.None;
            }
            return (BuildAssetBundleOptions)buildAssetBundleOptionsField;
        }

        set
        {
            buildAssetBundleOptionsField = value;
        }
    }

    private static AdditionalOption additionalOption
    {
        get
        {
            if (additionalOptionField == null)
            {
                additionalOptionField = 0;  //nothing = 0, everything = -1;
            }
            return (AdditionalOption)additionalOptionField;
        }

        set
        {
            additionalOptionField = value;
        }
    }

    #region Field
    private static Version versionField;
    private static bool? bundleSetFoldField;
    private static bool? versionFoldField;
    private static BuildTarget? buildTargetField;
    private static BuildAssetBundleOptions? buildAssetBundleOptionsField;
    private static AdditionalOption? additionalOptionField;
    #endregion
    #endregion

    [MenuItem(menu + toolName)]
    static void ShowMainWindow()
    {
        SHUFEditorWindow window = GetWindowWithRect(typeof(SHUFEditorWindow), wr, true, toolName) as SHUFEditorWindow;
        window.Show();
    }

    void OnGUI()
    {
        Paint();
        //EditorGUILayout.Space();
        //ShowBundleTools();
        //EditorGUILayout.Space();
    }

    private void Paint()
    {
        EditorGUI.DropShadowLabel(wr, "v1.0 by StalkerME");
        EditorGUILayout.BeginVertical();
        {
            bundleSetFold = EditorGUILayout.Foldout(bundleSetFold, "打包设置");
            if (bundleSetFold)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("AssetBundle 目标平台");
                    buildTarget = (BuildTarget)EditorGUILayout.EnumPopup(buildTarget);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                { 
                    EditorGUILayout.LabelField("AssetBundle 打包参数");
                    buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup(buildAssetBundleOptions);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    //EditorGUILayout.LabelField("其他设置");
                    additionalOption = (AdditionalOption)EditorGUILayout.EnumMaskPopup(new GUIContent("额外选项"), additionalOption);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                //Just fold
            }

            versionFold = EditorGUILayout.Foldout(versionFold, "版本号");
            if (versionFold)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    int gap = 20;
                    EditorGUILayout.LabelField("主版本号", GUILayout.Width((wr.width - gap) / 4f));
                    EditorGUILayout.LabelField("子版本号", GUILayout.Width((wr.width - gap)/ 4f));
                    EditorGUILayout.LabelField("编译版本号", GUILayout.Width((wr.width - gap)/ 4f));
                    EditorGUILayout.LabelField("修正版本号", GUILayout.Width((wr.width - gap)/ 4f));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    TargetVersion.VersionNumber[0] = EditorGUILayout.IntField(TargetVersion.VersionNumber[0]);

                    TargetVersion.VersionNumber[1] = EditorGUILayout.IntField(TargetVersion.VersionNumber[1]);

                    TargetVersion.VersionNumber[2] = EditorGUILayout.IntField(TargetVersion.VersionNumber[2]);

                    TargetVersion.VersionNumber[3] = EditorGUILayout.IntField(TargetVersion.VersionNumber[3]);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                //Just fold
            }

            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("导出"))
                {
                    ExportAssetBundle(buildTarget, buildAssetBundleOptions);
                }

                if(GUILayout.Button("重置"))
                {
                    EditorPrefs.DeleteKey(SHUF_Config.SHUFEditorWindowVersion);
                    FieldInfo[] fis = type.GetFields(BindingFlags.Static | BindingFlags.NonPublic);
                    for (int i = 0; i < fis.Length; i++)
                    {
                        if (fis[i].Name.EndsWith("Field"))
                        {
                            fis[i].SetValue(null, null);
                        }
                    }
                    //this.Close();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private static void CheckExportBundle()
    {

    }

    [MenuItem(menu + "Clear Persistent Data")]
    public static void ClearPersistentData()
    {
        if (Directory.Exists(SHUF_Config.PersistentDataPath))
        {
            Directory.Delete(SHUF_Config.PersistentDataPath, true);
            Directory.CreateDirectory(SHUF_Config.PersistentDataPath);
            Debug.LogFormat("Persistent Data 已被清空， Path: {0} ", SHUF_Config.PersistentDataPath);
        }
        else
        {
            Debug.LogWarningFormat("Persistent Data Path: {0} 暂时不存在，不需要清空", SHUF_Config.PersistentDataPath);
        }
    }

    private static void ExportAssetBundle(BuildTarget buildTarget, BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None)
    {
        string exportPath = EditorUtility.OpenFolderPanel("请选择AssetBundle的导出目录", "", "");
        if (exportPath == "") return;

        TargetVersion = TargetVersion;
        DateTime startTime = DateTime.Now;
        exportPath = SPath.Combine(exportPath, buildTarget.ToString());

        string targetPath = SPath.Combine(exportPath, SHUF_Config.RootAssetBundleName);

        if (Directory.Exists(targetPath))
        {
            Directory.Delete(targetPath, true);
        }
        Directory.CreateDirectory(targetPath);

        BuildPipeline.BuildAssetBundles(targetPath, buildAssetBundleOptions, buildTarget);
        ResourceListGenerator.GenerateResourceList(SPath.Combine(exportPath, SHUF_Config.RootAssetBundleName));

        Array allAdditionalOption = Enum.GetValues(typeof(AdditionalOption));
        for(int i = 0; i < allAdditionalOption.Length; i++)
        {
            AdditionalOption currentAdditionalOption = (AdditionalOption)allAdditionalOption.GetValue(i);
            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Static))
            {
                Debug.Log(mi.Name);
            }
            if((additionalOption & currentAdditionalOption) != 0)
            {
                try
                {
                    //MethodInfo method = type.GetMethod(currentAdditionalOption.ToString(), BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null);
                    //method.Invoke(null, new string[] { exportPath });
                    type.InvokeMember(currentAdditionalOption.ToString(), BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static, null, null, new string[] { exportPath });
                }
                catch (Exception)
                {
                    Debug.LogErrorFormat("打包额外操作时报错，可能是没有定义 {0} 方法", currentAdditionalOption.ToString());
                }
            }
        }

        DateTime endTime = DateTime.Now;
        Debug.LogFormat("{0} AssetBundle 导出完毕 path : {1}\n耗时 {2}, (buildAssetBundleOptions : {3})", buildTarget, targetPath, GetCostTime(startTime, endTime), buildAssetBundleOptions);
        CheckExportBundle();
    }

    private static string GetCostTime(DateTime startTime, DateTime endTime)
    {
        StringBuilder stringBuilder = new StringBuilder();
        TimeSpan timeSpan = endTime.Subtract(startTime);

        stringBuilder.TimeAppend(timeSpan, true);

        return stringBuilder.ToString();
    }

    #region AdditionalOption Method
    private static void EliminateManifest(string exportPath)
    {
        Debug.Log("EliminateManifest");
        string[] manifestPaths = SPath.GetPathsByEX(exportPath, false, new string[] { ".manifest" });
        for(int i = 0; i < manifestPaths.Length; i++)
        {
            File.Delete(manifestPaths[i]);
        }
    }

    private static void AutoVersion(string exportPath)
    {
        Debug.Log("AutoVersion");
        TargetVersion.VersionNumber[3] += 1;
        TargetVersion = TargetVersion;
    }
    #endregion
}
