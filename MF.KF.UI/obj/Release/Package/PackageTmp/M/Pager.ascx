<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="MF.KF.UI.M.Pager" %>
<div class="pager">
    共有记录<%=RowCount %>条 每页显示<asp:TextBox ID="txtPageSize" runat="server" CssClass="input" Text="30"></asp:TextBox>条 <%=PageIndex %>/<%=PageCount %>页
    <asp:Button ID="btnFirst" runat="server" onclick="btnFirst_Click" CssClass="button first"  Text="" ToolTip="第一页"/>
    <asp:Button ID="btnPrev" runat="server" onclick="btnPrev_Click" Text="" CssClass="button prev" ToolTip="上一页" />
    <asp:Button ID="btnNext" runat="server" onclick="btnNext_Click" Text=""  CssClass="button next" ToolTip="下一页" />
    <asp:Button ID="btnLast" runat="server" onclick="btnLast_Click" Text="" CssClass="button last" ToolTip="最后一页" />
    <asp:TextBox ID="txtPage" runat="server" CssClass="input"></asp:TextBox>
    <asp:Button ID="btnGo" runat="server" Text="GO" onclick="btnGo_Click"  CssClass="ui-button-icon-primary" ToolTip="转到"/>&nbsp;
    <asp:HiddenField ID="pageCount" runat="server" Value="0" />
    <asp:HiddenField ID="currentPage" runat="server" Value="1" />
</div>
