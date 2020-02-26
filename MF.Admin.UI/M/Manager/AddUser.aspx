<%@ Page Title=" 系统管理 》 添加管理员" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="MF.Admin.UI.M.Manager.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
        <div class="toolbar">添加管理员</div>
        <table class="editbox">
            <tr><th>管理员账号</th><td><asp:TextBox ID="txtAccount" runat="server" CssClass="box w120" ></asp:TextBox></td></tr>
            <tr><th>管理员姓名</th><td><asp:TextBox ID="txtName" runat="server" CssClass="box w120" ></asp:TextBox></td></tr>
            <tr><th>管理员密码</th><td><asp:TextBox ID="txtPwd" runat="server" CssClass="box w120" TextMode="Password"></asp:TextBox></td></tr>
            <tr><th>确认密码</th><td><asp:TextBox ID="txtRepwd" runat="server"  CssClass="box w120" TextMode="Password"></asp:TextBox></td></tr>
            <tr><th>IU-KEY</th><td><asp:TextBox ID="txtKey" runat="server"  CssClass="box w120" ></asp:TextBox></td></tr>
            <tr><th>安全令密码</th><td><asp:TextBox ID="txtToken" runat="server"  CssClass="box w120"></asp:TextBox></td></tr>
            <tr><th></th><td><asp:Button ID="btnSubmit" runat="server" Text="添 加" 
                    CssClass="ui-button-icon-primary" onclick="btnSubmit_Click" /></td></tr>
        </table>
</asp:Content>
