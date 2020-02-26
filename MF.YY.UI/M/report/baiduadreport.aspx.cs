using MF.Admin.BLL;
using Newtonsoft.Json;
using System;

namespace MF.YY.UI.M.report
{
    public partial class baiduadreport : BasePage
    {
        protected string channelDic = "{}";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                channelDic = JsonConvert.SerializeObject(Base.ChannelNumConst);
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, User != null ? User.Account : "", "mf/yy/ui/m/report/baiduadreport.aspx", ex.Message);
            }

        }
    }
}