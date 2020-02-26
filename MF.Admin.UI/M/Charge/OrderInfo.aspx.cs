using System;
using MF.Data;
using MF.Admin.BLL;
namespace MF.Admin.UI.M.Charge
{
    public partial class OrderInfo : BasePage
    {
        protected int id  =0;
        protected ChargeRecord info = new ChargeRecord();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (Request.QueryString.Count == 1 && Request.QueryString.GetKey(0) == "id")
                //{
                //     int.TryParse(Request["id"], out id);
                //     if (id > 0)
                //     {
                //         var res = ChargeBLL.GetChargeOrder(id);
                //         if (res.Code == 1) info = res.R;
                //         else
                //         {
                //             if (!string.IsNullOrEmpty(res.Message) && res.Message.Trim() != "")
                //                 ShowMessage(res.Message);
                //             else
                //                 ShowMessage("加载充值订单信息失败");
                //         }
                //     }
                //}
            }
            catch (Exception ex)
            {
                WriteError(User != null ? User.Account : "", ClientIP, Request.Url.PathAndQuery, ex.Message);
            }


        }
        protected string ConvertOrderFlag(int flag)
        {
            if (flag == 0) return "等待支付";
            else if (flag == 1) return "支付成功";
            else if (flag == 2) return "支付失败";
            else return flag.ToString();
        }
        protected string ConvertPlatform(int platformId)
        {
            if (platformId == 0) return "官方充值";
            else if (platformId == 10) return "支付宝(官网)";
            else if (platformId == 11) return "支付宝(App)";
            else if (platformId == 2) return "易宝";
            else if (platformId == 3) return "iOS";
            else if (platformId == 40) return "微信(官网)";
            else if (platformId == 41) return "微信(App)";
            else if (platformId == 5) return "联运渠道";
            else return platformId.ToString();
        }
        protected string ConvertToDevice(int device) {
            if (device == 0) return "官网";
            else if (device == 1) return "Web端";
            else if (device == 2) return "客户端";
            else if (device == 3) return "iPad";
            else if (device == 4) return "Android";
            else if (device == 5) return "iPhone";
            else if (device == 6) return "Pad";
            else if (device == 7) return "手机官网";
            else return device.ToString();
        }
    }
}
