<%@ Page  Title=" 俱乐部管理》联盟俱乐部成员" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="ClubMembersList.aspx.cs" Inherits="MF.Admin.UI.M.Guild.ClubMembersList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        jsonPager.pageSize = 1000;
        function search() {
            var args = [$("#clubId").val(), $("#memberId").val()];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getClubMembersList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            console.log("search memberslist:", data);
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
            addCell(tr, o.clubname, 0);
            addCell(tr, o.id, 1);
            addCell(tr, o.nickname, 2);
            addCell(tr, (new Date(o.join_date)).format("yyyy-MM-dd hh:mm:ss"), 3);
            return tr;
        }
        $(document).ready(function () { 
            var pagerTitles = ["俱乐部", "UID", "昵称", "入会时间"];
            jsonPager.init(ajax.getClubMembersList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            var c =<%=clubId%>;
            if (c != "" && c > 0) {
                $("#clubId").val(c);
                search();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部成员列表</div>
    <div class="search">&nbsp;&nbsp;
        俱乐部Id<input type="text" id="clubId" class="box"/>
        成员Id<input type="text" id="memberId" class="box"/>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>