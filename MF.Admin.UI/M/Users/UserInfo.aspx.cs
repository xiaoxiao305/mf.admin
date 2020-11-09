using System;
using MF.Admin.BLL;
using MF.Data;

namespace MF.Admin.UI.M.Users
{
    public partial class UserInfo : BasePage
    {
        protected MF.Data.Users user = new MF.Data.Users();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            { 
                int dbsource = 0;
                if (Request.QueryString.Count != 2 || Request.QueryString.GetKey(0) != "db")
                {
                    ShowMessage("参数错误");
                    Base.WriteError(ClientIP, " getuinfo(acc:", Request["acc"], ") err.args is err.", Request.Url.PathAndQuery);
                    return;
                }
                int.TryParse(Request["db"], out dbsource);
                if (string.IsNullOrEmpty(Request["acc"]))
                {
                    ShowMessage("获取用户信息失败,参数错误");
                    Base.WriteError(ClientIP, " getuinfo(acc:", Request["acc"], ") err.args is err.acc is ", Request["acc"], Request.Url.PathAndQuery);
                    return;
                }
                DBSource db_source = (DBSource)dbsource;
                if (db_source == DBSource.DBNOW)//显示操作区域
                    uinfodetailopreate.Attributes["Class"] = "show";
                var res = UserBLL.GetUserInfo(db_source, System.Web.HttpUtility.UrlEncode(Request["acc"]));
                if (res.Code == 1)
                    user = res.R;
                else
                {
                    ShowMessage("获取用户信息失败:" + res.Message);
                    Base.WriteError("getuinfo(", user.Account, ") err'msg is ", res.Message, Request.Url.PathAndQuery);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("获取用户信息异常:" + ex.Message);
                Base.WriteError("getuinfo ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }
        protected string ConvertLock(int _lock)
        {
            switch (_lock)
            {
                case 0:
                    return "未锁定";
                case 1:
                    return "已锁定";
                case 2:
                    return "锁定设备登录";
                default:
                    return _lock.ToString();
            }
        }
        protected string ConvertGuest(int guest)
        {
            switch (guest)
            {
                case 0:
                    return "注册用户";
                case 1:
                    return "游客";
                case 2:
                    return "游客转正";
                default:
                    return guest.ToString();
            }
        }
        protected string ConvertPlatformId(int platform)
        {
            switch (platform)
            {
                case 0:
                    return "";//官网
                case 1:
                    return "微信";
                //case 2:
                //    return "";
                default:
                    return platform.ToString();
            }
        }
    }
}
