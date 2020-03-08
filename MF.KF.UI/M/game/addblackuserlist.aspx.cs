using MF.Admin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.KF.UI.M.game
{
    public partial class addblackuserlist : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void AddBlackUserList(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtChargeIds.Value))
                    return;
                //2幺鸡 二七十  斗地主 二鬼 
                string[] gameIds = { "99861", "98861", "97761", "99561", "99761" };
                string[] values = { "-500, -300", "-500,-300", "-300,-200", "-1000,-600", "-5000,-3000" };
                string[] levelStr = { "HIGH", "HIGH", "HIGH", "HIGH", "HIGH" };
                string[] chargeidList = txtChargeIds.Value.Split(',');
                string remark = "批量添加";
                for (int i = 0; i < chargeidList.Length; i++)
                {
                    GameBLL.AddBlackUser(gameIds, chargeidList[i], values, levelStr, remark, 1);
                    Thread.Sleep(10000);
                }
                ShowMessage("success");
            }
            catch (Exception ex)
            {
                Admin.BLL.Base.WriteError("addblackuserlist ex:", ex.Message);
            }
        }
    }
}