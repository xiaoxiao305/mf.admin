using System;
using MF.Admin.BLL;
using System.Web.Configuration;
using System.Configuration;
using System.Web.SessionState;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MF.Data;
using System.Linq;
using MF.Common.Security;

namespace MF.KF.UI
{
    public partial class Default : BasePage, IRequiresSessionState, IReadOnlySessionState
    {
        private string chargeids = "";
        protected void Page_Load(object sender, EventArgs e)
        { 
            txtDyPwd.Attributes["autocomplete"] = "off";
            txtCode.Attributes["autocomplete"] = "off";
            txtAccount.Attributes["autocomplete"] = "off";
        }
     
        //private void GetInfo()
        //{
        //    //黑名单数据
        //    List<GameBlackUserInfo> black = GameBLL.GetGameBlackUsers(0, 1, "");
        //    //待审核
        //    List<GameBlackUserInfo> auditBlack = GameBLL.GetAuditBlackUsers(0, 1, "");
        //    if (auditBlack != null && auditBlack.Count > 0)
        //    {
        //        if (black == null)
        //            black = new List<GameBlackUserInfo>();
        //        black.AddRange(auditBlack);
        //    }
        //    if (black == null)
        //    {
        //        Response.Write("data is null");
        //        return;
        //    }
        //    //initData();
        //    if (string.IsNullOrEmpty(chargeids))
        //    {
        //        Response.Write("parm is null");
        //        return;
        //    }
        //    string[] chargeIdList = chargeids.Split(',');
        //    UserBLL.SetChargeIdCache(chargeIdList);
        //    for (int i = 0; i < chargeIdList.Length; i++)
        //    {
        //        bool isBlack = false;
        //        foreach (var item in black)
        //        {
        //            if (item.ChargeId.Trim().ToUpper() == chargeIdList[i].Trim().ToUpper())
        //            {
        //                isBlack = true;
        //                break;
        //            }
        //        }
        //        string str = chargeIdList[i];
        //        Admin.BLL.Base.WriteError(str);
        //        //str = "";
        //        //if (isBlack)
        //        //    str += "黑名单"; 
        //        //else
        //        //    str += "";
        //        //Admin.BLL.Base.WriteLog(str);
        //        str = "";
        //        Users u = UserBLL.GetChargeIdCache(chargeIdList[i]);
        //        if (u != null)
        //            str += u.GUID;
        //        else
        //            str += "";
        //        Admin.BLL.Base.WriteLog(str);
        //    }





        //}

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    if (AdminBLL.Login(txtAccount.Text, txtPwd.Text, txtDyPwd.Text, 2))
                        Response.Redirect("/m/default.aspx");
                    else
                        ShowMessage("错误的账号或密码。");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Base.WriteError("账号", txtAccount.Text, "登录异常:", ex.Message);
                ShowMessage("错误的账号或密码.");
            }
        }
        bool checkInput()
        {

            if (txtAccount.Text.Trim() == "")
            {
                ShowMessage("账号不能为空");
                return false;
            }
            if (txtPwd.Text.Trim() == "")
            {
                ShowMessage("密码不能为空");
                return false;
            }
            if (txtCode.Text.Trim() == "")
            {
                ShowMessage("请输入验证码");
                return false;
            }
            if (txtCode.Text.Trim().ToLower() != VerificationCode1.Text.ToLower())
            {
                ShowMessage("验证码错误");
                return false;
            }
            if (!Base.IsDebug)
            {
                if (txtDyPwd.Text.Trim() == "")
                {
                    ShowMessage("请输入8位手机动态密码");
                    return false;
                }
                if (txtDyPwd.Text.Trim().Length != 8)
                {
                    ShowMessage("请输入8位手机动态密码");
                    return false;
                }
            }
            return true;
        }
      
    }
}
