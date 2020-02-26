<%@ Page  Title="报表管理 》 商城兑换统计报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var channellist =  <%=channellist %>;
        function search() {
            var startTime = 0;
            var overTime = 0;
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
            var args = [parseInt($("#type").val()),checktime, startTime, overTime,$("#ddlchannel").val()];
            jsonPager.queryArgs = args;
            ajax.getExchangeReportList(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, o.KindSum == null ? "" : o.KindSum, 1);
            addCell(tr, o.TenMobile == null ? "" : o.TenMobile, 2);
            addCell(tr, o.TwentyMobile == null ? "" : o.TwentyMobile, 3);
            addCell(tr, o.FiftyMobile == null ? "" : o.FiftyMobile, 4);
            addCell(tr, o.HundredMobile == null ? "" : o.HundredMobile, 5);
            addCell(tr, o.FiveCurrency == null ? "" : o.FiveCurrency, 6);
            addCell(tr, o.TenCurrency == null ? "" : o.TenCurrency, 7);
            addCell(tr, o.TwentyCurrency == null ? "" : o.TwentyCurrency, 8);
            addCell(tr, o.FiftyCurrency == null ? "" : o.FiftyCurrency, 9);
            addCell(tr, o.HundredCurrency == null ? "" : o.HundredCurrency, 10);
            addCell(tr, o.TwoHundredCurrency == null ? "" : o.TwoHundredCurrency, 11);
            addCell(tr, o.TwentyRoomCard == null ? "" : o.TwentyRoomCard, 12);
            addCell(tr, o.HundredRoomCard == null ? "" : o.HundredRoomCard, 13);
            addCell(tr, o.TenJingdong == null ? "" : o.TenJingdong, 14);
            addCell(tr, o.FiftyJingdong == null ? "" : o.FiftyJingdong, 15);
            addCell(tr, o.HundredJingdong == null ? "" : o.HundredJingdong, 16);
            addCell(tr, o.TwoHundredJingdong == null ? "" : o.TwoHundredJingdong, 17);
            addCell(tr, o.FiveHundredJingdong == null ? "" : o.FiveHundredJingdong, 18);
            addCell(tr, o.EightHundredJingdong == null ? "" : o.EightHundredJingdong, 19);
            addCell(tr, o.EightHundredJingdong == null ? "" : o.EightHundredJingdong, 19);
            addCell(tr, getChannelInfo(o.ChannelId), 20);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist)
                $("#ddlchannel").append("<option value=\"" + id + "\">" + channellist[id] + "</option>");
            showTimeBox($$("time"));
            var pagerTitles = ["日期", "实物合计", "10元话费", "20元话费", "50元话费", "100元话费", "5万元宝", "10万元宝", "20万元宝", "50万元宝", "100万元宝", "200万元宝", "20张房卡", "100张房卡", "10元京东卡", "50元京东卡", "100元京东卡", "200元京东卡", "500元京东卡", "800元京东卡","渠道"];
            jsonPager.init(ajax.getExchangeReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">商城兑换统计报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="ddlchannel">
	                <option value="-1">所有渠道</option>
                </select>
                <select id="type">
	                <option value="-1">所有类型</option>
                    <option value="1">10元话费</option>
                    <option value="2">20元话费</option>
                    <option value="3">50元话费</option>
                    <option value="4">100元话费</option>
                    <option value="5">5万元宝</option>
                    <option value="6">10万元宝</option>
                    <option value="7">20万元宝</option>
                    <option value="8">50万元宝</option>
                    <option value="9">100万元宝</option>
                    <option value="10">200万元宝</option>
                    <option value="11">20张房卡</option>
                    <option value="12">100张房卡</option>
                    <option value="13">10元京东卡</option>
                    <option value="14">50元京东卡</option>
                    <option value="15">100元京东卡</option>
                    <option value="16">200元京东卡</option>
                    <option value="17">500元京东卡</option>
                    <option value="18">800元京东卡</option>
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
