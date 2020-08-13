<%@ Page  Title=" 俱乐部管理》高税俱乐部列表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="hightaxclubs.aspx.cs" Inherits="MF.KF.UI.M.club.hightaxclubs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            jsonPager.queryArgs = [];
            $("#loading").show();
            ajax.getHighTaxClub(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, "<input type='checkbox' value='" + o.Id + "'>", 0);
            addCell(tr, o.Id, 1);
            addCell(tr, o.Name, 2);
            var guildMembers = o.Members_Count > 0 ? "<a href='javascript:void (0);' onclick=\"showClubMembers(" + o.Id + ");\">" + o.Members_Count + "</a>" : "0";
            addCell(tr, guildMembers, 3);
            return tr;
        }

        function showClubMembers(clubId) {
            currentClubId = clubId;
            var url = "/m/club/ClubMembersList.aspx?clubId=" + clubId;
            window.location.href = url;
        }
        $(document).ready(function() {
            var pagerTitles = ["选择","Id", "名称"];
            jsonPager.init(ajax.getHighTaxClub, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            search();
        }); 
        function delhightaxclub()
        {
            if ($(":checkbox").length < 1) {
                alert("请查询数据，并选择需要删除的高税俱乐部。");
                return;
            }
            var recoms = new Array();
            var i = 0;
            $(":checkbox").each(function () {
                if ($(this).is(":checked")) {
                    recoms[i] = $(this).val();
                    i++;
                }
            });
            if (recoms == null || recoms.length < 1) {
                alert("并选择需要删除的高税俱乐部。");
                return;
            }
            if (confirm("确认要删除高税俱乐部【" + recoms + "】操作？")) {
                ajax.delHighTaxClub("delhightaxclub", [recoms], winresult);
            }
        } 
        function addhightaxclub() {
            var clubId = $("#txtClubId").val();
            var token = $("#token").val();
            if (clubId == "") {
                $("#lblerr").text("请输入俱乐部ID");
                return;
            }
            else if (token == "") {
                $("#lblerr").text("请输入安全令");
                return;
            }
            ajax.addHighTaxClub("addhightaxclub", [clubId, token], winresult);
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">高税俱乐部列表</div>
    <div class="search">&nbsp;&nbsp;
       <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="添加高税俱乐部" onclick="showAddMoneyWin(1)" class="ui-button-icon-primary oprbutton"/>
        <input type="button" value="删除高税俱乐部" onclick="delhightaxclub()" class="ui-button-icon-primary oprbutton"/>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
    <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">添加高税俱乐部</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li>俱乐部ID：<input class="ipt" type="text" id="txtClubId" /></li>
            <li class="err red">注：多个俱乐部ID，以英文逗号（,）分隔。</li>
            <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addhightaxclub()" /></li>
        </ul>  
    </div>
</asp:Content>