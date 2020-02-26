using MF.Admin.BLL;
using MF.Common.Json;
using MF.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.Admin.UI.M.Guild
{
    public partial class SetSuggestGuild : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LoadFile(object sender, EventArgs e)
        {
            try
            {
                if (!uploadFile.HasFile)
                {
                    lblerr.InnerText = "请选择需要上传的excel文档";
                    return;
                }
                if (uploadFile.PostedFile.ContentLength >= 10485760)
                {
                    lblerr.InnerText = "文件过大";
                    return;
                }
                string extensionname = Path.GetExtension(uploadFile.FileName);
                if (!extensionname.Trim().ToLower().Equals(".xls"))
                {
                    lblerr.InnerText = "该服务器excel版本只支持.xls版本";
                    return;
                }
                string withoutextensionname = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                string filename = DateTime.Now.ToString("yyyMMddHHmm") + withoutextensionname + extensionname;
                string path = Server.MapPath("~/m/guild/setversion/") + filename;
                try
                {
                    uploadFile.PostedFile.SaveAs(path);//在服务器的路径，上传excel的路径
                }
                catch (Exception ex)
                {
                    lblerr.InnerText = "上传excel异常：" + ex.Message;
                    Base.WriteError("上传excel异常：", ex.Message);
                    return;
                }
                SetExcelData(path);
            }
            catch (Exception ex)
            {
                lblerr.InnerText = "设置推荐俱乐部异常：" + ex.Message;
                Base.WriteError("设置推荐俱乐部异常：", ex.Message);
            }
        }

        private void SetExcelData(string path)
        {
            try
            {
                DataSet ds = Base.ReadExcel(path);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    lblerr.InnerText = "文件数据为空或读取数据异常";
                    return;
                }
                DataRow dr = null;
                List<object> list = new List<object>();
                object[] tempobj = null;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = ds.Tables[0].Rows[i];
                    tempobj = new object[dr.ItemArray.Length];
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        tempobj[j] = int.Parse(dr[j].ToString());
                    }
                    list.Add(tempobj);
                }
                int res = GuildBLL.SetSuggestGuild(list, path);
                if (res > 0)
                {
                    lblerr.InnerText = "设置推荐俱乐部成功";
                    return;
                }
                lblerr.InnerText = "设置推荐俱乐部失败。code：" + res;
            }
            catch (Exception ex)
            {
                BLL.Base.WriteError("ReadExcel ex:", ex.Message);
            }
        }
    }
}