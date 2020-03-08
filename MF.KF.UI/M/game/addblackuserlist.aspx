<%@ Page Title=" 游戏管理 》 批量添加黑名单" MasterPageFile="~/M/main.Master"  Language="C#" AutoEventWireup="true" CodeBehind="addblackuserlist.aspx.cs" Inherits="MF.KF.UI.M.game.addblackuserlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">批量添加黑名单</div>
          <textarea rows="3" cols="20" id="txtChargeIds" runat="server"></textarea>
        <asp:Button runat="server" OnClick="AddBlackUserList" Text="批量添加黑名单" /> 
</asp:Content>