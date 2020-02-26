<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecomClubsList.aspx.cs" Inherits="MF.Admin.UI.M.Guild.RecomClubsList" MasterPageFile="~/M/main.Master" Title="俱乐部管理》推荐俱乐部列表"%>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
         function search() {
             var args = [];
             jsonPager.queryArgs = args;
             $("#loading").show();
             ajax.getRecommondClubsList(jsonPager.makeArgs(1), searchResult);
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
             addCell = function (tr, text, i) {
                 var td = tr.insertCell(i);
                 td.innerHTML = text;
             };
             addCell(tr, "<input type='checkbox' value='" + o.Id + "'>", 0);
             addCell(tr, o.Id, 1);
             addCell(tr, o.Name, 2);
             addCell(tr, o.Members_Count, 3);
             addCell(tr, o.Room_Card, 4);
             addCell(tr, o.Founder, 5);
             addCell(tr, o.Create_Date, 6);
             addCell(tr, o.Status, 7);
             return tr;
         }
         $(document).ready(function () {
             var pagerTitles = ["选择","Id", "名称", "人数", "房卡数量", "创建者", "创建时间", "状态"];
             jsonPager.init(ajax.getRecommondClubsList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
             jsonPager.dataBind(1, 0);
             search();
         });
         function DelRecomClubsWin()
         {
             if ($(":checkbox").length < 1) {
                 alert("请查询数据，并选择需要删除的推荐俱乐部。");
                 return;
             }
             var recoms = new Array();
             var i = 0;
             $(":checkbox").each(function () {
                 if ($(this).is(":checked")) {
                     recoms[i] = $(this).val();
                 }
                 i++;
             });
             if (recoms == null || recoms.length < 1) {
                 alert("请选择需要删除的推荐俱乐部。");
                 return;
             }
             showAddMoneyWin(1);
         }
         function DelRecomClubs() {
             //if ($("#token").val() == "")
             //{
             //    $("#lblerr").html("请输入安全令");
             //    return;
             //}
             if ($(":checkbox").length < 1) {
                 $("#lblerr").html("请查询数据，并选择需要删除的推荐俱乐部。");
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
                 $("#lblerr").html("请选择需要删除的推荐俱乐部。");
                 return;
             }
             ajax.setRecomClubs("setrecomclubs", [recoms, 2], winresult);
         }
         
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">推荐俱乐部列表</div>
    <div class="search">&nbsp;&nbsp;
        <%--<select id="filed">
            <option value="3">俱乐部Id</option>
	        <option value="1">俱乐部名称</option>
	        <option value="2">俱乐部创建者</option>
        </select>
        <input  type="text" id="key" class="box" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />--%>
        <input type="button" value="删除推荐俱乐部" onclick="DelRecomClubsWin()" class="ui-button-icon-primary oprbutton"/>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
      <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">删除推荐俱乐部</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <ul style="width:88%;" class="hide" id="T1">
    <%--    <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>--%>
        <li class="err red" id="lblerr"></li>
        <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="DelRecomClubs()" /></li>
    </ul> 
    <!--弹出窗口结束-->
</asp:Content>