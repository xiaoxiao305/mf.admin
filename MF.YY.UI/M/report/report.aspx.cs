using MF.Admin.BLL;
using MF.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.YY.UI.M.report
{
    public partial class report : BasePage
    {
        protected string channellistDic = "{}";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                channellistDic = JsonConvert.SerializeObject(Base.ChannelNameConst);
            }
            catch (Exception ex)
            {
                Base.WriteError(ClientIP, User != null ? User.Account : "", "/m/report/report.aspx", ex.Message);
            }
        }
    }
}