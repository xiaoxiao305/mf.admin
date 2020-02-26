<%@ Page Title="报表管理 》 欢乐卡报表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
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
            var args = ["happycard",startTime, overTime,$("#ddlchannel").val()];
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
        function insertRow(o,tr) {
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
            var date=new Date("2012/10/1")
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, o.LeftHappy == 0 ? "" : o.LeftHappy, 1);
            addCell(tr, o.ChargeHappy == 0 ? "" : o.ChargeHappy, 2);
            addCell(tr, o.AdminHappy == 0 ? "" : o.AdminHappy, 3);
            addCell(tr, o.GameUseHappy == 0 ? "" : o.GameUseHappy, 4);
            addCell(tr, o.GameReturnHappy == 0 ? "" : o.GameReturnHappy, 5);
            addCell(tr, getChannelInfo(o.ChannelId), 6);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist)
                $("#ddlchannel").append("<option value=\"" + id + "\">" + channellist[id] + "</option>");
            var pagerTitles = ["日期", "剩余数量", "充值赠送", "管理员发放","报名","游戏退赛","渠道"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">欢乐卡报表</div>
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

