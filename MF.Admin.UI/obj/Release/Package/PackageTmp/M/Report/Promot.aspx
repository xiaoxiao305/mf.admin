<%@ Page Title=" 报表管理 》 推广报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var channellist =  <%=channellist %>;
        function search() {
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
            $("#loading").show();
            var args = ["promot",startTime, overTime,$("#ddlchannel").val()];
            jsonPager.queryArgs = args;
            ajax.getReportList(jsonPager.makeArgs(1), searchResult);
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
            getChannelInfo = function(channelid) {
                if(channelid ==null || channelid == "")
                    return "";
                for (var id in channellist)
                {
                    if(channelid.toUpperCase() == id.toUpperCase())
                        return channellist[id];
                }
                return channelid;
            };
            var date = new Date("2012/10/1");
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            var arr = ["Day", "LoginNum", "TakeCurrency", "ActUserNum", "ActMatchsUserNum", "RegNum", "RegNum", "RegNum", "M_1Day", "M_3Day", "M_4Day", "M_5Day", "M_6Day", "M_7Day", "M_15Day", "M_30Day","GAP_15","ARPU","ARPPU","PayRate","ChargeUserRate"];
            for (var i = 0; i < arr.length; i++) {
                if (i == 0) continue;
                else if (i == 6)
                    addCell(tr, o.RegMatchsNum + "<br/>" + (o.RegNum - o.RegMatchsNum), i);
                else if (i == 7)
                    addCell(tr, (o.ActMatchsUserNum > 0 && o.TakeCurrency > 0) ? parseFloat(Math.floor((o.TakeCurrency * 100) / (o.ActMatchsUserNum * 10000)) / 100).toFixed(2) : "", i);
                else if (i == 8 || i == 9 || i == 10 || i == 11 || i == 12 || i == 13 || i == 14 || i == 15) {
                    if (o.RegNum < 1 || o.RegMatchsNum < 1) {
                        addCell(tr, "", i);
                    } else {
                        addCell(tr, Math.floor((o[arr[i]] / o.RegMatchsNum) * 10000) / 100 + "%<br/>"
                            + Math.floor((o["NN" + arr[i]] / (o.RegNum - o.RegMatchsNum)) * 10000) / 100 + "%", i);
                    }
                }
                 else if (i == 19 || i==20)
                    addCell(tr, o[arr[i]] > 0 ? o[arr[i]]+ "%" : "", i);
                 else
                    addCell(tr, o[arr[i]] > 0 ? o[arr[i]] : "", i);
            }
            addCell(tr, getChannelInfo(o.ChannelId), 21);
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist) {
                var checked = id.toUpperCase() == "10A"?" selected":"";
                $("#ddlchannel").append("<option value=\"" + id + "\" " + checked + ">" + channellist[id] + "</option>");
            }
            var pagerTitles = ["日期", "登录数量", "元宝消耗数量", "⽇活跃", "包间活跃", "新增数量", "新增用户活跃", "包间ARPU",
                "次日留存", "3日留存", "4日留存", "5日留存", "6日留存", "7日留存", "15日留存", "30日留存","回归(15天)",
                "ARPU", "ARPPU", "付费率(登录)", "付费率(活跃)", "渠道"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">推广报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="ddlchannel">
	                <option value="-1">所有渠道</option>
                </select>
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
