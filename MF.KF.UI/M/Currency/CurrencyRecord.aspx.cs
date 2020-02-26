using System;
using MF.Common.Json;
using MF.Admin.BLL;
using Newtonsoft.Json;

namespace MF.KF.UI.M.Currency
{
    public partial class CurrencyRecord : BasePage
    {
        protected string matchDic = "{}";
        protected string gameDic = "{}";
        protected string account = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            {
                if (Request.QueryString.Count != 1 || Request.QueryString.GetKey(0) != "account")
                {
                    ShowMessage("参数错误");
                    Base.WriteError(ClientIP, " getuserCurrencyRecord(account:", Request["account"],") err.", Request.Url.PathAndQuery);
                    return;
                }
                if (Request["account"] == null || string.IsNullOrEmpty(Request["account"].ToString()))
                {
                    ShowMessage("获取用户元宝信息失败,参数错误");
                    Base.WriteError(ClientIP, " getuserCurrencyRecord(account:", Request["account"], ") err.args is err.", Request.Url.PathAndQuery);
                    return;
                }
                account = Request["account"];
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
