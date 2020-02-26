using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.KF.UI.M.club
{
    public partial class guildlist :BasePage
    {
        public static string club_ids = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["club_id"] != null && !string.IsNullOrEmpty(Request["club_id"].ToString()))
            {
                club_ids = Request["club_id"].ToString();
            }
        }
    }
}