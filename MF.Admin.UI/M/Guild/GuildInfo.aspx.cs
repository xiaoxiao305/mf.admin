using MF.Admin.BLL;
using System;
namespace MF.Admin.UI.M.Guild
{
    public partial class GuildInfo :BasePage
    {
        protected MF.Data.Guild guild = new MF.Data.Guild();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            {
                int id = 0;
                if (Request.QueryString.Count != 1 || Request.QueryString.GetKey(0) != "id")
                {
                    ShowMessage("参数错误");
                    Base.WriteError(ClientIP, " getuserguild(id:", Request["id"], ") err.args is err.", Request.Url.PathAndQuery);
                    return;
                }
                int.TryParse(Request["id"], out id);
                if (id < 1)
                {
                    ShowMessage("获取用户俱乐部信息失败,参数错误");
                    Base.WriteError(ClientIP, " getuserguild(id:", Request["id"], ") err.args is err.id is ", Request["id"], Request.Url.PathAndQuery);
                    return;
                } 
                var res = GuildBLL.GetGuildInfo(id);
                if (res.Code == 1)
                    guild = res.R;
                else
                {
                    ShowMessage("获取用户俱乐部信息失败:" + res.Message);
                    Base.WriteError("getuserguild(", guild.Name, ") err'msg is ", res.Message, Request.Url.PathAndQuery);
                }

            }
            catch (Exception ex)
            {
                ShowMessage("获取用户俱乐部信息异常:" + ex.Message);
                Base.WriteError("getuserguild ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }
    }
}
