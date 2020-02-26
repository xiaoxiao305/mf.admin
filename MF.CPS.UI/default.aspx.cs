﻿using System;
using MF.Admin.BLL;
using System.Web.Configuration;
using System.Configuration;
using System.Web.SessionState;

namespace MF.CPS.UI
{
    public partial class _default : BasePage, IRequiresSessionState
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            txtCode.Attributes["autocomplete"] = "off";
            txtAccount.Attributes["autocomplete"] = "off";
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    if (AdminBLL.LoginCPS(txtAccount.Text.Trim(), txtPwd.Text))
                        Response.Redirect("/m/");
                    else
                        ShowMessage("错误的账号或密码");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Base.WriteError("账号", txtAccount.Text, "登录异常:", ex.Message);
                ShowMessage("错误的账号或密码");
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
            if (!Base.IsDebug)
            {
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
            }
            return true;
        }
    }
}