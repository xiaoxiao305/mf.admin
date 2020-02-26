<%@ Page Title=" 系统管理》管理员列表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="MF.Admin.UI.M.Manager.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">管理员列表</div>
    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" CssClass="list_table">
        <Columns>
            <asp:BoundField HeaderText="账号" DataField="Account" />
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# (int)Eval("Flag")==1?"正常":"冻结" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="姓名" DataField="Name" />
            <asp:BoundField HeaderText="最后一次登录IP" DataField="LastIp" />
            <asp:TemplateField HeaderText="最后一次登录时间">
                <ItemTemplate>
                <%#ConvertToDate("s",(int)Eval(("LastLogin"))).ToString("yyyy-MM-dd HH:mm:ss")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
