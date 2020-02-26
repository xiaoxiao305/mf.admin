<%@ Page Title=" 管理设置 》 游戏包间列表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="roomlist.aspx.cs" Inherits="MF.Admin.UI.M.game.roomlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server"><script language="javascript" type="text/javascript">
    function setGameSetting()
    {
        var clubId = $("#clubId").val().trim();
        if (clubId == "") return;
        if (confirm("确认要重置包间配置吗？")) {
            $("#loading").show();
            ajax.setGameSetting([clubId], winresult);
        }
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏包间配置</div>
    <div class="search">&nbsp;&nbsp;
        参考俱乐部ID<input  type="text" id="clubId" class="box" value="" />
        <input type="button" value="重置包间配置" onclick="setGameSetting()" class="ui-button-icon-primary" />
    </div>
    <div class="loading" id="loading"></div>
</asp:Content>
