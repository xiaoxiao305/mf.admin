<%@ Page Title=" 系统管理 》 修改密码" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UpdatePwd.aspx.cs" Inherits="MF.Admin.UI.M.Manager.UpdatePwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
        <div class="toolbar">修改管理员密码</div>
        <table class="editbox">
            <tr><th>账号</th><td><%=User.Account %></td></tr>
            <tr><th>旧密码</th><td><asp:TextBox ID="txtOld" runat="server" CssClass="box w120" TextMode="Password"></asp:TextBox></td></tr>
            <tr><th>新密码</th><td><asp:TextBox ID="txtPwd" runat="server"  CssClass="box w120" TextMode="Password"></asp:TextBox></td></tr>
            <tr><th>确认新密码</th><td><asp:TextBox ID="txtPwd1" runat="server"  CssClass="box w120" TextMode="Password"></asp:TextBox></td></tr>
            <tr><th>安全令密码</th><td><asp:TextBox ID="txtToken" runat="server"  CssClass="box w120"></asp:TextBox></td></tr>
            <tr><th></th><td><asp:Button ID="btnSubmit" runat="server" Text="修 改" 
                    CssClass="ui-button-icon-primary" onclick="btnSubmit_Click" /></td></tr>
        </table>
</asp:Content>
