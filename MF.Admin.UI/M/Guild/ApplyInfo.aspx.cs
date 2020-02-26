using MF.Admin.BLL;
using MF.Data;
using System;
namespace MF.Admin.UI.M.Guild
{
    public partial class ApplyInfo : BasePage
    {
        protected GuildApplyRecord info = new GuildApplyRecord();
        int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.Count != 1 || Request.QueryString.GetKey(0) != "id")
                    return;
                int.TryParse(Request["id"], out id);
                if (id <= 0)
                    return;
                var res = GuildBLL.GetGuildApplyInfo(id);
                if (res.Code == 1)
                {
                    info = res.R;
                    if (info.Flag != 3)
                    {
                        ShowMessage("该订单状态不能进行退款操作", "/M/Guild/ApplyRecord.aspx");
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(res.Message) && res.Message.Trim() != "")
                        ShowMessage(res.Message);
                    else
                        ShowMessage("加载俱乐部保证金订单信息失败");
                }
            }
            catch (Exception ex)
            {
                Base.WriteError(User != null ? User.Account : "", ClientIP, Request.Url.PathAndQuery, ex.Message);
            }
        }
        protected string ConvertOrderFlag(long flag)
        {
            if (flag == 0)
                return "等待支付";
            else if (flag == 1)
                return "支付成功";
            else if (flag == 2)
                return "已创建俱乐部";
            else if (flag == 3)
                return "申请退款中";
            else if (flag == 4)
                return "已退款";
            else
                return flag.ToString();
        }
    }
}