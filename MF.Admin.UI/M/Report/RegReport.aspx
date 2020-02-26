<%@ Page  Title=" 报表管理 》 每日注册报表" MasterPageFile="~/M/main.Master"  Language="C#" AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="MF.Admin.UI.M.Report.Currency" %>
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
            var args = ["regist",startTime, overTime,$("#ddlchannel").val()];
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
            addCell(tr, o.NewUser > 0 ? o.NewUser : "", 1);
            addCell(tr, o.NewVisitor > 0 ? o.NewVisitor : "", 2);
            addCell(tr, o.ClientUser > 0 ? o.ClientUser : "", 3);
            addCell(tr, o.IphoneUser > 0 ? o.IphoneUser : "", 4);
            addCell(tr, o.AndroidUser > 0 ? o.AndroidUser : "", 5);
            addCell(tr, o.IpadUser > 0 ? o.IpadUser : "", 6);
            addCell(tr, o.TouristToUser > 0 ? o.TouristToUser : "", 7);
            addCell(tr, o.Relief > 0 ? o.Relief : "", 8);
            addCell(tr, o.SubAccTotal > 0 ? o.SubAccTotal : "", 9);
            addCell(tr, o.WeixinUser > 0 ? o.WeixinUser : "", 10);
            addCell(tr, o.AccTotal > 0 ? o.AccTotal : "", 11);
            addCell(tr, getChannelInfo(o.ChannelId), 12);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in channellist) {
                var checked = id.toUpperCase() == "10A"?" selected":"";
                $("#ddlchannel").append("<option value=\"" + id + "\" " + checked + ">" + channellist[id] + "</option>");
            }
            var pagerTitles = ["日期", "新增总数","游客总数", "客户端", "iOS", "Android", "iPad", "游客转正", "救济人数", "新增子账号","微信注册", "总用户数","渠道"];
            jsonPager.init(ajax.getReport, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">注册报表</div>
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
