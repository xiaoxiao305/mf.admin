 
using MF.Common.Security;
using MF.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace MF.ConsoleApp
{
   
    public class PostHelper
    { 
        public static string WebServerKey = ConfigurationManager.AppSettings["WEB_SECURITY"];
        public static string ClubServer = ConfigurationManager.AppSettings["ClubsURI"];




        public static T Post<T>(string module, string func, Dictionary<string, object> args)
        {
            return Post<T>(ClubServer, module, func, args);
        }

        public static T Post<T>(string _server, string module, string func, Dictionary<string, object> args)
        {
            var protocol = new Protocol { module = module, func = func, args = args };
            var body = Post(_server,JsonConvert.SerializeObject(protocol));

            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            if (dic != null && dic.ContainsKey("ret") && dic["ret"].ToString() == "0")
                return JsonConvert.DeserializeObject<RequestResult<T>>(body).msg;
            Log.WriteError("获取数据返回失败,", JsonConvert.SerializeObject(protocol), ",服务器返回:", body);
            return default(T);
        }
        public static T Post<T>(string module, string func, object[] args)
        {
            return Post<T>(ClubServer, module, func, args);
        }
        public static string Post(string url, string data, int timeout = 6)
        {
            return Post(url, data, Encoding.UTF8, timeout);
        }
        public static string Post(string url, string data, Encoding coding, int timeout = 6)
        {
            Log.WriteDebug(url, data);
            var request = (System.Net.HttpWebRequest)HttpWebRequest.Create(url);
            return Post(request, data, timeout, coding);
        }
        public static string Post(System.Net.HttpWebRequest request, string data, int timeout, Encoding coding)
        {
            try
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                request.Timeout = timeout * 1000;
                request.KeepAlive = true;
                var sw = new StreamWriter(request.GetRequestStream());
                sw.Write(data);
                sw.Close();
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), coding))
                {
                    var res = reader.ReadToEnd();
                    Log.WriteDebug("POST res:", res);
                    return res;
                }
            }
            catch (Exception e)
            {
                Log.WriteError("向[", request.RequestUri.OriginalString, "]请求数据时出错:", e.Message, "\nPost Data:", data, "\n堆栈信息:", e.StackTrace);
            }
            return "";
        }

        public static T Post<T>(string _server, string module, string func, object[] args)
        {
            
            var protocol = new Protocol2 { module = module, func = func, args = args };
            var body = Post(_server, JsonConvert.SerializeObject(protocol));

            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            if (dic.ContainsKey("ret") && dic["ret"].ToString() == "0")
                return JsonConvert.DeserializeObject<RequestResult<T>>(body).msg;
            Log.WriteError("获取数据返回失败,", JsonConvert.SerializeObject(protocol), ",服务器返回:", body);
            return default(T);
        }
        public static T PostClubServer<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(requestUrl))
                {
                    Log.WriteError("requestUrl is empty.");
                    return default(T);
                }
                Log.WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var res = reader.ReadToEnd();
                    Log.WriteDebug("sever'res is ", res);
                    try
                    {
                        var t = JsonConvert.DeserializeObject<T>(res);
                        if (t == null)
                            Log.WriteError("Deserialize<T> t is null.");
                        response.Close();
                        return t;
                    }
                    catch (Exception ex2)
                    {
                        Log.WriteError("PostServer Deserialize Convert ex：", ex2.Message, ";URL: ", request.RequestUri.PathAndQuery, " requestUrl is ", requestUrl, ".param is ", param, ";res：", res);
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("PostServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        public static Result<List<T>> PostRecordServer<T>(string requestUrl, string param)
        {
            PostResult<List<T>> res = Http.PostRecordServer<T>(requestUrl, param);
            if (res == null)
                return null;
            return new Result<List<T>>() { Code = res.Code, Ex = res.Ex, Message = res.Message, R = res.R };
        }
        public static T Post<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(param))
                {
                    Console.WriteLine("Post<T> param is empty.");
                    return default(T);
                }
                return requestServerUrl<T>(requestUrl, param);
            }
            catch (Exception ex2)
            {
                Console.WriteLine("Post ex：" + ex2.Message + ",requestUrl:" + requestUrl + ",param:" + param);
            }
            return default(T);
        }
        static T requestServerUrl<T>(string requestUrl, string param)
        {
            try
            {
                Log.WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.ContentLength = buffer.Length;
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                    var response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var res = reader.ReadToEnd();
                        Log.WriteDebug("sever'res is ", res);
                        try
                        {
                            var t = JsonConvert.DeserializeObject<T>(res);
                            if (t == null)
                                Console.WriteLine("Deserialize<T> t is null.");
                            response.Close();
                            return t;
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine("PostServer Deserialize Convert ex：" + ex2.Message + ";URL: " +
                                request.RequestUri.PathAndQuery + " requestUrl is " + requestUrl +
                                ".param is " + param, ";res：" + res);
                            response.Close();
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PostServer requestUrl ex:" + ex.Message + ",requestUrl:" + requestUrl +
                    ",param:" + param);
            }
            return default(T);
        }
    }
    public class Protocol2
    {
        public string module;
        public string func;
        public object[] args;

    }
    public class RequestResult<T>
    {
        public int ret { get; set; }
        public T msg { get; set; }
    }
    public class Protocol
    {
        public string module;
        public string func;
        public Dictionary<string, object> args;

    }
}
