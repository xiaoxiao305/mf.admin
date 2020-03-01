<%@ Page Title=" 游戏管理 》 输赢值预警设置" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gamealertconfig.aspx.cs" Inherits="MF.Admin.UI.M.game.gamealertconfig" %>
<asp:Content ID="Content3" ContentPlaceHolderID="h" runat="server">
    <style type="text/css">
       li {display:block; text-align:left;padding-left:10%;}
       .center{text-align:center;}
   </style>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript">
        var oprGameType;
        var oprGameValue;
        function search() {
            $("#loading").show();
            jsonPager.queryArgs = [];
            jsonPager.pageSize = 1000;
            ajax.getRedAlert(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                var newResult = [];
                for (var key in data.result) {
                    newResult.push({ "type": key, "value": data.result[key] });
                }
                jsonPager.data = newResult;
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
            addCell(tr, GetGameNameByType(o.type), 0);
            addCell(tr, o.value, 1);
            addCell(tr, "<a href='javascript:;' onclick='delRedAlert(\"" + o.type + "\")'>删除</a>", 2);
            return tr;
        }
        function delRedAlert(gameType) {
            oprGameType = gameType;
            ajax.setRedAlert("delredalert", [gameType], delwinresult);
        }

        function delwinresult(res) {
            $("#loading").hide();
            if (res.code == 1) {
                alert("操作成功");
                var dataOld = jsonPager.data;
                var index = 0;
                for (var i = 0; i < dataOld.length; i++) {
                    if (dataOld[i]["type"].trim().toUpperCase() == oprGameType) {
                        index = i;
                        break;
                    }
                }
                dataOld.splice(index, 1);
                oprGameType = null;
                jsonPager.data = dataOld;
                jsonPager.dataBind(res.index, dataOld.length);
            } else {
                if (res.msg != "")
                    alert(res.msg);
                else
                    alert("操作失败：" + res.code);
            }
        }
        function setRedAlert() {
            showAddMoneyWin(1);
            $("#msgtitle").text("设置预警输赢值");
        }
        function setRedAlertConfirm() {
            var type = $("#game").val();
            var val = $("#value").val(); 
            if (!type || type == "") {
                $("#lblerr").text("请选择游戏");
                return;
            } else if (!val || val == "" || parseInt(val) < -1) {
                $("#lblerr").text("请输入设置值");
                return;
            }
            oprGameType = type;
            oprGameValue = val;
            ajax.setRedAlert("setredalert", [type, val], setwinresult);
        }
        function setwinresult(res) {
            $("#loading").hide();
            if (res.code == 1) {
                alert("操作成功");
                $('.theme-popover-mask').hide();
                $('.theme-popover').slideUp(200);
                var dataOld = jsonPager.data;
                dataOld.push({ "type": oprGameType, "value": oprGameValue });
                oprGameType = null;
                oprGameValue = null;
                jsonPager.data = dataOld;
                jsonPager.dataBind(res.index, dataOld.length);
            } else {
                if (res.msg != "")
                    alert(res.msg);
                else
                    alert("操作失败：" + res.code);
            }
        }
        $(document).ready(function () {
            for (var id in blackGameMap) {
                if (blackGameMap.hasOwnProperty(id)) {
                    $("#game").append("<option value=\"" + blackGameMap[id].type + "\">" + blackGameMap[id].name + "</option>");
                }
            }
            var pagerTitles = ["游戏", "值", "操作"];
            jsonPager.init(ajax.getRedAlert, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">输赢值预警设置</div>
    <div class="search">&nbsp;&nbsp;
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />　
        <input type="button" value="设置输赢值" onclick="setRedAlert()" class="ui-button-icon-primary oprbutton"/> 
    </div>
    <p></p>
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
            <li>　　游　戏：<select id="game"><option value="">请选择游戏</option></select></li>
            <li>　　输赢值：<input type="text" id="value"/></li> 
            <li class="err red center" id="lblerr"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="setRedAlertConfirm()" /></li>
        </ul> 
    </div> 
</asp:Content>
