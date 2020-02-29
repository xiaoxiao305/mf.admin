using MF.Admin.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.Admin.UI.M.game
{
    public partial class gamerec : BasePage
    {
        protected string gameDic = "{}";
        private string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<Dictionary<string, string>> res = GameBLL.GetGameListForBlack();
                if (res == null || res.Count < 1) return;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (Dictionary<string, string> item in res)
                {
                    if (item.ContainsKey("id") && item.ContainsKey("name"))
                        dic.Add(item["id"], item["name"]);
                }
                gameDic = JsonConvert.SerializeObject(dic);
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, "/m/game/gameblacklist.aspx", ex.Message);
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
        protected void DownYesterday(object sender, EventArgs e)
        {
            try
            {
                if (hidrecUrl.Value == "")
                {
                    ShowMessage("请选择游戏");
                    return;
                }

                string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                url = string.Format("{0}/{1}/REC_{2}.txt", System.Configuration.ConfigurationManager.AppSettings["RecURI"], hidrecUrl.Value, time);
                string fname = string.Format("{0}_REC_{1}.txt", hidrecUrl.Value, time);
                CopyFileByUrl(url, fname);
            }
            catch (Exception ex)
            {
                ShowMessage("下载昨天录像异常：" + ex.Message + " url:" + url);
            }
        }
        protected void DownToday(object sender, EventArgs e)
        {
            try
            {
                if (ddlGame.Value == "" || hidrecUrl.Value == "")
                {
                    ShowMessage("请选择游戏");
                    return;
                }
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                url = string.Format("{0}/{1}/REC.txt", System.Configuration.ConfigurationManager.AppSettings["RecURI"], hidrecUrl.Value);
                //string fname = string.Format("{0}_REC_{1}.txt", hidrecUrl.Value,time);
                string fname = string.Format("{0}_REC.txt", hidrecUrl.Value);
                CopyFileByUrl(url, fname);
            }
            catch (Exception ex)
            {
                ShowMessage("下载今天录像异常：" + ex.Message + " url:" + url);
            }
        }
        protected void DownWarring(object sender, EventArgs e)
        {
            try
            {
                if (ddlGame.Value == "" || hidrecUrl.Value == "")
                {
                    ShowMessage("请选择游戏");
                    return;
                }
                url = string.Format("{0}/{1}/warning.txt", System.Configuration.ConfigurationManager.AppSettings["RecURI"], hidrecUrl.Value);
                string fname = string.Format("{0}_warring.txt", hidrecUrl.Value);
                CopyFileByUrl(url, fname);
            }
            catch (Exception ex)
            {
                ShowMessage("下载日志异常：" + ex.Message + " url:" + url);
            }
        }
        public void CopyFileByUrl(string url, string fname)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(fname)) return;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
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
                + HttpUtility.UrlEncode(fname, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
    }
}