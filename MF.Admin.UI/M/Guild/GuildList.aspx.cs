using System;

namespace MF.Admin.UI.M.Guild
{
    public partial class GuildList : System.Web.UI.Page
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
