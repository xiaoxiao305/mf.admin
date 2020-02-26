using MF.Admin.BLL;
using System;

namespace MF.Admin.UI.M.Currency
{

    public partial class ExchangeRecord : BasePage
    {
        protected string account = "";
        protected string UIDFirst = "";
        protected int type = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            {
                if (Request.QueryString.Count != 3 || Request.QueryString.GetKey(0) != "account")
                {
                    ShowMessage("参数错误");
                    Base.WriteError(ClientIP, " getuserqmallrecord(account:", Request["account"], ") err.", Request.Url.PathAndQuery);
                    return;
                }
                if (Request["account"] == null || string.IsNullOrEmpty(Request["account"].ToString()))
                {
                    ShowMessage("获取用户兑换奖品记录失败,参数错误");
                    Base.WriteError(ClientIP, " getuserqmallrecord(account:", Request["account"], ") err.args is err.", Request.Url.PathAndQuery);
                    return;
                }
                account = Request["account"];
                if (Request["uid"] == null || string.IsNullOrEmpty(Request["uid"]))
                    UIDFirst = "1";
                else
                {
                    UIDFirst = Request["uid"].ToString().Substring(0, 1);
                    if (UIDFirst == "0")
                        UIDFirst = "1";
                }
                if (Request["type"] != null && !string.IsNullOrEmpty(Request["type"]))
                    type = int.Parse(Request["type"].ToString());
            }
            catch (Exception ex)
            {
                ShowMessage("查询其他奖品记录获取参数异常:" + ex.Message);
                Base.WriteError("getuserqmallrecord ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }
    }
}