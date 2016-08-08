using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class SHUFTest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //StartCoroutine(download());
        //AssetBundleDownLoadManager.Instance.DownLoad("prefab");
        //AssetBundleDownLoadManager.Instance.GetDependencies();
        //ResourceListGenerator.GenerateResourceList(@"C:\Users\DSKJ-163\Desktop\Mine");
        //ResourceLoadHelper.Instance.LoadAsset("material", "material");
        Instantiate(ResourceLoadHelper.Instance.LoadAsset("prefab", "prefab"));
    }

    //IEnumerator download()
    //{
    //    //AssetBundle ab = www.assetBundle;
    //    //AssetBundleManifest mainfest = ab.LoadAsset();
    //   // string[] dps = mainfest.GetAllDependencies("prefab");
    //   // foreach (string s in dps)
    //    //    Debug.Log(s);

    //    WWW bundle = new WWW("http://192.168.21.1/StandaloneWindows/StalkerME/StalkerME");
    //    yield return bundle;
    //    Debug.Log(bundle.assetBundle);
    //    //yield return Instantiate(bundle.assetBundle.LoadAsset("prefab"));
    //    //FileStream fs = new FileStream(@"D:\123\prefab", FileAccess.Write, FileAccess.Write);
    //    //File.WriteAllBytes(, bundle.bytes);
    //    //bundle.assetBundle.Unload(false);
    //    //Debug.Log("done");
    //}
}
