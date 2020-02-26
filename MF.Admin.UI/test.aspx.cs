using MF.Admin.BLL;
using MF.Common.Security;
using MF.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace MF.Admin.UI
{
    public class UserTest
    {
        public string account { get; set; }
        public int id { get; set; }
        public string identity { get; set; }
    }
    public class JsonData<T>
    {
        public string token { get; set; }
        public T list { get; set; }
    }
    public class UTest
    {
        public int Id { get; set; }
        public string Account { get; set; }
    }
    public class GameInfoExcel
    {
        public int gameid { get; set; }
        public int matchid { get; set; }
        public string gamename { get; set; }
        public string matchname { get; set; }
    }
    public partial class test : BasePage
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
            GameServerList();
        }
        private void FlushServer(string[] args)
        {
            //string[] args = new string[] { "game_mgr_pro_51_0@172.17.0.51" };
            OprGameServer ogs = SettingBLL.FlushGameServer(args);
            if (ogs == null)
            {
                Response.Write("FlushServer'return is null.");
                return;
            }
            if (string.IsNullOrEmpty(ogs.msg))
            {
                Response.Write("FlushServer'ogs.msg is empty.");
                return;
            }
            else if (ogs.ret != 0)
            {
                Response.Write("FlushServer.ret:" + ogs.ret.ToString());
                return;
            }
            else if (!ogs.msg.Trim().ToLower().Equals("success"))
            {
                Response.Write("FlushServer.msg:" + ogs.msg);
                return;
            }
            Response.Write(ogs.msg + "<br/>");
        }
        private void GameServerList()
        {
            List<GameServerList> gs = SettingBLL.GameServerList();
            if (gs == null || gs.Count < 1)
            {
                Response.Write("GameServerList is null.");
                return;
            }
            foreach (GameServerList gsl in gs)
            {
                Response.Write(gsl.gname + "<br/>");
            }
            FlushServer(new string[] { gs[0].gname });
        }
        private void GameExcle()
        {
            if (Request["hd"] != null)
            {
                ChkHDInfo(Request["hd"].ToString());
                return;
            }
            string path = "E:\\gameinfo.xls";
            Result<List<GameInfoExcel>> res = ReadExcel<GameInfoExcel>(path);
            if (res == null)
            {
                Response.Write("res is null");
                return;
            }
            if (res.R == null)
            {
                Response.Write("res.r is null");
                return;
            }
            string str = "";
            foreach (GameInfoExcel excel in res.R)
            {
                str += string.Format("gameid:{0},matchid:{1},gamename:{2},matchname:{3}\r\n", excel.gameid, excel.matchid, excel.gamename, excel.matchname);
            }
            Response.Write(str);
        }
        protected void LoadFile(object sender, EventArgs e)
        {
            try
            {
                if (!uploadFile.HasFile)
                {
                    Response.Write("uploadFile.HasFile is flase.");
                    return;
                }
                if (uploadFile.PostedFile.ContentLength >= 10485760)
                {
                    Response.Write("file is too strong.");
                    return;
                }
                string lattername = Path.GetExtension(uploadFile.FileName);
                if (!lattername.Trim().ToLower().Equals(".xls"))
                {
                    Response.Write("the server is only support .xls file");
                    return;
                }
                try
                {
                    uploadFile.PostedFile.SaveAs(Server.MapPath("~/m/guild/setversion/") + uploadFile.FileName);//在服务器的路径，上传excel的路径
                    Response.Write("<br/>上传成功");
                }
                catch (Exception ex)
                {
                    Response.Write("<br/>上传excel异常：" + ex.Message);
                    return;
                }
                string excelPath = Server.MapPath("~/m/guild/setversion/") + uploadFile.FileName;
                Response.Write("<br/>excelPath:" + excelPath);
                //DataSet ds = ReadExcel(excelPath);
                //if (ds == null || ds.Tables.Count < 1 || ds.Tables[0] == null
                //    || ds.Tables[0].Rows.Count < 1)
                //    Response.Write("<br/>ds is null");
                //else
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        Response.Write("<br/>" + ds.Tables[0].Rows[i][0].ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void Click(object sender, EventArgs e)
        {

        }
        private void ChkHDInfo(string md5hd)
        {
            string userhd = "XIAOXIAOWin32NTMicrosoft Windows NT 6.1.7601 Service Pack 14000BE769 18610555 6C979E44 C8610853 D6FDFBEF FC37ADFC BFEBFBFF000506E3 GenuineIntel Intel(R) Core(TM) i3-6100 CPU @ 3.70GHz 3700 9905622-058.A00G     {78A75790-28BE-4D9F-886B-02BDC7621A28} 1C:1B:0D:85:5B:AD";
            CheckMD5(md5hd, userhd);
        }
        public static List<string> Create(string iukey)
        {
            List<string> list = new List<string>();
            DateTime start = DateTime.Parse("1970-01-01 00:00:00");
            DateTime end = DateTime.Now.AddHours(-8);
            TimeSpan span = end - start;
            int totalsecond = Convert.ToInt32(span.TotalSeconds);//自1970-01-01 00:00:00到当前时间的总秒数
            int msecd = totalsecond % 30;
            int[] seconds = { totalsecond - msecd, totalsecond - msecd - 30, totalsecond - msecd - 60, totalsecond - msecd - 90, totalsecond - msecd + 30, totalsecond - msecd + 60, totalsecond - msecd + 90 };
            foreach (var s in seconds)
                list.Add(MD5.Encrypt(iukey + s).ToLower());

            return list;
        }
        /// <summary>
        /// 校验传入的MD5码是否正确
        /// </summary>
        /// <param name="md5Str">已对key进行加密后MD5密码串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckMD5(string md5Str, string key)
        {
            if (string.IsNullOrEmpty(md5Str))
                return false;
            var tokens = Create(key);
            foreach (var k in tokens)
            {
                if (k.ToUpper() == md5Str.ToUpper())
                    return true;
            }
            return false;
        }
        public Result<List<T>> ReadExcel<T>(string path)
        {
            try
            {
                Result<List<T>> res = new Result<List<T>>();
                try
                {
                    string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;'";//.xls版本
                    DataSet ds = new DataSet();
                    OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
                    oada.Fill(ds);
                    if (ds == null || ds.Tables[0].Rows.Count < 1)
                    {
                        res.Message = "文件数据为空或读取数据异常";
                        return res;
                    }
                    List<string> keys = new List<string>();

                    var sb = new StringBuilder();
                    sb.Append("[");
                    string colname = "";
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sb.Append("{");
                        for (int col = 0; col < ds.Tables[0].Columns.Count; col++)
                        {
                            colname = ds.Tables[0].Columns[col].ColumnName;
                            sb.AppendFormat("\"{0}\":\"{1}\",", colname, row[colname]);
                        }
                        sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                        sb.Append("},");
                    }
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                    sb.Append("]");
                    res.R = JsonConvert.DeserializeObject<List<T>>(sb.ToString());
                }
                catch (Exception ex)
                {
                    Base.WriteError("ReadExcel ex:", ex.Message);
                }
                return res;
            }
            catch (Exception ex)
            {
                Response.Write("ex:" + ex.Message);
                return null;
            }

        }
    }
}