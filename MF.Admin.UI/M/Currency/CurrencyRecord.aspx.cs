using System;
using MF.Common.Json;
using MF.Admin.BLL;
using Newtonsoft.Json;

namespace MF.Admin.UI.M.Currency
{
    public partial class CurrencyRecord : BasePage
    {
        protected string matchDic = "{}";
        protected string gameDic = "{}";
        protected string account = "";
        protected string chargeId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            { 
                if (Request["account"] != null && !string.IsNullOrEmpty(Request["account"].ToString()))
                    account = Request["account"];
                if (Request["chargeid"] != null && !string.IsNullOrEmpty(Request["chargeid"].ToString()))
                    chargeId = Request["chargeid"];
                InitGameInfo();//加载游戏
            }
            catch (Exception ex)
            {
                ShowMessage("查询元宝记录获取参数异常:" + ex.Message);
                Base.WriteError("getuserCurrencyRecord ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }

        private void InitGameInfo()
        {
            try
            {
                matchDic = JsonConvert.SerializeObject(GameBLL.GetMatchMap());
                gameDic = JsonConvert.SerializeObject(GameBLL.GetGameMap());
            }
            catch (Exception ex)
            {
                Base.WriteError("get gamedata(currency) ex:", ex.Message);
            }
        }
    }
}
