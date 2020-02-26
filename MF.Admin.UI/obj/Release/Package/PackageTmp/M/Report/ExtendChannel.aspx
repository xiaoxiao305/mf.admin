<%@ Page  Title=" 报表管理 》 推广页面渠道报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="ExtendChannel.aspx.cs" Inherits="MF.Admin.UI.M.Report.ExtendChannel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function searchRedis() {
            $("#loading").show();
            var args = ["", "", 2, 0, 0];
            jsonPager.queryArgs = args;
            ajax.getExtendChannelRecord(jsonPager.makeArgs(1), searchResult);
        }
        function search() {
            var startTime = 0;
            var overTime = 0;
            var checktime = $("#time").is(":checked") ? 1 : 0;
            if (checktime == 1) {
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                }
                startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("d");
                overTime = new Date($("#overtime").val().replace(/-/g, "/")).dateDiff("d");
                if (overTime < startTime) {
                    alert("查询截止时间不能小于开始时间");
                    return;
                }
            }
            $("#loading").show();
            var args = [$("#channellist").val(), $("#channelnumlist").val(), checktime,startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getExtendChannelRecord(jsonPager.makeArgs(1), searchResult);
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
                if (i == 3 || i == 6 || i == 7) {
                    td.style.textAlign = "left";
                    td.style.padding = "10px";
                }
            };
            var date=new Date("2012/10/1")
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, o.Channel, 1);
            addCell(tr, o.ChannelNum, 2);
            addCell(tr, o.PCLoad + o.AndroidLoad + o.iOSLoad>0?o.PCLoad + o.AndroidLoad + o.iOSLoad:"", 3);
            addCell(tr, o.PCLoad>0?o.PCLoad:"", 4);
            addCell(tr, o.AndroidLoad>0?o.AndroidLoad:"", 5);
            addCell(tr, o.iOSLoad>0?o.iOSLoad:"", 6);
            addCell(tr, o.LoadTimeAvg>0?o.LoadTimeAvg:"", 7);
            addCell(tr, o.Stay>0?o.Stay:"", 8);
            addCell(tr, o.PCDown + o.AndroidDown + o.iOSDown>0?o.PCDown + o.AndroidDown + o.iOSDown:"", 9);
            addCell(tr, o.PCDown>0?o.PCDown:"", 10);
            addCell(tr, o.AndroidDown>0?o.AndroidDown:"", 11);
            addCell(tr, o.iOSDown>0?o.iOSDown:"", 12);
            addCell(tr, o.PCFirstActive + o.AndroidFirstActive + o.iOSFirstActive>0?o.PCFirstActive + o.AndroidFirstActive + o.iOSFirstActive:"", 13);
            addCell(tr, o.PCFirstActive>0?o.PCFirstActive:"", 14);
            addCell(tr, o.AndroidFirstActive>0?o.AndroidFirstActive:"", 15);
            addCell(tr, o.iOSFirstActive>0?o.iOSFirstActive:"", 16);
            addCell(tr, o.SecondDown>0?o.SecondDown:"", 17);
            addCell(tr, o.SecondDownTimeAvg>0?o.SecondDownTimeAvg/1000:"", 18);
            addCell(tr, o.Register>0?o.Register:"", 19);
            addCell(tr, o.NetWifi>0?o.NetWifi:"", 20);
            addCell(tr, o.NetMobileData>0?o.NetMobileData:"", 21);
            return tr;
        }
        $(document).ready(function () {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var c =  <%=channellistarray %>;
            var cn = <%=channelnumlistarray %>;
            for (var i = 0; i < c.length; i++) {
                $("#channellist").append("<option value=\"" + c[i] + "\">" + c[i] + "</option>");
            }
            for (var j = 0; j < cn.length; j++) {
                $("#channelnumlist").append("<option value=\"" + cn[j] + "\">" + cn[j] + "</option>");
            }
            var pagerTitles = ["日期", "渠道", "渠道码", "加载", "PC", "Android", "iOS", "加载平均耗时(ms)", "停留", "下载", "PC", "Android", "iOS", "激活", "PC", "Android", "iOS", "二次下载", "二次下载平均耗时(s)","注册","wifi","流量"];
            jsonPager.init(ajax.getExtendChannelRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">推广页面渠道报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="channellist" onchange="selectGame(this.value)">
	                <option value="">所有渠道</option>
                </select>
                <select id="channelnumlist">
	                <option value="">所有渠道码</option>
                </select>
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="历史数据查询" onclick="search()" class="ui-button-icon-primary" />　　　<input type="button" value="今日redis数据" onclick="searchRedis()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
