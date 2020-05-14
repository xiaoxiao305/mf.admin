using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MF.KF.UI
{
    /// <summary>
    /// HandlerDownFile 的摘要说明
    /// </summary>
    public class HandlerDownFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse Response = context.Response;
            string gameId= context.Request["gameId"];
            string chargeId = context.Request["chargeId"];
            string model = context.Request["model"];
            string type = context.Request["type"];
            try
            {
                Admin.BLL.Base.WriteLog("HandlerDownFile ProcessRequest:",gameId,chargeId,model,type);
                string recUri = ConfigurationManager.AppSettings["ShiChuiURI"];
                if (string.IsNullOrEmpty(model) || int.Parse(model) < 1 || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(recUri))
                {
                    Admin.BLL.Base.WriteError("downlog model or type is empty.model:",model,"type:",type, " recUri:", recUri);
                    return; 
                }
                int m = int.Parse(model);
                if (m == 1)//确认实锤
                {
                    if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(chargeId)) return;
                    string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    string filePath = string.Format("{0}record/{1}/{2}/SHICHUI/{3}.txt", recUri, gameId, chargeId,type);
                    string fileName = string.Format("{0}_{1}_{2}_{3}.txt", time, gameId, chargeId,type);
                    //Admin.BLL.Base.WriteLog("DownConfirmBlackUserLog SHICHUI filePath:", filePath, " fileName:", fileName);
                    DownLoadHttp(filePath, fileName, context);
                }else if (m == 2)//全部
                {
                    if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(chargeId)) return;
                    string downGameLogDays = ConfigurationManager.AppSettings["DownGameLogDays"];//需要下载游戏日志的天数
                    if (string.IsNullOrEmpty(downGameLogDays)) { Admin.BLL.Base.WriteError("DownGameLogDays config is empty"); return; }
                    int days = int.Parse(downGameLogDays);
                    if (days < 1) return;
                    List<byte> buffer = new List<byte>();
                    for (int i = 0; i < days; i++)
                    {
                        string day= DateTime.Now.AddDays(-1*i).ToString("yyyy-MM-dd");
                        string filePath = string.Format("{0}record/{1}/{2}/ALL/{3}_{4}.txt", recUri, gameId, chargeId, type,day);
                        Admin.BLL.Base.WriteLog("DownConfirmBlackUserLog ALL filePath:", filePath);
                        byte[] bytes = GetBytesFromService(filePath);
                        if (bytes == null || bytes.Length < 1) continue;
                        Admin.BLL.Base.WriteLog("DownConfirmBlackUserLog add start.:",filePath, bytes.Length.ToString());
                        for (int j = 0; j < bytes.Length; j++)
                        {
                            buffer.Add(bytes[j]);
                        }
                    }
                    Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    string fileName = string.Format("{0}_{1}_{2}_{3}.txt", time, gameId, chargeId, type);
                    Admin.BLL.Base.WriteLog("DownConfirmBlackUserLog fileName:", fileName, " time:", time);

                    Response.AddHeader("Content-Disposition", "attachment;  filename="
                        + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                    Response.BinaryWrite(buffer.ToArray());
                    Response.Flush();
                    Response.End();
                    //DownLoadHttp(filePath, fileName, context);
                } 
            }
            catch (IOException e)
            {
                Admin.BLL.Base.WriteError("DownConfirmBlackUserLog IOException ex:", e.Message);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Admin.BLL.Base.WriteError("DownConfirmBlackUserLog ex:", ex.Message, " gameId:", gameId ," chargeId：", chargeId," model:", model,"type:", type);
            }
        }
        public static byte[] GetBytesFromService(string filePath)
        {
            try
            {
                //HttpResponse Response = context.Response;
                HttpWebRequest request = HttpWebRequest.Create(filePath) as HttpWebRequest;
                request.Method = "GET";
                request.ProtocolVersion = new Version(1, 1);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return (new byte[0]);//找不到则直接返回null
                           // 转换为byte类型
                System.IO.Stream stream = response.GetResponseStream();
                byte[] bytes= ReadFully(stream);
                //Response.ContentType = "application/octet-stream";
                ////通知浏览器下载文件而不是打开
                //Response.AddHeader("Content-Disposition", "attachment;  filename="
                //    + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                //Response.BinaryWrite(bytes);
                //Response.Flush();
                //Response.End();
                return bytes;

            }catch(System.Threading.ThreadAbortException e) { return (new byte[0]); }
            catch(Exception ex)
            {
                Admin.BLL.Base.WriteError("GetBytesFromService ex:", ex.Message, "filePath:", filePath);
                return (new byte[0]);
            }
        }
        public static void DownLoadHttp(string filePath, string fileName, HttpContext context)
        {
            try
            {
                HttpResponse Response = context.Response;
                HttpWebRequest request = HttpWebRequest.Create(filePath) as HttpWebRequest;
                request.Method = "GET";
                request.ProtocolVersion = new Version(1, 1);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return;//找不到则直接返回null
                           // 转换为byte类型
                System.IO.Stream stream = response.GetResponseStream();
                byte[] bytes = ReadFully(stream);
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                Response.AddHeader("Content-Disposition", "attachment;  filename="
                    + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

            }
            catch (System.Threading.ThreadAbortException e) { }
            catch (Exception ex)
            {
                Admin.BLL.Base.WriteError("DownLoadHttp ex:", ex.Message, "filePath:", filePath, " fileName:", fileName);
            }
        }
        public static Array ReadFully2(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                byte[] bytes = ms.ToArray();
                Array para = Array.CreateInstance(typeof(Byte), bytes.Length);
                return para;
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}