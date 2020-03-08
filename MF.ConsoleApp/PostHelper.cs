using MF.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
  public class PostHelper
    {
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
                Base.WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
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
                        Base.WriteDebug("sever'res is ", res);
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
}
