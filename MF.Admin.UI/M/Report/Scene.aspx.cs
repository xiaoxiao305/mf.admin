using System;
using MF.Common.Json;
using MF.Admin.BLL;
using Newtonsoft.Json;

namespace MF.Admin.UI.M.Report
{
    public partial class Scene : BasePage
    {
        protected int gameid = 0;
        protected string gameDic = "{}";
        protected string matchDic = "{}";
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                if (Request.QueryString.Count == 1 && Request.QueryString.GetKey(0) == "gameid")
                    int.TryParse(Request["gameid"], out gameid);
                gameDic = JsonConvert.SerializeObject(GameBLL.GetGameMap());
                matchDic = JsonConvert.SerializeObject(GameBLL.GetMatchMap());
            }
            catch (Exception ex) {
                Base.WriteError(ClientIP,User!=null?User.Account:"","/m/report/scene.aspx", ex.Message); 
            }
        }
    }
}
