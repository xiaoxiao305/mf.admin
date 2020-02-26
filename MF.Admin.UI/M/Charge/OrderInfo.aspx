<%@ Page Title="充值管理》查看订单详细" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="OrderInfo.aspx.cs" Inherits="MF.Admin.UI.M.Charge.OrderInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
<script language="javascript" type="text/javascript">
    function showbox() {
        var flag=<%=info.Flag %>;
        if (flag==0){
            $("#loading").show();
            $("#box").show();
        }else{alert("该订单已处理");}
    }
    function refund() {
        if ($("#token").val().trim() == "") {
            alert("请输入手机安全动态密码");
            return;
        }
        $.ajax({ url: "/m/ajax.ashx", data: { m: "dealorder", args: '["<%=info.OrderNo %>",<%=info.SumbitMoney %>,"' + $("#token").val() + '"]', r: Math.random() }, dataType: "json",cache: false,
            success: function(res) {
                if (res.code == 1)
                 alert("补单成功");
                else 
                  alert(res.msg); 
                 Close();
            },
            error: function(xhr, status, err) {
                Close();
                alert(err);
            }
        });
    }
    function Close() {
        $("#loading").hide();
        $("#box").hide();
    } 
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">查看订单详细信息</div>
    <table class="editbox">
        <tr><th>订单号</th><td colspan="3"><%=info.OrderNo%></td></tr>
        <tr><th>充值账号</th><td colspan="3"><%=info.Account%></td></tr>
        <tr><th>订单状态</th><td colspan="3"><%=ConvertOrderFlag(info.Flag)%></td></tr>
        <tr><th>支付渠道</th><td colspan="3"><%=ConvertPlatform(info.PayChannel)%></td></tr>
        <tr><th>提交金额</th><td><%=info.SumbitMoney%>元</td><th>提交时间</th><td><%=ConvertToDate("s",info.CreateDate)%></td></tr>
        <tr><th>支付金额</th><td><%=info.PayMoney%>元</td><th>支付时间</th><td><%=info.PayDate>0?ConvertToDate("s", info.PayDate).ToString():""%></td></tr>
        <tr><th>兑换金券为</th><td><%=info.Gold%>金券</td><th>订单IP</th><td><%=info.CreateIp%></td></tr>
        <tr><th>支付宝交易号</th><td><%=info.PlatformTransId %></td><th>联运渠道ID</th><td><%=info.Channel>0?info.Channel.ToString() :"" %></td></tr>
        <tr><th>订单设备</th><td ><%=ConvertToDevice(info.Device)%></td><th>活动/任务</th><td><%=info.MissionType%></td></tr>
        <tr><td colspan="4"  style="text-align:center;height:40px; line-height:40px;"><input type="button" value="补单" class="ui-button-icon-primary" onclick="showbox()" /></td></tr>
    </table>
    <div class="waiting_bg" id="loading">
    </div> 
     <div style="border-radius:5px; height:150px; width:300px; border:1px solid #cccccc; background:#fafafa; z-index:100000;margin:0px auto; position:relative; margin-top:-10%; display:none ; " id="box">
        <p style="border-bottom:1px solid #cccccc; line-height:35px; height:35px;margin:0px;padding:0px;  padding-left:8px; ">请输入手机安全令动态密码<span style="float:right;line-height:16px; height:16px; margin-right:5px; cursor:pointer;" onclick="Close()">X</span></p>
        <p style="height:80px;line-height:40px; padding-left:15px; ">
            动态密码:<input type="text" class="box w80" id="token" /> <input type="button" value="确定" class="ui-button-icon-primary" onclick="refund()" /></p>
        </div>
</asp:Content>
