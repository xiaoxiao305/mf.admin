using System;
using System.Net;
using System.IO;
using System.Xml;
using MF.Enum;

namespace MF.Protocol
{
    public class SMS
    {
        static int iHuyiSMS_Send(string phone, string msg)
        {
            try
            {
                var url = string.Format("http://106.ihuyi.cn/webservice/sms.php?method=Submit&account=cf_qimen&password=7c3s9u&mobile={0}&content={1}", phone, msg);
                HttpWebRequest r = (HttpWebRequest)HttpWebRequest.Create(url);
                using (WebResponse res = r.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string str = sr.ReadToEnd();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(str);
                        XmlNodeList l = xml.GetElementsByTagName("code");
                        if (l != null && l.Count > 0)
                        {
                            int i = 0;
                            int.TryParse(l.Item(0).InnerText, out i);
                            return i;
                        }
                        else
                            Base.WriteError("发送短信返回值解析错误:res=", str);
                    }
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("发送短信验证码时异常:", ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// 发短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int Send(string phone, UserEnum.SMS_Operation type, string val)
        {
            return HB_SMS.Send(phone, type,val);
        }
        /// <summary>
        /// 群发短信
        /// </summary>
        /// <param name="phones"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int Send(string[] phones, string msg)
        {
            try
            {
                string arr = "";
                foreach (string p in phones)
                    arr += p + ",";
                arr = arr.Substring(0, arr.Length - 1);
                string url = string.Format("http://ihuyi.10658.cc/webservice/api?method=SendSms&account=hy_qmwl&password=qmwl321&mobile={0}&content={1}【2255棋牌】&pid=32", arr, msg);
                HttpWebRequest r = (HttpWebRequest)HttpWebRequest.Create(url);
                using (WebResponse res = r.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string str = sr.ReadToEnd();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(str);
                        XmlNodeList l = xml.GetElementsByTagName("code");
                        if (l != null && l.Count > 0)
                        {
                            int i = 0;
                            int.TryParse(l.Item(0).InnerText, out i);
                            if (i != 2)
                                Base.WriteError("群发短信失败，返回值：", str);
                            return i;
                        }
                        else
                            Base.WriteError("群发送短信返回值解析错误:res=", str);
                    }
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("群发短信时异常：", ex.Message);

            }
            return 0;

        }
    }
}
