using MF.Admin.BLL;
using System;

namespace MF.Admin.UI.M.Currency
{
    public partial class StrongBox : BasePage
    {
        protected string account = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            {
                if (Request.QueryString.Count != 1 || Request.QueryString.GetKey(0) != "account")
                {
                    ShowMessage("参数错误");
                    Base.WriteError(ClientIP, " getuserStrongBox(account:", Request["account"], ") err.", Request.Url.PathAndQuery);
                    return;
                }
                if (Request["account"] == null || string.IsNullOrEmpty(Request["account"].ToString()))
                {
                    ShowMessage("获取用户二级密码信息失败,参数错误");
                    Base.WriteError(ClientIP, " getuserStrongBox(account:", Request["account"], ") err.args is err.", Request.Url.PathAndQuery);
                    return;
                }
                account = System.Web.HttpUtility.UrlEncode(Request["account"]);
            }
            catch (Exception ex)
            {
                ShowMessage("查询二级密码记录获取参数异常:" + ex.Message);
                Base.WriteError("getuserStrongBox ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }
    }
}
