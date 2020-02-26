using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.YY.UI.M.users
{
    public partial class userslist : System.Web.UI.Page
    {
        protected int type = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString == null || Request.QueryString.Count < 1)
                return;
            int.TryParse(Request["t"], out type);
        }
    }
}