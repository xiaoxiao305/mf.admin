<%@ Page Title="用户管理 》 包间账号列表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="SubUserList.aspx.cs" Inherits="MF.Admin.UI.M.Users.SubUserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            $("#loading").show();
            var args = ["<%=account%>"];
            jsonPager.queryArgs = args;
            ajax.getSubUserList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index,data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            state = function (_state) { if (_state == 1) { return "正常"; } else { return "冻结"; } };
            addCell(tr, o.Account, 0);
            addCell(tr, o.Nickname, 1);
            addCell(tr, o.Master, 2);
            addCell(tr, state(o.Flag), 3);
            addCell(tr, o.Currency, 4);
            addCell(tr, new Date("2012/10/01").dateAdd("s", o.Regitime).Format("yyyy-MM-dd hh:mm:ss"), 5);
            addCell(tr, new Date("2012/10/01").dateAdd("s", o.LastLogin).Format("yyyy-MM-dd hh:mm:ss"), 6);
            addCell(tr, o.LoginCount, 7);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["账号", "昵称","所属主账号", "状态", "元宝", "创建时间", "登录时间", "登录次数"];
            jsonPager.init(ajax.getSubUserList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if ("<%=account%>" != "") search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">用户列表</div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>