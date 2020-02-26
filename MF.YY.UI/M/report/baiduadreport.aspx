<%@ Page Title=" 报表管理 》 统计报表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="baiduadreport.aspx.cs" Inherits="MF.YY.UI.M.report.baiduadreport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var channelconst =  <%=channelDic %>;
        function search() {
            var checktime = $("#time").is(":checked") ? 1 : 0;
            if (checktime == 1) {
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                }
            }
            startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("d");
            overTime = new Date($("#overtime").val().replace(/-/g, "/")).dateDiff("d");
            if (overTime < startTime) {
                alert("查询截止时间不能小于开始时间");
                return;
            }
            $("#loading").show();
            var args = [parseInt($("#channel").val()), parseInt($("#device").val()), parseInt($("#gamelist").val()), checktime, startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getbaiduadReport(jsonPager.makeArgs(1), searchResult);
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
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            SelectChannel = function (channel) {
                var channelval = channelconst[channel];
                if (channelval && channelval != "undefined")
                    return channelval;
                else
                    return channel;
            };
            SelectDevice = function (device) {
                if (device == 3) return "PC";
                else if (device == 4) return "移动端";
                return "";
            };
            SelectGame = function (game) {
                if (game == 21) return "斗地主";
                else if (game == 22) return "棋牌";
                else if (game == 23) return "捕鱼";
                return "";
            };
            var date = new Date("2012/10/1");
            addCell(tr, date.dateAdd("d", o.day).format("yyyy-MM-dd"), 0);
            addCell(tr, SelectChannel(o.ADID.substring(0, 2)), 1);
            addCell(tr, SelectDevice(o.ADID.substring(2, 3)), 2);
            addCell(tr, SelectGame(o.ADID.substring(3, 5)), 3);
            addCell(tr, o.regNum, 4);
            addCell(tr, o.LoginNum,5);
            addCell(tr, o.ActUserNum,6);
            addCell(tr, o.OneDayLeft, 7);
            addCell(tr, o.ThreeDayLeft, 8);
            addCell(tr, o.SevenDayLeft, 9);
            addCell(tr, o.ADID, 10);
            return tr;
        }
        $(document).ready(function ()
        {
            var i = 0;
            for (var id in channelconst)
            {
                if (i == 0)
                    $("#channel").append("<option value=\"" + id + "\" selected=\"selected\">" + channelconst[id] + "</option>");
                else
                    $("#channel").append("<option value=\"" + id + "\">" + channelconst[id] + "</option>");
                i++;
            }
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["日期", "渠道", "设备", "游戏", "注册人数", "登录人数", "活跃人数", "次日留存", "3日留存", "7日留存", "ADID"];
            jsonPager.init(ajax.getbaiduadReport, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">渠道统计报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="channel">
	                <option value="-1">渠道</option>
                </select>
                <select id="device">
	                <option value="-1" selected="selected">所有设备</option>
                    <option value="3">PC</option>
                    <option value="4">移动端</option>
                </select>
                <select id="gamelist">
	                <option value="-1">所有游戏</option>
                    <option value="21">斗地主</option>
                    <option value="22">2255棋牌</option>
                    <option value="23">捕鱼</option>
                </select>
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                <span id="divTime" class="date" >
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </span>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
