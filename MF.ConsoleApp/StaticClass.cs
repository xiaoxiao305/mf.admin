using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
    /// <summary>
    /// 参数类
    /// </summary>
    public class SendCode
    {
        public string Ext { get; set; }
        public string Extend { get; set; }
        public string[] Params { get; set; }
        public string Sig { get; set; }
        public string Sign { get; set; }
        public Phone Tel { get; set; }
        public string Time { get; set; }
        public string Tpl_id { get; set; }
    }
    /// <summary>
    /// 电话参数
    /// </summary>
    public class Phone
    {
        public string Mobile { get; set; }
        public string Nationcode { get; set; }
    }
    public static class StaticClass
    {
        /// <summary>
        /// 返回指定个数的随机数
        /// </summary>
        /// <param name="length">个数</param>
        /// <returns>随机数</returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }
        /// <summary>
        /// 获取时间戳格式
        /// </summary>
        /// <param name="flg">多少位的时间戳</param>
        /// <returns>时间戳</returns>
        public static long GetTimeStamp(int flg)
        {
            long time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            switch (flg)
            {
                case 10:
                    time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    break;
                case 13:
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                    time = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
                    break;
            }
            return time;
        }
        /// <summary>
        /// Sha256加密算法
        /// </summary>
        /// <param name="data">加密的内容</param>
        /// <returns>加密后的数据</returns>
        public static string Sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < SHA256.Create().ComputeHash(bytes).Length; i++)
            {
                builder.Append(SHA256.Create().ComputeHash(bytes)[i].ToString("X2"));
            }
            return builder.ToString();
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postdata">参数</param>
        /// <returns>返回内容</returns>
        public static string HttpPost(string url, string postdata)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Referer = null;
            req.AllowAutoRedirect = true;
            req.Accept = "*/*";

            byte[] data = Encoding.UTF8.GetBytes(postdata);
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
