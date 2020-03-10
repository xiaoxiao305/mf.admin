using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using MF.Common.Security;
using MF.Data;
using MF.Admin.DAL;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace MF.Admin.BLL
{
    public class Base
    {
        public static int redisport;
        public static string AccountServer;
        public Base()
        {
            if (ConfigurationManager.AppSettings["RedisPort"] != null)
                redisport = int.Parse(ConfigurationManager.AppSettings["RedisPort"].ToString());
            if (ConfigurationManager.AppSettings["AccountServer"] != null)
                AccountServer = ConfigurationManager.AppSettings["AccountServer"];
        }
        public static bool IsDebug
        {
            get { return BaseDAL.IsDebug; }
        }
        public static Administrator CurrentUser
        {
            get
            {
                var Session = HttpContext.Current.Session;
                var admin = Session["administrator"] as Administrator;
                return admin;
            }
        }
        public static void WriteError(params string[] err)
        {
            BaseDAL.WriteError(err);
        }
        public static void WriteLog(params string[] log)
        {
            BaseDAL.WriteLog(log);
        }
        public static bool VerificationIp(string ip)
        {
            if (string.IsNullOrEmpty(ip) || ip.Length < 7 || ip.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(ip);
        }
        ///<summary>
        ///获取客户端真实IP地址
        ///</summary>
        ///<returns></returns>
        public static string ClientIP
        {
            get
            {
                try
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.ServerVariables == null) return "";
                    string customerIP = "";
                    //CDN加速后取到的IP
                    customerIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                    if (!string.IsNullOrEmpty(customerIP))
                        return customerIP;
                    if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    {
                        customerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(customerIP))
                            customerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        else
                        {
                            //可能有代理 
                            if (customerIP.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                                customerIP = null;
                            else
                            {
                                if (customerIP.IndexOf(",") != -1)
                                {
                                    //有“,”，估计多个代理。取第一个不是内网的IP。 
                                    customerIP = customerIP.Replace(" ", "").Replace("'", "");
                                    string[] temparyip = customerIP.Split(",;".ToCharArray());
                                    for (int i = 0; i < temparyip.Length; i++)
                                    {
                                        if (VerificationIp(customerIP) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                                            return temparyip[i];    //找到不是内网的地址 
                                    }
                                }
                                else if (VerificationIp(customerIP))  //代理即是IP格式 
                                    return customerIP;
                                else
                                    customerIP = null;
                            }
                        }
                    }
                    else
                        customerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    if (string.IsNullOrEmpty(customerIP) || string.Compare(customerIP, "unknown", true) == 0)
                        customerIP = HttpContext.Current.Request.UserHostAddress;
                    if (customerIP == "::1")
                    {
                        customerIP = "127.0.0.1";
                    }
                    return customerIP;
                    /** 
                    关键就在HTTP_X_FORWARDED_FOR 
                    使用不同种类代理服务器，上面的信息会有所不同： 
                 
                    一、没有使用代理服务器的情况： 
                    REMOTE_ADDR = 您的 IP 
                    HTTP_VIA = 没数值或不显示 
                    HTTP_X_FORWARDED_FOR = 没数值或不显示 
                 
                    二、使用透明代理服务器的情况：Transparent Proxies 
                    REMOTE_ADDR = 代理服务器 IP  
                    HTTP_VIA = 代理服务器 IP 
                    HTTP_X_FORWARDED_FOR = 您的真实 IP 
                    这类代理服务器还是将您的信息转发给您的访问对象，无法达到隐藏真实身份的目的。 
                 
                    三、使用普通匿名代理服务器的情况：Anonymous Proxies 
                    REMOTE_ADDR = 代理服务器 IP  
                    HTTP_VIA = 代理服务器 IP 
                    HTTP_X_FORWARDED_FOR = 代理服务器 IP 
                    隐藏了您的真实IP，但是向访问对象透露了您是使用代理服务器访问他们的。 
                 
                    四、使用欺骗性代理服务器的情况：Distorting Proxies 
                    REMOTE_ADDR = 代理服务器 IP  
                    HTTP_VIA = 代理服务器 IP  
                    HTTP_X_FORWARDED_FOR = 随机的 IP 
                    告诉了访问对象您使用了代理服务器，但编造了一个虚假的随机IP代替您的真实IP欺骗它。 
                 
                    五、使用高匿名代理服务器的情况：High Anonymity Proxies (Elite proxies) 
                    REMOTE_ADDR = 代理服务器 IP 
                    HTTP_VIA = 没数值或不显示 
                    HTTP_X_FORWARDED_FOR = 没数值或不显示  
                    **/
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #region 关于时间
        /// <summary>
        /// 以2012-10-01开启的基准时间
        /// </summary>
        public static DateTime BaseTime
        {
            get
            {
                return BaseDAL.BaseTime;
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="datepart">时间类型s m h d M y</param>
        /// <param name="timeSpan">时间刻度数</param>
        /// <returns>返回【在2012-10-01基准时间上添加刻度】后的时间</returns>
        public static DateTime ConvertSpanToDate(string datepart, int timeSpan)
        {
            return BaseDAL.ConvertSpanToDate(datepart, timeSpan);
        }
        /// <summary>
        /// 时间比较
        /// </summary>
        /// <param name="time">需要比较的时间</param>
        /// <param name="datepart">时间类型s m h d M y</param>
        /// <returns>返回【与2012-10-01基准时间比较后的刻度数量】
        /// 大于0表示该时间在基准时间之后
        /// 小于0表示该时间在基准时间之前
        /// </returns>
        public static int ConvertDateToSpan(DateTime time, string datepart)
        {
            return BaseDAL.ConvertDateToSpan(time, datepart);
        }
        #endregion
        public static DataSet ReadExcel(string path)
        {
            return BaseDAL.ReadExcel(path);
        }
        public static Result<List<T>> ReadExcel<T>(string path)
        {
            return BaseDAL.ReadExcel<T>(path);
        }

        #region 渠道码
        public static Dictionary<int, string> ChannelNumConst
        {
            get
            {
                List<CPSUsers> list = CPSUsersBLL.GetALLChannelList();
                if (list == null || list.Count < 1)
                    return null;
                Dictionary<int, string> dic = new Dictionary<int, string>();
                foreach (CPSUsers model in list)
                {
                    if (!dic.ContainsKey(model.channel_num))
                    {
                        if (model.channel_num > 0)
                            dic.Add(model.channel_num, model.channel.ToUpper());
                    }

                }
                return dic;
            }
        }
        public static Dictionary<string, string> ChannelNameConst
        {
            get
            {
                List<CPSUsers> list = CPSUsersBLL.GetALLChannelList();
                if (list == null || list.Count < 1)
                    return null;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (CPSUsers model in list)
                {
                    if (!dic.ContainsKey(model.channel.ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(model.channel_name))
                            dic.Add(model.channel.ToUpper(), model.channel.ToUpper() + "_" + model.channel_name);
                        else
                            dic.Add(model.channel.ToUpper(), model.channel.ToUpper());
                    }

                }
                return dic;
            }
        }
        #endregion



        public static T Readjson<T>(string filePath)
        {
            try
            {
                using (StreamReader file = File.OpenText(filePath))
                {
                    var jsonText = file.ReadToEnd();
                    WriteLog("Readjson:", jsonText);
                    return JsonConvert.DeserializeObject<T>(jsonText);
                }
            }
            catch (Exception ex)
            {
                WriteError("Readjson ex:", ex.Message);
            }
            return default(T);
        }
    }
}
