using System;
using MF.Admin.BLL;
using System.Web.Configuration;
using System.Configuration;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MF.Admin.UI
{
    public partial class _Default : BasePage, IRequiresSessionState
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDyPwd.Attributes["autocomplete"] = "off";
            txtCode.Attributes["autocomplete"] = "off";
            txtAccount.Attributes["autocomplete"] = "off";
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    if (AdminBLL.Login(txtAccount.Text.Trim(), txtPwd.Text, txtDyPwd.Text,1, hidhd.Value.Trim()))
                        Response.Redirect("/m/default.aspx");
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
