<%@ Page Title=" 游戏管理 》 解散游戏" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="closeroom.aspx.cs" Inherits="MF.KF.UI.M.game.closeroom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
<script language="javascript" type="text/javascript">
    function closeRoom()
    {
        if (<%=isAdmin%>== 1) {
            var id = $("#room").val().trim();
            if (id == "" || parseInt(id) < 1) { alert("请输入正确的包间号"); return; }
            if (confirm("确认要解散包间游戏桌【" + id + "】吗？"))
                ajax.closeRoom([id], winresult);
        } else {
            alert("该账号无【解散游戏】权限");
        }
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">解散游戏</div>
            <div class="search">&nbsp;&nbsp;
                包间桌号<input  type="text" id="room" class="box" />
                <input type="button" value="解散游戏" onclick="closeRoom()" class="ui-button-icon-primary" />
            </div>    
            <div id="container"></div> 
            <div class="loading" id="loading"></div>
</asp:Content>
