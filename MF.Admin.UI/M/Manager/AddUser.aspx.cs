using System;
using MF.Data;
using MF.Common.Security;
using MF.Admin.BLL; 
namespace MF.Admin.UI.M.Manager
{
    public partial class AddUser :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsLogin) return;
                var admin = new Administrator();
                admin.Account = txtAccount.Text;
                admin.Flag = 1;
                admin.LastIP = "";
                admin.LastLogin = 0;
                admin.Name = txtName.Text;
                admin.Password = txtRepwd.Text;
                admin.Powers = "";
                admin.Token = txtKey.Text;
                if (admin.Account.Trim() == "") {
                    ShowMessage("请输入要添加的管理员账号");
                    return;
                }
                if (admin.Password == "")
                {
                    ShowMessage("管理员密码不能为空");
                    return;
                }
                if (admin.Password != txtPwd.Text) {
                    ShowMessage("两次密码输入不一致");
                    return;
                }
                if (admin.Token == "")
                {
                    ShowMessage("请输入管理员账号需要绑定的手机安全令Key");
                    return;
                }
                //if (!CheckToken(txtToken.Text)) {

                //    ShowMessage("手机安全令动态密码错误");
                //    LogBLL.WriteSystemLog(User.Account, ClientIP, string.Format("添加管理员账号[{0}]时输入了错误的手机安全令动态密码",admin.Account) , "/m/manager/adduser.aspx"); 
                //    return;
                //}
                //admin.Password = MD5.Encrypt(admin.Password);
                //if (AdminBLL.AddAdministartor(admin))
                //    ShowMessage("添加成功");
                //else {
                //    LogBLL.WriteSystemLog(User.Account, ClientIP, string.Format("添加管理员账号[{0}]失败", admin.Account), "/m/manager/adduser.aspx");
                //    ShowMessage("添加失败");
                //}
            }
            catch (Exception ex) {
                WriteError("添加管理员账号异常:", ex.Message);
                ShowMessage("添加失败");
            }
            
        }
    }
}
