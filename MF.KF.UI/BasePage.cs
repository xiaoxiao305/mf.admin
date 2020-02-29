﻿using MF.Admin.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MF.KF.UI
{
    public class BasePage : System.Web.UI.Page
    {

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
        public int isAdmin {
            get
            {
                MF.Data.Administrator administrator = Session["administrator"] as MF.Data.Administrator;
                if (administrator == null) return 0;
                return administrator.IsAdmin;
            }
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
                    if (item.ContainsKey("id") && item.ContainsKey("name"))
                        dic.Add(item["id"], item["name"]);
                }
                return JsonConvert.SerializeObject(dic);  
            }
        } 




        #region 是否登录
        protected bool IsLogin
        {
            get
            {
                return AdminBLL.CheckIsLogin();
            }
        }
        #endregion
        #region 客户端IP地址
        protected string ClientIP
        {
            get
            {
                return Base.ClientIP;
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
    }
}