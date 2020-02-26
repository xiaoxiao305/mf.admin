using System;
using MF.Common.Json;
using MF.Admin.BLL;

namespace MF.Admin.UI.M.Guild
{
    public partial class UserList : BasePage
    {
        protected string guildMap = "{}";
        protected void Page_Load(object sender, EventArgs e)
        {
           // guildMap = Json.SerializeObject(GuildBLL.GetGuildMap());   
        }
    }
}
