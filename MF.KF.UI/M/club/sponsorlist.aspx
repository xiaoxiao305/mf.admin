<%@ Page Title=" 俱乐部管理 》 管理员列表" Language="C#" MasterPageFile="~/M/main.Master"  AutoEventWireup="true" CodeBehind="sponsorlist.aspx.cs" Inherits="MF.KF.UI.M.club.sponsorlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
   
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            var args = [$("#txtClubId").val(), $("#txtMemberId").val()];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getSponsor(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            }else{
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            }; 
            addCell(tr, o.club_id, 0);
            addCell(tr, o.member_id, 1);
            addCell(tr, o.sponsor_id, 2);
            addCell(tr, "<a href='javascript:;' onclick=\"goDelSponsor('" + o.club_id + "','" + o.member_id + "')\">删除</a>", 3);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["俱乐部Id","玩家Id","管理员Id","操作"];
            jsonPager.init(ajax.getSponsor, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
        function setSponsor() {
            var clubId1 = $("#txtClubId1").val();
            var member1 = $("#txtMemberId1").val();
            var sponsor1 = $("#txtSponsorId1").val();
            if (clubId1 == "" || member1 == "" || sponsor1 == "") { alert("请输入需要设置的信息"); return; }
            var token1 = $("#txtToken1").val();
            if (token1 == "") { alert("请输入安全令"); return; }
            ajax.setSponsor("setsponsor", [clubId1, member1, sponsor1, token1], winresult);
        }
        function joinClub() {
            var clubId2 = $("#txtClubId2").val();
            var member2 = $("#txtMemberId2").val();
            var sponsor2 = $("#txtSponsorId2").val();
            if (clubId2 == "" || member2 == "" || sponsor2 == "") { alert("请输入需要设置的信息"); return; }
            var token2 = $("#txtToken2").val();
            if (token2 == "") { alert("请输入安全令"); return; }
            ajax.joinClub("joinclub", [clubId2, member2, sponsor2, token2], winresult);
        }        
        function goDelSponsor(clubId,memberId)
        {
            if (clubId == "" || memberId == "") { alert("数据有误"); return;}
            if (confirm("确认删除此信息？")) {
                ajax.delSponsor("delsponsor", [clubId, memberId], winresult);
            } 
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">管理员</div>
    <div class="search">&nbsp;&nbsp;
         俱乐部ID<input  type="text" id="txtClubId" class="box" />
         玩家ID<input  type="text" id="txtMemberId" class="box" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="设置管理员" onclick="showAddMoneyWin(1);" class="ui-button-icon-primary oprbutton"/>
        <input type="button" value="加入俱乐部" onclick="showAddMoneyWin(2);" class="ui-button-icon-primary oprbutton"/>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
     <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle"></h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide"> 
        <ul id="T1">
            <li class="red" style="font-weight:bold;">设置管理员</li>
            <li>俱乐部Id：<input class="ipt" type="text" id="txtClubId1" /></li>
            <li>玩家Id：<input class="ipt" type="text" id="txtMemberId1" /></li>
            <li>管理员Id：<input class="ipt" type="text" id="txtSponsorId1" /></li>
            <li>安　全　令：<input class="ipt" type="text" id="txtToken1" /></li> 
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="setSponsor()" /></li>
        </ul>
        <ul id="T2">
            <li class="red" style="font-weight:bold;">设置俱乐部成员</li>
            <li>俱乐部Id：<input class="ipt" type="text" id="txtClubId2" /></li>
            <li>玩家Id：<input class="ipt" type="text" id="txtMemberId2" />(多个以,分隔)</li>
            <li>管理员Id：<input class="ipt" type="text" id="txtSponsorId2" /></li>
            <li>安　全　令：<input class="ipt" type="text" id="txtToken2" /></li> 
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="joinClub()" /></li>
        </ul> 
    </div> 
</asp:Content>