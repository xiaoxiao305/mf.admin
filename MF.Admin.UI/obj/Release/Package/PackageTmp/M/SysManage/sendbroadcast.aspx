<%@ Page  Title="系统管理 》 系统广播" MasterPageFile="~/M/main2.Master"  Language="C#" AutoEventWireup="true" CodeBehind="sendbroadcast.aspx.cs" Inherits="MF.Admin.UI.M.SysManage.sendbroadcast" %>
<asp:Content ID="Content" ContentPlaceHolderID="h" runat="server">
     <link href="http://code.jquery.com/ui/1.9.1/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css"  />
    <script src="http://code.jquery.com/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.9.1/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/common/js/jQuery-Timepicker-Addon/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <script src="/common/js/jQuery-Timepicker-Addon/jquery.ui.datepicker-zh-CN.js.js" type="text/javascript" charset="gb2312"></script>
    <script src="/common/js/jQuery-Timepicker-Addon/jquery-ui-timepicker-zh-CN.js" type="text/javascript"></script>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <style>
        ul>li{border:none;}
    </style>
    <script type="text/javascript">
        $(function () {
            // 时间设置
            $('#time').datetimepicker({
                timeFormat: "HH:mm:ss",
                dateFormat: "yy-mm-dd",
                defaultDate: new Date(),
                showSecond: true,
                stepHour: 1,
                stepMinute: 1,
                stepSecond: 1
            });
        });
        function Send() {
            $("#lblerr").text("");
            var d = $("#time").val();
            var check = $("#checked").is(":checked");
            if (check) {
                d = new Date().setSeconds(30);
            }
            if (d == "") {
                $("#lblerr").text("请选择广播日期");
                return;
            }
            else if ($("#broadText").val() == "") {
                $("#lblerr").text("请输入广播内容");
                return;
            }
            $("#loading").show();
            var date = new Date("1970/01/01 00:00:00");
            var d4 = Math.floor(new Date(d) / 1000) - Math.floor(date.getTime() / 1000);
            ajax.SendBroadCast("sendbroadcast", [d4, $("#broadText").val()], winresult);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">系统广播</div>
    <ul style="width:40%;margin:20px auto;">
        <li style="height:200px;line-height:200px;">广播内容：<textarea id="broadText" rows="12" cols="40" autocomplete="off" ></textarea></li>
        <li>广播时间：<input type="text" id="time" autocomplete="off" style="height: 30px;" />
            <input type="checkbox" id="checked" /> 立即广播</li>
        <li class="err red" id="lblerr"></li>
        <li style="text-align: center;"><input class="btn btn-primary" type="button" value=" 确 定" onclick="Send()" /></li>
    </ul> 
</asp:Content>