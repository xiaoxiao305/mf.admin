using MF.Admin.BLL;
using MF.Common.Json;
using MF.Data.ExtendChannel;
using System;

namespace MF.Admin.UI.M.Report
{
    public partial class ExtendChannel : BasePage
    {
        protected string channellistarray = "";
        protected string channelnumlistarray = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                channellistarray = Json.SerializeObject(Enum.GetNames(typeof(ChannelEnum)));
                channelnumlistarray = Json.SerializeObject(Enum.GetNames(typeof(ChannelNumEnum)));
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, User != null ? User.Account : "", "/m/report/extendchannel.aspx", ex.Message);
            }
        }
    }
}