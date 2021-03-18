using MF.Common.Json;
using MF.Common.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Configuration;

namespace MF.Protocol
{
    public class JTokenResolver
    {
        public static void Resolve(IList<object> root, JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Array:
                    IList<object> list = new List<object>();
                    foreach (JToken item in token)
                    {
                        Resolve(list, item);
                    }
                    root.Add(list);
                    break;
                case JTokenType.Integer:
                    root.Add(token.Value<int>());
                    break;
                case JTokenType.Float:
                    root.Add(token.Value<int>());
                    break;
                case JTokenType.String:
                    root.Add(token.Value<string>());
                    break;
                case JTokenType.Boolean:
                    root.Add(token.Value<bool>());
                    break;
                case JTokenType.Null:
                    root.Add(null);
                    break;
                case JTokenType.Undefined:
                    break;
                case JTokenType.Date:
                    root.Add(token.Value<DateTime>());
                    break;
                case JTokenType.Bytes:
                default:
                    break;
            }
        }
    }
    [Serializable]
    public class PostResult<T>
    {
        public int Code { get; set; }
        public T R { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
    }
    public class Http
    {
        /// <summary>
        /// 请求账号服务器
        /// 使用Web对应key【WEB_SECURITY】
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="requestUrl">请求服务器URL 1其后追加protocol 2若不追加protocol，则protocol置args首位</param>
        /// <param name="args">参数集合  若requestUrl不追加protocol，则protocol置args首位</param>
        /// <returns>返回T数据类型</returns>
        public static T Post<T>(string requestUrl, params object[] args)
        {
            string param = "";
            try
            {
                param = makeArgs(Base.config.WebServerKey, args);
                if (string.IsNullOrEmpty(param))
                {
                    Base.WriteError("Post<T> param is empty.");
                    return default(T);
                }
                HttpWebRequest request = requestServerUrl(requestUrl, param);
                if (request == null)
                {
                    Base.WriteError("Post<T> requestServerUrl is null.");
                    return default(T);
                }
                return Deserialize<T>(request);
            }
            catch (Exception ex2)
            {
                Base.WriteError("Post ex：", ex2.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        /// <summary>
        /// 请求账号服务器
        /// 参数为包含sign在内打包好的json字符串
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="requestUrl">请求服务器URL</param>
        /// <param name="param">包含sign在内打包好的json字符串</param>
        /// <returns>返回T数据类型</returns>
        public static T Post<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(param))
                    return default(T);
                HttpWebRequest request = requestServerUrl(requestUrl, param);
                if (request == null)
                {
                    Base.WriteError("Post<T> requestServerUrl is bull.");
                    return default(T);
                }
                return Deserialize<T>(request);
            }
            catch (Exception ex2)
            {
                Base.WriteError("Post(param) ex：", ex2.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        public static T Post<T>(string requestUrl)
        {
            try
            {
                Base.WriteDebug("requestUrl is ", requestUrl);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = "POST";
                using (var reqStream = request.GetRequestStream())
                {
                    return Deserialize<T>(request);
                }
            }
            catch (Exception ex2)
            {
                Base.WriteError("Post(param) ex：", ex2.Message, ",requestUrl:", requestUrl);
            }
            return default(T);
        }
        /// <summary>
        /// 请求充值服务器
        /// 使用Charge对应key【CHARGE_SECURITY】
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="requestUrl">请求服务器URL 1其后追加protocol 2若不追加protocol，则protocol置args首位</param>
        /// <param name="args">参数集合  若requestUrl不追加protocol，则protocol置args首位</param>
        /// <returns>返回T数据类型</returns>
        public static T PostCharge<T>(string requestUrl, params object[] args)
        {
            string param = "";
            try
            {
                param = makeArgs(Base.config.ChargeServerKey, args);
                if (string.IsNullOrEmpty(param))
                {
                    Base.WriteError("PostCharge<T> param is empty.");
                    return default(T);
                }
                HttpWebRequest request = requestServerUrl(requestUrl, param);
                if (request == null)
                {
                    Base.WriteError("PostCharge<T> requestServerUrl is null.");
                    return default(T);
                }
                return Deserialize<T>(request);
            }
            catch (Exception ex2)
            {
                Base.WriteError("PostCharge ex：", ex2.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        /// <summary>
        /// 用户充值模块，向服务器返回验证结果【iOS、微信】
        /// 不解析返回值
        /// </summary>
        public static void PostCharge(string requestUrl, params object[] args)
        {
            string param = "";
            try
            {
                param = makeArgs(Base.config.ChargeServerKey, args);
                if (string.IsNullOrEmpty(param))
                {
                    Base.WriteError("PostCharge param is empty.");
                    return;
                }
                HttpWebRequest request = requestServerUrl(requestUrl, param);
                if (request == null)
                {
                    Base.WriteError("PostCharge requestServerUrl is null.");
                    return;
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    //var res = reader.ReadToEnd();
                    //Base.WriteLog("post server appleres:", param);
                    response.Close();
                }
            }
            catch (Exception ex2)
            {
                Base.WriteError("PostCharge[app] ex：", ex2.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
        }
        /// <summary>
        /// 请求服务器读取数据记录
        /// 使用Web对应key【WEB_SECURITY】
        /// </summary> 
        /// <param name="requestUrl">请求服务器URL，其后追加protocol</param>
        /// <param name="param">包括sign在内的，已封装的json格式参数字符串</param>
        ///<remarks>返回Result<List<T>>数据类型</remarks>
        //public static PostResult<List<T>> PostRecordServer<T>(string requestUrl, string param)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(requestUrl))
        //        {
        //            Base.WriteError("PostRecordServer requestUrl is empty.");
        //            return null;
        //        }
        //        Base.WriteDebug("requestServerUrl22222 is ", requestUrl, ".param is ", param);
        //        var request = WebRequest.Create(requestUrl) as HttpWebRequest;
        //        var buffer = Encoding.UTF8.GetBytes(param);
                



        //        request.Method = "POST";
        //        request.Timeout = 600000;
        //        request.ReadWriteTimeout = 600000;
        //        request.AllowWriteStreamBuffering = false;
        //        request.SendChunked = true;
        //        Base.WriteDebug("111111");
        //        using (var reqStream = request.GetRequestStream())
        //        {
        //            Base.WriteDebug("22222222");
        //            reqStream.Write(buffer, 0, buffer.Length);
        //            Base.WriteDebug("33333");
        //            if (request != null) {
        //                var response = (HttpWebResponse)request.GetResponse();
        //                Base.WriteDebug("request != null");
        //                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        //                {
        //                    var res = reader.ReadToEnd();
        //                    Base.WriteDebug("recordserver'res1111 is ", res);
        //                    try
        //                    {
        //                        var jarry = JsonConvert.DeserializeObject(res) as JArray;
        //                        if (jarry == null)
        //                        {
        //                            Base.WriteError("DeserializeRecord JsonConvert.DeserializeObject(res) is null");
        //                            return default(PostResult<List<T>>);
        //                        }
        //                        List<object> list = new List<object>();
        //                        foreach (JToken item in jarry)
        //                        {
        //                            JTokenResolver.Resolve(list, item);
        //                        }
        //                        if (list == null || list.Count < 1)
        //                        {
        //                            Base.WriteError("DeserializeRecord JTokenResolver.Resolve'list is null.");
        //                            return default(PostResult<List<T>>);
        //                        }
        //                        if (list[0] == null || string.IsNullOrEmpty(list[0].ToString()))
        //                        {
        //                            Base.WriteError("DeserializeRecord res not has data.");//返回约定格式有误
        //                            return default(PostResult<List<T>>);
        //                        }
        //                        if (int.Parse(list[0].ToString()) < 1)
        //                        {
        //                            //Base.WriteError("DeserializeRecord res has'data is err.");//无数据
        //                            return default(PostResult<List<T>>);
        //                        }
        //                        if (list.Count != 3)
        //                        {
        //                            // Base.WriteError("DeserializeRecord res'num is bad count.");//无数据
        //                            return default(PostResult<List<T>>);
        //                        }
        //                        PostResult<List<T>> r = new PostResult<List<T>>() { Code = int.Parse(list[0].ToString()) };
        //                        var keys = list[1] as List<object>;
        //                        var values = list[2] as List<object>;
        //                        List<object> valuelist = null;
        //                        var sb = new StringBuilder();
        //                        sb.Append("[");
        //                        for (int i = 0; i < values.Count; i++)
        //                        {
        //                            valuelist = values[i] as List<object>;
        //                            sb.Append("{");
        //                            for (int j = 0; j < valuelist.Count; j++)
        //                            {
        //                                if (valuelist[j] != null && !string.IsNullOrEmpty(valuelist[j].ToString()))
        //                                    sb.AppendFormat("\"{0}\":\"{1}\",", keys[j], valuelist[j]);
        //                            }
        //                            sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
        //                            sb.Append("},");
        //                        }
        //                        sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
        //                        sb.Append("]");
        //                        r.R = JsonConvert.DeserializeObject<List<T>>(sb.ToString());
        //                        response.Close();
        //                        return r;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Base.WriteError("DeserializeRecord convert ex:", ex.Message, ";URL: ", request.RequestUri.PathAndQuery, ";res：", res);
        //                        response.Close();
        //                    }
        //                }
        //            }
        //        } 
        //    }
        //    catch (Exception ex2)
        //    {
        //        Base.WriteError("PostRecordServer ex:", ex2.Message, "url:", requestUrl + "?" + param);
        //    }
        //    return default(PostResult<List<T>>);
        //}

        public static PostResult<List<T>> PostRecordServer<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(requestUrl))
                {
                    Base.WriteError("PostRecordServer requestUrl is empty.");
                    return null;
                } 
                WebRequest request = requestServerUrl(requestUrl, param);
                if (request != null)
                    return DeserializeRecord<T>(request);
            }
            catch (Exception ex2)
            {
                Base.WriteError("PostRecordServer ex:", ex2.Message, "url:", requestUrl + "?" + param);
            }
            return default(PostResult<List<T>>);
        }

        public static T PostRedpack<T>(string requestUrl,string key, string method, params object[] args)
        {
            var param = makeArgs(key, args);
            if (string.IsNullOrEmpty(param))
            {
                Base.WriteError("PostRedpack<T> param is empty.");
                return default(T);
            }
            string url = string.Format("{0}?m={1}&args={2}", requestUrl, method, param);            
            Base.WriteDebug("requestUrl:", url);
            var request = WebRequest.Create(url) as HttpWebRequest;
            return Deserialize<T>(request); 
        }
        public static string makeArgs(string key, params object[] args)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    Base.WriteError("makeArgs'key is null");
                    return null;
                }
                string argsStr = JsonConvert.SerializeObject(args);
                string param_key = (key + argsStr).ToLower();
                string argsOutStr = "";
                for (int i = 0; i < argsStr.Length; i++)
                {
                    if (Regex.IsMatch(argsStr[i].ToString(), @"[\u4e00-\u9fa5]"))
                        argsOutStr += "\\u" + ((int)argsStr[i]).ToString("x");
                    else
                        argsOutStr += argsStr[i].ToString();
                }
                param_key = (key + argsOutStr).ToLower();
                string sign = MD5.Encrypt(param_key, Encoding.UTF8);
                List<object> argslist = new List<object>();
                argslist.AddRange(args);
                argslist.Add(sign);
                args = argslist.ToArray();
                return JsonConvert.SerializeObject(args);
            }
            catch (Exception ex)
            {
                Base.WriteError("PostServer makeArgs ex:", ex.Message, ",key:", key, ",args,", args);
            }
            return "";
        }
        static HttpWebRequest requestServerUrl(string requestUrl, string param)
        {
            try
            {
                Base.WriteDebug("requestServerUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                request.Timeout = 1200000;
                request.ReadWriteTimeout = 1200000;
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                    return request;
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("PostServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return null;
        } 
        static T Deserialize<T>(HttpWebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var res = reader.ReadToEnd();
                    Base.WriteDebug("sever'res is ", res);
                    try
                    {
                        var t = JsonConvert.DeserializeObject<T>(res);
                        if (t == null)
                            Base.WriteError("Deserialize<T> t is null.");
                        response.Close();
                        return t;
                    }
                    catch (Exception ex2)
                    {
                        Base.WriteError("PostServer Deserialize Convert ex：", ex2.Message, ";URL: ", request.RequestUri.PathAndQuery, ";res：", res);
                        response.Close();
                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("Deserialize ex：", ex.Message);
                return default(T);
            }
        }
        static PostResult<List<T>> DeserializeRecord<T>(WebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var res = reader.ReadToEnd();
                    Base.WriteDebug("recordserver'res is ", res);
                    try
                    {
                        var jarry = JsonConvert.DeserializeObject(res) as JArray;
                        if (jarry == null)
                        {
                            Base.WriteError("DeserializeRecord JsonConvert.DeserializeObject(res) is null");
                            return default(PostResult<List<T>>);
                        }
                        List<object> list = new List<object>();
                        foreach (JToken item in jarry)
                        {
                            JTokenResolver.Resolve(list, item);
                        }
                        if (list == null || list.Count < 1)
                        {
                            Base.WriteError("DeserializeRecord JTokenResolver.Resolve'list is null.");
                            return default(PostResult<List<T>>);
                        }
                        if (list[0] == null || string.IsNullOrEmpty(list[0].ToString()))
                        {
                            Base.WriteError("DeserializeRecord res not has data.");//返回约定格式有误
                            return default(PostResult<List<T>>);
                        }
                        if (int.Parse(list[0].ToString()) < 1)
                        {
                            //Base.WriteError("DeserializeRecord res has'data is err.");//无数据
                            return default(PostResult<List<T>>);
                        }
                        if (list.Count != 3)
                        {
                            // Base.WriteError("DeserializeRecord res'num is bad count.");//无数据
                            return default(PostResult<List<T>>);
                        }
                        PostResult<List<T>> r = new PostResult<List<T>>() { Code = int.Parse(list[0].ToString()) };
                        var keys = list[1] as List<object>;
                        var values = list[2] as List<object>;
                        List<object> valuelist = null;
                        var sb = new StringBuilder();
                        sb.Append("[");
                        for (int i = 0; i < values.Count; i++)
                        {
                            valuelist = values[i] as List<object>;
                            sb.Append("{");
                            for (int j = 0; j < valuelist.Count; j++)
                            {
                                if (valuelist[j] != null && !string.IsNullOrEmpty(valuelist[j].ToString()))
                                    sb.AppendFormat("\"{0}\":\"{1}\",", keys[j], valuelist[j]);
                            }
                            sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                            sb.Append("},");
                        }
                        sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                        sb.Append("]");
                        r.R = JsonConvert.DeserializeObject<List<T>>(sb.ToString());
                        response.Close();
                        return r;
                    }
                    catch (Exception ex)
                    {
                        Base.WriteError("DeserializeRecord convert ex:", ex.Message, ";URL: ", request.RequestUri.PathAndQuery, ";res：", res);
                        response.Close();
                    }
                }
            }
            catch (Exception ex2)
            {
                Base.WriteError("DeserializeRecord ex222:", ex2.Message, ";URL: ", request.RequestUri.PathAndQuery);
            }
            return default(PostResult<List<T>>);
        }
         
    }
}
