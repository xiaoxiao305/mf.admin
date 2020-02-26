using MF.Admin.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.Admin.UI.M.Report
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
                Base.WriteError(ClientIP, User != null ? User.Account : "", "/m/report/baiduadreport.aspx", ex.Message);
            }
            
        }
    }
}