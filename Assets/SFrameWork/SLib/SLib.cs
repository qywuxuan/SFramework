using UnityEngine;
using System.Collections;

namespace SLib
{
    public delegate void WrongDelegate(string format, params object[] args);
    public delegate void StepDelegate(int current, int count);
    public delegate void CallBackDelegate();

    public struct ResourceList
    {
        public Version Version;
        public Data[] Datas;

        public ResourceList(Version version, Data[] datas)
        {
            Version = version;
            Datas = datas;
        }

        public struct Data
        {
            public string Name;
            public string MD5;

            public Data(string name, string md5)
            {
                Name = name;
                MD5 = md5;
            }

            public Data(string path)
            {
                Name = SPath.GetFullNameByPath(path);
                MD5 = MD5Calculator.CalculateMD5(path);
            }
        }
    }
}
