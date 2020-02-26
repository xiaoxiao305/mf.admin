using MF.Admin.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.KF.UI.M.game
{
    public partial class gameblacklist : BasePage
    {
        protected string gameDic = "{}";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<Dictionary<string, string>> res = GameBLL.GetGameListForBlack();
                if (res == null || res.Count < 1) return;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (Dictionary<string, string> item in res)
                {
                    if (item.ContainsKey("id") && item.ContainsKey("name"))
                        dic.Add(item["id"], item["name"]);
                }
                gameDic = JsonConvert.SerializeObject(dic);
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, "/m/game/gameblacklist.aspx", ex.Message);
            }
        }
    }
}