using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Configuration;
using MF.Enum;

namespace MF.Protocol
{
    /// <summary>
    /// 2255游戏正式使用 2019-01-18
    /// </summary>
    public class HB_SMS
    {
        static HB_SMS()
        {
            init();
        }
        static void init()
        {
            dic = new Dictionary<string, string>();
            dic.Add("100", "余额不足");
            dic.Add("101", "账号关闭");
            dic.Add("102", "短信内容超过1000字（包括1000字）或为空");
            dic.Add("103", "手机号码超过200个或合法手机号码为空或者与通道类型不匹配");
            dic.Add("104", "corp_msg_id超过50个字符或没有传corp_msg_id字段");
            dic.Add("106", "用户名不存在");
            dic.Add("107", "密码错误");
            dic.Add("108", "指定访问ip错误");
            dic.Add("109", "业务代码不存在或者通道关闭");
            dic.Add("110", "扩展号不合法");
            dic.Add("9", "访问地址不存在");
        }
        public static int Send(string phone, UserEnum.SMS_Operation type, string val)
        {
            //return Post(phone, type,val);
            return Tencent_SMS.SendSMS(phone, val);
        }
        static Dictionary<string, string> dic;
        static int Result(WebRequest r)
        {
            var response = (HttpWebResponse)r.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var res = reader.ReadToEnd().Trim();
                if (res != "0#1")
                {
                    if (dic == null && dic.Count < 1) init();
                    if (dic.ContainsKey(res))
                        Base.WriteError("发送短信失败:", dic[res], ",code:", res);
                    else
                        Base.WriteError("发送短信失败:code=", res);
                }
                else
                {
                    response.Close();
                    return 2;
                }
            }
            response.Close();
            return 0;
        }
       
        public static int Post(string phone, UserEnum.SMS_Operation type, string val)
        {
            try
            {
                if (Base.IsDebug)
                {
                    Base.WriteDebug("post to send sms is ok.phone:", phone, ",msg:", type, val);
                    return 2;
                }
                //2019-07-26 更改为大象短信平台  xxl
                string url = "https://api.capcplus.com/requestCaptchaClient.do";
                const string signatureId= "2fbbb3ca-0df1-42e5-8ff4-c49ae7a25259";
                const string ywId = "ca5525a9-8794-4546-8ada-a1dabdccf345_captcha";
                const string ywPwd = "17823460";
                string authz = MF.Common.Security.MD5.Encrypt(ywId + ywPwd).ToLower();
                string msg = "{\"code\":\""+val+"\"}";
                string templateId = "7e31635a-e291-4d40-aed1-3c336709c94c";//各种操作验证码
                if(type == UserEnum.SMS_Operation.Register)
                {
                    templateId = "	10a39907-594b-4bbf-8d11-e11220553e62";//注册验证码
                }
                string data = string.Format("mobile={0}&smsTemplateId={1}&smsSignatureId={2}&ywId={3}&authz={4}&smsParam={5}&msgId={6}",phone, templateId, signatureId,ywId, authz, msg, "");
                var request = HttpWebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.ContentLength = data.Length;
                request.Timeout = 20000;
                try
                {
                    var sw = new StreamWriter(request.GetRequestStream());
                    sw.Write(data);
                    if (sw != null)
                        sw.Close();
                    return Result(request);
                }
                catch (Exception ex)
                {
                    Base.WriteError("发送短信时异常:", ex.Message, data);
                }
            }
            catch (Exception ex2)
            {
                Base.WriteError("发送短信时异常2：", ex2.Message, phone, type,val);
            }
            return 0;

        }
    }
}
