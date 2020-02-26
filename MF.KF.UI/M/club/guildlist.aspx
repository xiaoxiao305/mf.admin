<%@ Page Title=" 俱乐部管理》俱乐部列表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="guildlist.aspx.cs" Inherits="MF.KF.UI.M.club.guildlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            var club_id = "<%=club_ids%>"; 
            if (club_id!="") $("#key").val(club_id);
            if ($("#filed").val() == "" || $("#key").val() == "") return;
            var args = [parseInt($("#filed").val()), $("#key").val()];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getGuildList(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, o.Members_Count, 3);
            addCell(tr, o.Room_Card, 4);
            addCell(tr, o.Founder,5);
            addCell(tr, o.Create_Date, 6); 
            addCell(tr, o.Status==1?"生效":"失效", 7);
            addCell(tr, o.Type == 1 ? "俱乐部" : o.Type == 2 ? "俱乐部" : o.Type == 3 ? "积分俱乐部" : o.Type == 4 ? "金币俱乐部" : "", 8);
            if(<%=isAdmin%>)
                addCell(tr, "<a href='javascript:void (0);' onclick=\"verifyGuildInfo(" + o.Id + ",'" + o.Name + "');\">俱乐部审核</a>     <a href='javascript:void (0);' onclick=\"deleteClub(" + o.Id + ");\">解散俱乐部</a>", 9);
            else
                addCell(tr, "",9);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["选择","Id", "名称", "人数", "房卡数量","创建者", "创建时间", "状态","类型","操作"];
            jsonPager.init(ajax.getGuildList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            search();
        });
        function deleteClub(id) {
            if (confirm("确认要删除俱乐部【"+id+"】操作？")) {
                ajax.verifyGuildStatus("verifyguildstatus", [id,10000], winresult);
            }
        }
        function addClubRoomCard(id,name)
        {
            $('#hidClubId').val(id);
            $('#lblName1').text(name);
            $('#lblName2').text(name);
            showAddMoneyWin(1);
        }
        function SetRecomClubsWin()
        {
            if ($(":checkbox").length < 1) {
                alert("请查询数据，并选择需要设置的推荐俱乐部。");
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
                alert("请选择需要设置的推荐俱乐部。");
                return;
            }
            showAddMoneyWin(3);
        }
        function SetRecomClubs()
        {
            //if ($("#token3").val() == "") {
            //    $("#lblerr3").html("请输入安全令");
            //    return;
            //}
            if ($(":checkbox").length < 1)
            {
                $("#lblerr3").html("请查询数据，并选择需要设置的推荐俱乐部。");
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
            if (recoms == null || recoms.length < 1)
            {
                $("#lblerr3").html("请选择需要设置的推荐俱乐部。");
                return;
            }
            ajax.setRecomClubs("setrecomclubs", [recoms, 1], winresult);
        }
        function verifyGuildInfo(id, name) {
            $('#lblClubId4').html(id);
            $('#lblName4').html(name);
            showAddMoneyWin(4);
        }
        function verifyGuildStatus()
        {
            //if ($("#token4").val() == "") {
            //    $("#lblerr4").html("请输入安全令");
            //    return;
            //}
            ajax.verifyGuildStatus("verifyguildstatus", [parseInt($("#lblClubId4").html()), parseInt($("#status").val())], winresult);
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部列表</div>
    <div class="search">&nbsp;&nbsp;
        <select id="filed">
            <option value="3">俱乐部Id</option>
	        <option value="1">俱乐部名称</option>
	        <option value="2">俱乐部创建者</option>
        </select>
        <input  type="text" id="key" class="box"/> 
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <%--<input type="button" value="设置推荐俱乐部" onclick="SetRecomClubsWin()" class="ui-button-icon-primary oprbutton"/>--%>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
    <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">添加俱乐部房卡</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li style="text-align:left;">　　　俱乐部名称：<label id="lblName1"></label></li>
            <li>俱乐部房卡数量：<input class="ipt" type="text" id="num" /></li>
            <li>&nbsp;　　　安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addusermoney($('#hidClubId').val(),4)" /></li>
            <li class="err red">注：该房卡直接派发至该俱乐部账上。</li>
        </ul>
        <ul id="T2">
            <li class="red">请仔细核对如下信息，确认是否添加俱乐部房卡</li>
            <li>　　　　俱乐部名称：<label id="lblName2"></label></li>
            <li>俱乐部房卡数量：<span id="confirmnum"></span></li>                
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="confirmopr(parseInt($('#hidClubId').val()), 4)" /></li>
        </ul>
        <ul id="T3" style="width:88%;" class="hide">
            <%--<li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token3" /></li>--%>
            <li class="err red" id="lblerr3"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="SetRecomClubs()" /></li>
        </ul> 
        <ul id="T4" style="width:88%;" class="hide">
            <li style="text-align:left;">　　　俱乐部Id：<label id="lblClubId4"></label></li>
            <li style="text-align:left;">　　　俱乐部名称：<label id="lblName4"></label></li>
            <li style="text-align:left;">　　　俱乐部状态：<select id="status"><option value="1">生效</option><option value="0">失效</option></select></li>
            <%--<li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token4" /></li>--%>
            <li class="err red" id="lblerr4"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="verifyGuildStatus()" /></li>
        </ul> 
        <input type="hidden"  id="hidNum" />
        <input type="hidden"  id="hidToken" />
        <input type="hidden"  id="hidClubId" />
    </div>
</asp:Content>