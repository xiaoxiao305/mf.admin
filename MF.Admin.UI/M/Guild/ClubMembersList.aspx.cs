using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.Admin.UI.M.Guild
{
    public partial class ClubMembersList : BasePage
    {
        protected int clubId;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["clubId"] != null && !string.IsNullOrEmpty(Request["clubId"].ToString()))
                    clubId = int.Parse(Request["clubId"].ToString());
            }
            catch (Exception ex) {
                Admin.BLL.Base.WriteError("member.cs err:", ex.Message);
            }
        }
    }
}