using System;
using System.Collections.Generic;
using System.Text;

namespace mf.token
{
    public class Security
    {
        /// <summary>
        /// 根据key生成相应的MD5校验码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> Create(string iukey)
        {
            List<string> list = new List<string>();
            DateTime start = DateTime.Parse("1970-01-01 00:00:00");
            DateTime end = DateTime.Now.AddHours(-8);
            TimeSpan span = end - start;
            int totalsecond = Convert.ToInt32(span.TotalSeconds);//自1970-01-01 00:00:00到当前时间的总秒数
            int msecd = totalsecond % 30;
            int[] seconds = { totalsecond - msecd, totalsecond - msecd - 30, totalsecond - msecd - 60, totalsecond - msecd - 90, totalsecond - msecd + 30, totalsecond - msecd + 60, totalsecond - msecd + 90 };
            foreach (var s in seconds)
                list.Add(MD5.Encrypt(iukey + s).ToLower());
            return list;
        }

        /// <summary>
        /// 将md5码转换为8位数字码
        /// </summary>
        /// <param name="md5Str">md5</param>
        /// <returns></returns>
        public static string GetToken(string md5Str)
        {
            md5Str = md5Str.ToLower();
            StringBuilder val = new StringBuilder();
            for (int i = 0; i < md5Str.Length; i = i + 4)
            {
                string t = md5Str.Substring(i, 4);
                byte[] temp = Encoding.UTF8.GetBytes(t);
                ushort t0 = Convert.ToUInt16(temp[0]);
                ushort t1 = Convert.ToUInt16(temp[1]);
                ushort t2 = Convert.ToUInt16(temp[2]);
                ushort t3 = Convert.ToUInt16(temp[3]);
                int ret = t1 * 256 + t0 + t3 * 256 + t2;
                val.Append((ret % 10).ToString());
            }
            return val.ToString();
        }
    }
}
