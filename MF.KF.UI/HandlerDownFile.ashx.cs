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
                    string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    string filePath = string.Format("{0}record/{1}/{2}/ALL/{3}.txt", recUri, gameId, chargeId, type);
                    string fileName = string.Format("{0}_{1}_{2}_{3}.txt", time, gameId, chargeId, type);
                    //Admin.BLL.Base.WriteLog("DownConfirmBlackUserLog ALL filePath:", filePath, " fileName:", fileName);
                    DownLoadHttp(filePath, fileName, context);
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
        public static void DownLoadHttp(string filePath, string fileName,HttpContext context)
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

            }catch(System.Threading.ThreadAbortException e) { }
            catch(Exception ex)
            {
                Admin.BLL.Base.WriteError("DownLoadHttp ex:", ex.Message, "filePath:", filePath, " fileName:", fileName);
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