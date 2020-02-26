<%@ Page Title=" 报表管理 》 元宝报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
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
            var args = ["currency",startTime, overTime,$("#ddlchannel").val()];
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
            addCell(tr, o.LeftCurrency == 0 ? "" : o.LeftCurrency, 1);
            addCell(tr, o.TakeCurrency == 0 ? "" : o.TakeCurrency, 2);
            addCell(tr, o.ChargeCurrency == 0 ? "" : o.ChargeCurrency, 3); 
            var v = o.LastLeftCurrency + o.ChargeCurrency - o.LeftCurrency;
            addCell(tr, v == 0 ? "" : v, 4);
            addCell(tr, o.StrongBoxCurrency == 0 ? "" : o.StrongBoxCurrency, 5); 
            addCell(tr, o.ReliefCurrency == 0 ? "" : o.ReliefCurrency, 6);
            addCell(tr, o.SystemDeliveryCurrency == 0 ? "" : o.SystemDeliveryCurrency, 7);
            addCell(tr, o.FreeGameCurrency == 0 ? "" : o.FreeGameCurrency, 8);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist) {
                var checked = id.toUpperCase() == "10A"?" selected":"";
                $("#ddlchannel").append("<option value=\"" + id + "\" "+checked+">" + channellist[id] + "</option>");
            }
            var pagerTitles = ["日期", "剩余数量", "抽水数量","充值数量","货币消耗数量", "保险箱总额", "救济", "系统赠送", "免费比赛"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">元宝报表</div>
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
