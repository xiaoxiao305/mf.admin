<%@ Page Title=" 俱乐部管理》俱乐部活跃" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="clubactive.aspx.cs" Inherits="MF.Admin.UI.M.Guild.clubactive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript">
        jsonPager.queryArgs = [];
        var rowCount = 0;
        ajax.getClubActiveCount(jsonPager.makeArgs(1), searchRowData);
        function searchRowData(data) {
            if (data.code == 1) {
                rowCount = data.result.rank_count;
            }
        }
        function search() {
            var args = [0];
            if ($("#order").is(":checked"))
                args = [parseInt($("#order").val())];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getClubActive(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, "<a href ='/m/guild/guildlist.aspx?club_id=" + o.club_id + "' target='main' class='white'>" + o.club_id + "</a>", 0);
            addCell(tr, o.nick_name, 1);
            addCell(tr, o.active, 2);
            return tr;
        }
        $(document).ready(function () {
            attachCalenderbox('#starttime', '', null, new Date().Format("yyyy-MM-dd"), '');
            var pagerTitles = ["Id", "俱乐部", "上一周收益（元宝）"];
            jsonPager.init(ajax.getClubActive, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部收益详情</div>
    <div class="search">&nbsp;&nbsp;
        <input type="checkbox" value="-1" id="order" />倒序
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div> 
</asp:Content>