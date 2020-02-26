using System;
using MF.Admin.BLL;
using MF.Common.Security;

namespace MF.Admin.UI.M.Manager
{
    public partial class UpdatePwd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsLogin)
                {
                    if (txtOld.Text == "")
                    {
                        ShowMessage("请输入旧密码");
                        return;
                    }
                    if (txtPwd.Text == "")
                    {
                        ShowMessage("请输入新密码");
                        return;
                    }
                    if (txtPwd1.Text == "")
                    {
                        ShowMessage("请输入确认新密码");
                        return;
                    }
                    if (txtPwd1.Text != txtPwd.Text)
                    {
                        ShowMessage("两次输入的新密码不一致");
                        return;
                    }
                    if (txtToken.Text.Trim() == "")
                    {
                        ShowMessage("请输入手机安全令动态密码");
                        return;
                    }

                    //if (CheckToken(txtToken.Text))
                    //{
                    //    if (MD5.Encrypt(txtOld.Text).ToLower() != User.Password.ToLower())
                    //    {
                    //        ShowMessage("原密码错误");
                    //        LogBLL.WriteSystemLog(User.Account, ClientIP, "修改密码失败,旧密码错误", "/m/manager/updatepwd.aspx");
                    //        return;
                    //    }
                    //    if (AdminBLL.UpdatePassword(User.Account, txtPwd1.Text))
                    //    {
                    //        LogBLL.WriteSystemLog(User.Account, ClientIP, "修改密码成功", "/m/manager/updatepwd.aspx");
                    //        ShowMessage("修改密码成功");
                    //    }
                    //    else
                    //    {
                    //        LogBLL.WriteSystemLog(User.Account, ClientIP, "修改密码失败", "/m/manager/updatepwd.aspx");
                    //        ShowMessage("修改密码失败");
                    //    }
                    //}
                    //else
                    //{
                    //    ShowMessage("手机安全令动态密码错误");
                    //    //LogBLL.WriteSystemLog(User.Account, ClientIP, "修改密码失败,手机安全令动态密码错误", "/m/manager/updatepwd.aspx");
                    //}
                }
            }
            catch (Exception ex)
            {
                WriteError("修改密码时出错:", ex.Message);
                ShowMessage("修改失败");

            }
        }
    }
}
