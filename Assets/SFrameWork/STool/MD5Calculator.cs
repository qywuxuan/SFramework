using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public static class MD5Calculator
{
    private static MD5 md5;

    static MD5Calculator()
    {
        md5 = new MD5CryptoServiceProvider();
    }

    public static string CalculateMD5(string path)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] retVal = md5.ComputeHash(fs);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("MD5计算失败： {0}", ex.Message));
        }
    }
}
