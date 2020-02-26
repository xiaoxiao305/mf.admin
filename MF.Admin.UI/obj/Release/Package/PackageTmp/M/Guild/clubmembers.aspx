<%@ Page Title=" 俱乐部管理》俱乐部成员" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="GuildList.aspx.cs" Inherits="MF.Admin.UI.M.Guild.GuildList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            var args = [parseInt($("#filed").val()), $("#key").val()];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getMembersList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr,o.Id, 0);
            addCell(tr, o.Name, 1);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["ID","俱乐部"];
            jsonPager.init(ajax.getMembersList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部成员列表</div>
    <div class="search">&nbsp;&nbsp;    
        <select id="filed">
            <option value="1">UID</option>
            <option value="2">账号</option>
        </select>
        <input type="text" id="key" class="box" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>