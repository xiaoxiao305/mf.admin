using MF.Admin.BLL;
using MF.Data;
using System;

namespace MF.CPS.UI
{
    public class BasePage : System.Web.UI.Page
    {
        #region 客户端IP地址
        protected string ClientIP
        {
            get
            {
                return Base.ClientIP;
            }
        }
        #endregion

        #region 当前登录用户
        public new Administrator User
        {
            set { }
            get
            {
                if (Session["administrator"] != null)
                    return (Administrator)Session["administrator"];
                else
                    return null;
            }
        }
        #endregion

        #region 当前登录用户渠道码
        public string UserChannel
        {
            set { }
            get
            {

                if (Session["administrator"] != null)
                {
                    Administrator admin = (Administrator)Session["administrator"];
                    CPSUsersAdmin model = CPSUsersBLL.GetCPSUserModel(admin.Account);
                    if (model == null)
                        return "-2";
                    else
                        return model.channel;
                }
                else
                    return "-2";
            }
        }
        #endregion

        #region 是否登录
        protected bool IsLogin
        {
            get
            {
                return AdminBLL.CheckIsLogin();
            }
        }
        #endregion

        #region Controls
        /// <summary>
        /// 仅弹出消息
        /// </summary>
        /// <param name="strMessage">消息内容</param>
        protected void ShowMessage(string strMessage)
        {
            string strScript = "";
            strScript += "alert(\"" + strMessage + "\");";
            RegisterScript(strScript);
        }
        /// <summary>
        /// 弹出消息,并转到指定页面
        /// </summary>
        /// <param name="strMessage">消息内容</param>
        /// <param name="strURL">页面路径</param>
        protected void ShowMessage(string strMessage, string strURL)
        {
            string strScript = "";
            strScript += "alert(\"" + strMessage + "\");";
            strScript += "location.href=\"" + strURL + "\";";
            RegisterScript(strScript);
        }
        /// <summary>
        /// 注册客户端脚本块 
        /// </summary>
        /// <param name="script">客户端脚本( 如:window.onload=function(){alert('正在加载页面...');} )</param>
        private void RegisterScript(string script)
        {
            Random r = new Random();
            string key = "script" + r.NextDouble().ToString();
            while (ClientScript.IsStartupScriptRegistered(Page.GetType(), key))
                key = "script" + r.NextDouble().ToString();
            ClientScript.RegisterStartupScript(Page.GetType(),
               key, string.Format(
                @"<script language='javascript' type='text/javascript' >
                 {0}
              </script>", script));
        }
        #endregion

        public DateTime ConvertToDate(string datepart, int timeSpan)
        {
            var date = DateTime.Parse("2012-10-01 00:00:00");
            if (datepart.ToLower() == "d" || datepart.ToLower() == "dd")
            {
                return date.AddDays((double)timeSpan);
            }
            else if (datepart.ToLower() == "s" || datepart.ToLower() == "ss")
            {
                return date.AddSeconds((double)timeSpan);
            }
            return date;
        }
        public string ConvertDevice(int deviceID)
        {
            switch (deviceID)
            {
                case 2:
                    return "pc";
                case 3:
                    return "ipad";
                case 5:
                    return "iOS";
                case 6:
                    return "Android";
                default:
                    return deviceID.ToString();
            }
        }
    }
}