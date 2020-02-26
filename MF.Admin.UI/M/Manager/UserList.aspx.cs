using System;
using MF.Admin.BLL;

namespace MF.Admin.UI.M.Manager
{
    public partial class UserList :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //gv.DataSource = AdminBLL.GetAdministatorList();
            gv.DataBind(); 
        }
    }
}
