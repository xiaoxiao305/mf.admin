<%@ Page Title="俱乐部管理》查看俱乐部保证金订单" Language="C#" MasterPageFile="~/M/main.Master"  AutoEventWireup="true" CodeBehind="ApplyInfo.aspx.cs" Inherits="MF.Admin.UI.M.Guild.ApplyInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
<link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
<script language="javascript" type="text/javascript">
        function showWin() {
            $('.theme-popover-mask').show();
            $('.theme-popover').slideDown(200); 
            $('.theme-popover').css("width",380);
            $('.theme-popover').css("left", "55%");
            $('.theme-popover').css("top", "50%");
            $('.theme-poptit .close').click(function() { $('.theme-popover-mask').hide(); $('.theme-popover').slideUp(200);});
        }
        function setapplyflag()
        {
            var memo = $("#refundmemo").val();
            //var token = $("#token").val();
            if("<%=info.ID%>" == "" || parseInt("<%=info.ID%>") <1)
            {
                $("#lblerr").text("请选择需要处理的保证金记录");
                return;
            }
            //else if (token == "") {
            //    $("#lblerr").text("请输入安全令");
            //    return;
            //}
            ajax.setApplyGuildFlag("setapplyguildflag", [parseInt("<%=info.ID%>"), memo], winresult);
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部保证金订单详细信息</div>
    <table class="editbox">
        <tr><th>订单号</th><td><%=info.OrderNo%></td><th>保证金</th><td><%=info.Money%>元</td></tr>
        <tr><th>申请账号</th><td><%=info.Account%></td><th>申请时间</th><td><%=ConvertToDate("s",info.CreateDate)%></td></tr>
        <tr><th>付款时间</th><td ><%=info.PayDate>0?ConvertToDate("s", info.PayDate).ToString():""%></td><th>支付宝交易号</th><td><%=info.TransId %></td></tr>
        <tr><th>订单状态</th><td><%=ConvertOrderFlag(info.Flag)%></td><th>俱乐部名称</th><td><%=info.GuildName %></td></tr>
        <tr><th>申请退款时间</th><td><%=info.ApplyRefundTime>0?ConvertToDate("s", info.ApplyRefundTime).ToString():""%></td></tr>
        <tr><th>退款支付宝账号</th><td><%=info.AlipayAccount %></td><th>退款支付宝姓名</th><td><%=info.AlipayName %></td></tr>
        <tr><th>退款人</th><td><%=info.RefundUser%></td><th>退款时间</th><td><%=(info.RefundTime>0)?ConvertToDate("s", info.RefundTime).ToString():""%></td></tr>
        <tr><th>退款说明</th><td colspan="3"><%=info.Memo%></td></tr>
        <tr><td colspan="4"  style="text-align:center;height:40px; line-height:40px;"><input type="button" value="退款" class="ui-button-icon-primary" onclick="showWin()" /></td></tr>
    </table>
      <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">处理俱乐部保证金退款</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content">
                <ul>
                    <li>退款备注：<input class="ipt" type="text" id="refundmemo" /></li>
                    <%--<li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>--%>
                    <li class="err red" id="lblerr"></li>
                    <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="setapplyflag()" /></li>
                </ul>

            </div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="waiting_bg" id="loading"></div> 
</asp:Content>
