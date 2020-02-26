<%@ Page Title="报表管理 》 金豆报表" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
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
            var args = ["bean",startTime, overTime,$("#ddlchannel").val()];
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
            addCell(tr, o.ShopExchangeBean == 0 ? "" : o.ShopExchangeBean, 1);
            addCell(tr, o.TimingBean == 0 ? "" : o.TimingBean, 2);
            addCell(tr, o.AdminBean == 0 ? "" : o.AdminBean, 3);
            addCell(tr, o.TelephoneFareBean == 0 ? "" : o.TelephoneFareBean, 4);
            addCell(tr, o.LeftBean == 0 ? "" : o.LeftBean, 5);
            addCell(tr, getChannelInfo(o.ChannelId), 6);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist)
                $("#ddlchannel").append("<option value=\"" + id + "\">" + channellist[id] + "</option>");
            var pagerTitles = ["日期", "商城兑换", "定时赛发放", "管理员发放","话费赛产出","总金豆数","渠道"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">金豆报表</div>
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
