<%@ Page Title=" 报表管理 》 充值报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
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
            var args = ["charge",startTime, overTime,$("#ddlchannel").val()];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
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
            addCell(tr, o.SubmitNum > 0 ? o.SubmitNum : "", 1);
            addCell(tr, o.SubmitMoney > 0 ? o.SubmitMoney : "", 2);
            addCell(tr, o.PayNum > 0 ? o.PayNum : "", 3);
            addCell(tr, o.AlipayPayMoney > 0 ? o.AlipayPayMoney : "", 4);
            addCell(tr, o.WeixinPayMoney > 0 ? o.WeixinPayMoney : "", 5);
            addCell(tr, o.IosPayMoney > 0 ? o.IosPayMoney : "", 6);
            addCell(tr, o.MaxPayMoney > 0 ? o.MaxPayMoney : "", 7);
            addCell(tr, o.PayMoney > 0 ? o.PayMoney : "", 8);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist) {
                var checked = id.toUpperCase() == "10A"?" selected":"";
                $("#ddlchannel").append("<option value=\"" + id + "\" " + checked + ">" + channellist[id] + "</option>");
            }
            var pagerTitles = ["日期", "提交笔数", "提交总额", "支付笔数", "支付宝(元)", "微信(元)", "苹果(元)", "易支付（元）", "支付总额(元)"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">充值报表</div>
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
