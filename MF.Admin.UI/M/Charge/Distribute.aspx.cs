using System;

namespace MF.Admin.UI.M.Charge
{
    public partial class Distribute : BasePage
    {
        protected string title = "金券充值分布";
        protected int type = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 1 && Request.QueryString.GetKey(0) == "t") {
                int.TryParse(Request["t"], out type);
                if (type == 2) title = "银票充值分布";
            }
        }
    }
}
