<%@ Page Title="系统管理 》 设置游戏推送消息" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="PushNews.aspx.cs" Inherits="MF.Admin.UI.M.SysManage.PushNews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function PushNews() {
            if ($(":radio").length < 1) {
                alert("请选择需要推送消息的设备信息。");
                return;
            }
            var recoms;
            $(":radio").each(function () {
                if ($(this).is(":checked")) {
                    recoms = parseInt($(this).val());
                }
            }); 
            if (recoms == null || recoms == "") {
                $("#lblerr").html("请选择需要推送消息的设备信息。");
                return;
            }
            else if ($("#news").val() == "") {
                $("#lblerr").html("请输入需要推送的消息。");
                return;
            }
            else if ($("#token").val() == "") {
                $("#lblerr").html("请输入安全令。");
                return;
            }
            ajax.setPushNews("setpushnews", [recoms, $("#news").val(), $("#token").val()], winresult);
        }
        $(document).ready(function () {
            showAddMoneyWin(1);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">设置游戏推送消息</div>
    <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">设置游戏推送消息</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <ul style="width:88%;" class="hide" id="T1">
        <li>推送消息设备：<input type="radio" value="1" checked="checked" name="sendDevice" />Android <input type="radio" value="2" name="sendDevice" />iOS</li>
        <li style="height:120px;">消息内容：<textarea class="ipt" id="news" style="height:100px;"></textarea></li>
        <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
        <li class="err red" id="lblerr"></li>
        <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="PushNews()" /></li>
    </ul> 
    <!--弹出窗口结束-->
</asp:Content>