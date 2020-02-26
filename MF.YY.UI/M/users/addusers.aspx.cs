using MF.Admin.BLL;
using MF.Common.Security;
using MF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.YY.UI.M.users
{
    public partial class addusers : BasePage
    {
        private int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString == null || Request.QueryString.Count < 1)
                    return;
                int.TryParse(Request["id"], out id);
                if (id < 1)
                    return;
                if (!IsPostBack)
                    InitUserInfo();
            }
            catch (Exception ex)
            {
                lblerr.InnerText = "加载信息异常：" + ex.Message;
            }
        }
        private void InitUserInfo()
        {
            int rowCount = 0;
            List<CPSUsersAdmin> list = CPSUsersBLL.GetCPSUserList(1, 1, 1, 3, id.ToString(), out rowCount);
            if (rowCount < 1)
                return;
            CPSUsersAdmin model = list[0];
            txtbank_acc.Value = model.bank_acc;
            txtbank_addr.Value = model.bank_addr;
            txtbank_name.Value = model.bank_name;
            txtbusiness_link.Value = model.business_link;
            txtchannel.Value = model.channel;
            txtchannel_name.Value = model.channel_name;
            txtdevice.Value = model.device;
            txtemail.Value = model.email;
            txtidnum.Value = model.idnum;
            txtpercent.Value = model.percent.ToString();
            txtprotocol.Value = model.protocol;
            txtqq.Value = model.qq;
            txttelephone.Value = model.telephone;
            txtchannel_num.Value = model.channel_num.ToString();
            //admin
            txtadminaccount.Value = model.admin_account;
            txtadminname.Value = model.admin_name;
            txtadmintoken.Value = model.admin_token;
            ddladminflag.SelectedIndex = model.admin_flag;
            hidadminid.Value = model.admin_id.ToString();
        }

        protected void Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckInput())
                    return;
                CPSUsersAdmin model = new CPSUsersAdmin()
                {
                    channel = txtchannel.Value.Trim().ToUpper(),
                    channel_name = txtchannel_name.Value.Trim(),
                    channel_num = string.IsNullOrEmpty(txtchannel_num.Value.Trim())?0:int.Parse(txtchannel_num.Value.Trim()),
                    idnum = txtidnum.Value.Trim(),
                    device = txtdevice.Value.Trim(),
                    bank_name = txtbank_name.Value.Trim(),
                    bank_addr = txtbank_addr.Value.Trim(),
                    bank_acc = txtbank_acc.Value.Trim(),
                    business_link = txtbusiness_link.Value.Trim(),
                    telephone = txttelephone.Value.Trim(),
                    email = txtemail.Value.Trim(),
                    qq = txtqq.Value.Trim(),
                    percent = string.IsNullOrEmpty(txtpercent.Value.Trim()) ? 0 : decimal.Parse(txtpercent.Value.Trim()),
                    protocol = txtprotocol.Value.Trim(),
                    admin_account = txtadminaccount.Value.Trim(),
                    admin_flag = ddladminflag.SelectedIndex,
                    admin_name = txtadminname.Value.Trim(),
                    admin_token = txtadmintoken.Value.Trim()
                };
                if (!string.IsNullOrEmpty(txtadminpassword.Value.Trim()))
                    model.admin_password = MF.Common.Security.MD5.Encrypt(txtadminpassword.Value.Trim());
                if (id > 0)
                    UpdateUsers(model);
                else
                    AddUsers(model);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Base.WriteError("编辑合作商信息异常：", ex.Message);
                lblerr.InnerText = "编辑合作商信息异常：" + ex.Message;
            }
        }

        private void UpdateUsers(CPSUsersAdmin model)
        {
            if (id > 0)
                model.id = id;
            if (!string.IsNullOrEmpty(hidadminid.Value.Trim()))
                model.admin_id = int.Parse(hidadminid.Value.Trim());
            int res = new CPSUsersBLL().UpdateCPSUsers(model);
            if (res < 1)
            {
                lblerr.InnerText = "编辑合作商信息有误。" + res;
                return;
            }
            Response.Redirect("/m/users/userslist.aspx?t=1");
        }
        private void AddUsers(CPSUsersAdmin model)
        {
            int res = new CPSUsersBLL().AddCPSUsers(model);
            if (res < 1)
            {
                lblerr.InnerText = "添加合作商信息有误。" + res;
                return;
            }
            Response.Redirect("/m/users/userslist.aspx?t=1");
        }
        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(txtchannel.Value.Trim()))
            {
                lblerr.InnerText = "渠道码为空";
                return false;
            }
            else if (string.IsNullOrEmpty(txttoken.Value.Trim()))
            {
                lblerr.InnerText = "安全令为空";
                return false;
            }
            else
            {
                if (!Base.IsDebug)
                {
                    if (txttoken.Value.Trim().Length != 8)
                    {
                        lblerr.InnerText = "请输入8位手机动态密码";
                        return false;
                    }
                    if (!Token.CheckToken(txttoken.Value.Trim(), Base.CurrentUser.Token))
                    {
                        lblerr.InnerText = "安全令有误";
                        return false;
                    }
                }
            }
            return true;
        }
    }
}