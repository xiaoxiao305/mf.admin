﻿using MF.Admin.BLL;
using System;

namespace MF.KF.UI.M.Charge
{
    public partial class RecordList : BasePage
    {
        protected string account = "";

        public RecordList()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
                return;
            try
            {
                if (Request.QueryString.Count == 1 && Request["account"] != null && !string.IsNullOrEmpty(Request["account"].ToString()))
                    account = Request["account"];
            }
            catch (Exception ex)
            {
                ShowMessage("查询充值记录获取参数异常:" + ex.Message);
                Base.WriteError("getuserRecordList ex:", ex.Message, Request.Url.PathAndQuery);
            }
        }
    }
}
