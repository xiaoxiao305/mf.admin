using System;
using MF.Data;
using MF.Admin.BLL;
using MF.Common.Security;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MF.Admin.UI
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
        public bool CheckToken(string dynamicPwd)
        {
            if (User == null) return false;
            return Token.CheckToken(dynamicPwd, User.Token);
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

        #region 渠道码
        public string channellist
        {
            get
            {
                if (Session["channellist"] == null)
                    Session["channellist"] = JsonConvert.SerializeObject(ChannelList);
                if (Session["channellist"] == null)
                    return "{}";
                return Session["channellist"] as string;
            }
        }
        public Dictionary<string, string> ChannelList
        {
            get
            {
                List<CPSUsers> list = CPSUsersBLL.GetALLChannelList();
                if (list == null || list.Count < 1)
                    return null;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (CPSUsers model in list)
                {
                    if (!dic.ContainsKey(model.channel.ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(model.channel_name))
                            dic.Add(model.channel.ToUpper(), model.channel.ToUpper() + "_" + model.channel_name);
                        else
                            dic.Add(model.channel.ToUpper(), model.channel.ToUpper());
                    }
                }
                return dic;
            }
        }
        #endregion
        protected DateTime ConvertToDate(string datepart, long number)
        {
            DateTime date = DateTime.Parse("2012-10-01");

            switch (datepart)
            {
                case "s":
                   return date.AddSeconds(number);
                case "m":
                    return date.AddMinutes(number);
                case "h":
                    return date.AddHours(number);
                case "d":
                    return date.AddDays(number); 
            }
            return date;
        }



        public string blackGameList
        {
            get
            {
                List<Dictionary<string, string>> res = GameBLL.GetGameListForBlack();
                if (res == null || res.Count < 1) return "{}";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (Dictionary<string, string> item in res)
                {
                    if (item.ContainsKey("id") && item.ContainsKey("name") && !dic.ContainsKey(item["id"]))
                        dic.Add(item["id"], item["name"]);
                }
                return JsonConvert.SerializeObject(dic);
            }
        }

    }
}
