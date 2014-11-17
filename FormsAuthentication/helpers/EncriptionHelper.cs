using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public static class EncriptionHelper
    {
        private static readonly byte[] _key = Encoding.ASCII.GetBytes("ABCDEFGH");
        private static readonly byte[] _iv = Encoding.ASCII.GetBytes("ABCDEFGH");
        public static string Decrypt(string stringToDecrypt)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(stringToDecrypt);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms,
              des.CreateDecryptor(_key, _iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        public static string Encrypt(string stringToEncrypt)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms,
              des.CreateEncryptor(_key, _iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
