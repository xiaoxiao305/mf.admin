using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace mf.token
{
    public class MD5
    {
        /// <summary>
        /// MD5加密,返回结果为大写的MD5码
        /// </summary>
        /// <param name="source">待加密的源字符串</param>
        /// <returns>返回结果为大写的32位MD5码</returns>
        public static string Encrypt(string source)
        {
            string md5String = string.Empty; 
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] byteCode =  Encoding.GetEncoding("GB2312").GetBytes(source);
            byteCode = new MD5CryptoServiceProvider().ComputeHash(byteCode);

            for (int i = 0; i < byteCode.Length; i++)
                md5String += byteCode[i].ToString("x").PadLeft(2, '0');

            return md5String.ToUpper();
        }
    }
}
