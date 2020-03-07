<%@ Page Title=" 游戏管理 》 待审核黑名单列表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="auditblacklist.aspx.cs" Inherits="MF.Admin.UI.M.game.auditblacklist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <style type="text/css">
       li {display:block; text-align:left;padding-left:10%;}
       .center{text-align:center;}
   </style>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript"> 
        var games =  <%=blackGameList %>;
        function search() {
            $("#loading").show();
            var gameid = parseInt($("#game").val());
            var field = parseInt($("#field").val());
            var args = [gameid, field, $("#account").val()];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getAuditBlackUsers(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                if (data.rowCount < 1) {
                    alert("没有待审核黑名单");
                }
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                if (i == 7)
                    td.style.width = "200px";
                td.innerHTML = text;
            };
            var date = new Date("2012/10/1");
            addCell(tr, date.dateAdd("s", o.CreateDate).format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, GetGameName(o.GameId), 1);
            addCell(tr, o.Account, 2);
            addCell(tr, "<a href='/M/currency/CurrencyRecord.aspx?chargeid=" + o.ChargeId + "' target='_blank'>" + o.ChargeId + "</a>", 3);
            addCell(tr, o.NickName, 4);
            addCell(tr, o.Value, 5);
            addCell(tr, getBlackLevelStr(o.Level), 6);
            if (o.Remark && o.Remark.length > 0 && o.Remark.indexOf('\'') >= 0) {
                o.Remark = o.Remark.replace(/\'/g, "");
            }
            addCell(tr, o.Remark, 7);
            addCell(tr, "<a href='javascript:;' onclick='updateBlackUser(" + JSON.stringify(o) + ")'>修改</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                "<a href='javascript:;' onclick='setWinMoney(" + JSON.stringify(o) + ")'>删除</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                "<a href='javascript:;' onclick='getGameMoney(" + JSON.stringify(o) + ")'>查看输赢值</a><br />" +
                "<a href='javascript:;' onclick='downloadLog(" + JSON.stringify(o) + ",2)'>下载录像</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                "<a href='javascript:;' onclick='confirmUser(" + JSON.stringify(o) + ")'>确认实锤</a><br/>", 8);
            return tr;
        }
        function confirmUser(o) {
            oprObjTmp = o;
            showAddMoneyWin(3);
            initValData(3, o.GameId, "");
            $("#lblgame3").text(GetGameName(o.GameId));
            $("#lblaccount3").text(o.Account);
            $("#lblnick3").text(o.NickName);
            $("#lblchargeid3").text(o.ChargeId);
            $("#txtRemark3").text(o.Remark);
            $("#msgtitle").text("实锤黑名单用户");
        }
        function confirmUserConfirm() {
            if (!oprObjTmp || oprObjTmp == null) {
                alert("操作有误");
                return;
            }
            var val = $('input:radio[name="gameval"]:checked').val();
            var levelStr = $('input:radio[name="gameval"]:checked').next("label").text();
            if ($('#inputVal3').is(':checked')) {
                val = "";
                if (token == "") {
                    $("#lblerr3").text("请输入安全令");
                    return;
                }
                if ($("#tval31").val() != "" && $("#tval32").val() != "")
                    val = "[" + $("#tval31").val() + "," + $("#tval32").val() + "]";
                //获取低中高的中文字符
                levelStr = "";
            }
            var gameid = oprObjTmp.GameId;
            var acc = oprObjTmp.Account;
            var chargeid = oprObjTmp.ChargeId;
            var remark = $("#txtRemark3").val();
            var confirmData = $("#txtConfirm3").val();
            if ($("#noneConfirm").is(':checked'))
                confirmData = "NONE_" + gameid;
            $("input:checkbox[name='chkGame']:checked")
            var token = $("#token3").val();
            if (gameid == "" || parseInt(gameid) < 1 || acc == "" || chargeid == "" || confirmData == "") {
                $("#lblerr3").text("参数错误");
                return;
            }
            else if (!val || val == "") {
                $("#lblerr3").text("请设置值");
                return;
            }
            ajax.confirmBlackUser("confirmblackuser", [gameid, acc, chargeid, val, levelStr, remark, confirmData, token], delwinresult);
        }
        $(document).ready(function () {
            var i = 0;
            for (var id in blackGameMap) {
                if (blackGameMap.hasOwnProperty(id)) {
                    $("#tgame4").append("<input type='checkbox' name='chkGame' id=" + id
                        + " value=" + id + " style='white-space:nowrap;'><label for='" + id + "' style='white-space:nowrap;'>" + blackGameMap[id].name + "</label>&nbsp;&nbsp;&nbsp;");
                    if ((i + 1) % 3 == 0) $("#tgame4").append("<br/>");
                    i++;
                }
            }
            $("#tgame4").append("<input type='checkbox' id='-1' value='-1' onclick='checkAll(this)'><label for='-1'>全选</label>");

            for (var id in games) {
                $("#game").append("<option value=\"" + id + "\">" + games[id] + "</option>");
            }
            var pagerTitles = ["添加时间", "游戏", "账号", "UID", "昵称", "值", "档位", "备注", "操作"];
            jsonPager.init(ajax.getAuditBlackUsers, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">待审核黑名单列表</div>
    <div class="search">&nbsp;&nbsp;
         <select id="game"><option value="-1">所有游戏</option></select>
        <select id="field"><option value="1">UID</option><option value="2">账号</option></select>
        <input type="text" id="account"/>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />　 
        <input type="button" value="添加黑名单" onclick="addBlackUser();" class="ui-button-icon-primary oprbutton"/> 
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
            <li>　　游　戏：<label id="lblgame"></label></li>
            <li>　　账　号：<label id="lblaccount"></label></li>
            <li>　　　UID:<label id="lblchargeid"></label></li>
            <li>　　　　值：<label id="valTypeArea"></label>
                <input type="checkbox" id="inputVal1" onchange="showInputVal(1)" />输入值
            </li>
            <li id="valLi1" style="display:none;">　　输入值：<input class="ipt" type="text" id="tval11" style="width:20%" />　至　<input class="ipt" type="text" id="tval12"  style="width:20%"/></li>
            <li style="height: 60px;">　　备　注：<textarea id="txtRemark" rows="5" cols="25"></textarea></li>
            <li id="tokenLi1" style="display:none;">安　全　令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red center" id="lblerr"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="updateUserConfirm()" /></li>
        </ul> 
        <ul id="T2">
             <li>　　游　戏：<label id="tgame2"></label></li>
            <li>　　　&nbsp;UID：<label id="tchargeid2"></label></li>
            <li>　　　　值：
                <input type='radio' value='0' name='gameval2' id="clearVal" /><label>清零</label>
                <input type='radio' value='' name='gameval2' id="keepVal" /><label>保留</label>
                <input type="checkbox" id="inputVal2" onchange="showInputVal(2)" />输入值
            </li>
            <li id="valLi2" style="display:none;">　　输入值：<input class="ipt" type="text" id="tval2"/></li>
            <li id="tokenLi2" style="display:none;">安　全　令：<input class="ipt" type="text" id="token2" /></li>
            <li class="err red center" id="lblerr2"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="setWinMoneyConfirm()" /></li>
        </ul> 
        <ul id="T3">
            <li>　　游　戏：<label id="lblgame3"></label></li>
            <li>　　账　号：<label id="lblaccount3"></label></li>
            <li>　　昵　称：<label id="lblnick3"></label></li>
            <li>　　　UID:<label id="lblchargeid3"></label></li>
            <li>　　　　值：<label id="valTypeArea3"></label></li>     
            <li>　　备　注：<textarea id="txtRemark3"></textarea></li>
               <li style="height: 40px;">　实锤数据：<textarea id="txtConfirm3" rows="3" cols="25"></textarea><input type="checkbox" id="noneConfirm"/>没有录像数据</li>
            <li class="err red center" id="lblerr3"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="confirmUserConfirm()" /></li>
        </ul> 
        <ul id="T4">
            <li style="height:100%;width: 100%;margin: 0;padding: 0;align-items:center; display: -webkit-flex;">　　游　戏：<p style="line-height: 30px;display:inline-block;width:70%;padding-left: 5%;border:1px solid #cccccc" id="tgame4"></p></li>
            <li>　　　UID：<input class="ipt" type="text" id="tacc4" /></li>
            <li>　　　　值：<label id="valTypeArea4"></label>
            </li>
            <li style="height: 70px;">　　备　注：<textarea id="txtRemark4" rows="4" cols="25"></textarea><input type="checkbox" id="isConfirm" style="margin-left:60px;"  />确认实锤</li>
            <li class="err red center" id="lblerr4"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="addBlackUserConfirm()" /></li>
        </ul> 
        <ul id="T5">
            <li>　　游　戏：<label id="lblgame5"></label></li>
            <li>　　账　号：<label id="lblaccount5"></label></li>
            <li>　　　UID:<label id="lblchargeid5"></label></li> 
            <li>　　输赢值：<label id="lblMoney5"></label></li>
            <li>　　备　注：<label id="lblRemark5"></label></li>
        </ul>
    </div> 
</asp:Content>