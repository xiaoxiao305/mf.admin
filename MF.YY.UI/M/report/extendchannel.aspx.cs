using MF.Admin.BLL;
using MF.Common.Json;
using MF.Data.ExtendChannel;
using Newtonsoft.Json;
using System;

namespace MF.YY.UI.M.report
{
    public partial class extendchannel : BasePage
    {
        protected string channelDic = "{}";
        protected string channelnumlistarray = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
                channelDic = JsonConvert.SerializeObject(Base.ChannelNumConst);
                channelnumlistarray = Json.SerializeObject(Enum.GetNames(typeof(ChannelNumEnum)));
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, User != null ? User.Account : "", "/m/report/extendchannel.aspx", ex.Message);
            }
        }
    }
}