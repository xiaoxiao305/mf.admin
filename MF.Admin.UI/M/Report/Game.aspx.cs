using System;
using MF.Common.Json;
using MF.Admin.BLL;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MF.Admin.UI.M.Report
{
    public partial class Game : BasePage
    { 
        protected string gameDic = "{}"; 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                  gameDic = JsonConvert.SerializeObject(GameBLL.GetGameMap());
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, User != null ? User.Account : "", "/m/report/game.aspx", ex.Message);
            }
        }
    }
}
