using MF.Admin.BLL;
using System;

namespace MF.Admin.UI.M.Currency
{
    public partial class RoomCard : BasePage
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
                    Base.WriteError(ClientIP, " getroomcard(account:", Request["account"], ") err.", Request.Url.PathAndQuery);
                    return;
                }
                if (Request["account"] == null || string.IsNullOrEmpty(Request["account"].ToString()))
                {
                    ShowMessage("获取用户房卡信息失败,参数错误");
                    Base.WriteError(ClientIP, " getroomcard(account:", Request["account"], ") err.args is err.", Request.Url.PathAndQuery);
                    return;
                }
                account = Request["account"];
            }
            catch (Exception ex)
            {
                ShowMessage("查询房卡记录获取参数异常:" + ex.Message);
                Base.WriteError("getroomcard ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }
    }
}
