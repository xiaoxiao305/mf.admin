<%@ Page  Title="俱乐部管理》设置推荐俱乐部" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="SetSuggestGuild.aspx.cs" Inherits="MF.Admin.UI.M.Guild.SetSuggestGuild" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">设置推荐俱乐部</div>
    <br /><br />　　　<asp:FileUpload  ID="uploadFile" runat="server"/>
    <label style="color:red;font-weight:bold;">只支持.xls版本</label>
    <br /><br /><br />　　　<asp:Button ID="btnLoad" runat="server" Text="上传推荐俱乐部excel" OnClick="LoadFile" /><br /><br />
    <label id="lblerr" runat="server" style="color:red;font-weight:bold;"></label>
</asp:Content>
