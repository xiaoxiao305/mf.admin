using MF.Admin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.KF.UI.M.SysManage
{
    public partial class sendbroadcast : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (isAdmin == 1) return;
            try
            {
                //504 系统管理 ---系统广播
                if (isExtraPowers == null || isExtraPowers.Length < 1 || Array.IndexOf(isExtraPowers, 504) == -1)
                {
                    Response.Redirect("/m/Users/UserList.aspx");
                    return;
                }
                foreach (var item in isExtraPowers)
                {
                    Base.WriteError("isExtraPowers:", item.ToString());
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }
    }
}