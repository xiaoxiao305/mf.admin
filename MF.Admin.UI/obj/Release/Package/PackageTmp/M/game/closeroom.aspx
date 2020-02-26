<%@ Page Title=" 游戏管理 》 解散游戏" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="closeroom.aspx.cs" Inherits="MF.Admin.UI.M.game.closeroom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
<script language="javascript" type="text/javascript">
    function closeRoom()
    {
        var id = $("#room").val().trim();
        if (id == "") return;
        if (confirm("确认要解散包间游戏桌【" + id + "】吗？"))
            ajax.closeRoom([id], winresult);
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">解散游戏</div>
    <div class="search">&nbsp;&nbsp;
        包间桌号<input  type="text" id="room" class="box" />
        <input type="button" value="解散游戏" onclick="closeRoom()" class="ui-button-icon-primary" />
        <div class="loading" id="loading"></div>
    </div>     
</asp:Content>
