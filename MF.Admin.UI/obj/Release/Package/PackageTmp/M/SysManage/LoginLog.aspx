<%@ Page Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="LoginLog.aspx.cs" Inherits="MF.Admin.UI.M.SysManage.LoginLog" Title="系统管理 》 登录日志列表"%>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            var exact = $("#exact").is(":checked")?1:0;
            var checktime = $("#time").is(":checked")?1:0;
            var startTime = 0;
            var overTime = 0;
            if (checktime==1) {
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                }
                startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("s");
                overTime = new Date($("#overtime").val().replace(/-/g, "/") + " 23:59:59").dateDiff("s");
                if (overTime < startTime) {
                    alert("查询截止时间不能小于开始时间");
                    return;
                }
            }
            var args = [parseInt($("#flag").val()), exact, parseInt($("#filed").val()), $("#key").val(), checktime, startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getLoginLogRecord(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index,data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, o.Account, 0);
            addCell(tr,o.LoginTime, 1);
            addCell(tr, o.IP, 2);
            addCell(tr, o.LoginState == 1 ? "成功" : "失败", 3);
            return tr;
        } 
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["账号", "登录时间", "IP", "状态"];
            jsonPager.init(ajax.getLoginLogRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">登录日志列表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="flag">
	                <option value="-1">登录状态</option>
	                <option value="1">成功</option>
	                <option value="0">失败</option>
                </select>
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
	                <option value="1">账号</option>
	                <option value="2">IP</option>
                </select>
                <input  type="text" id="key" class="box" />
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">登录时间</label>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
                <div id="divTime" class="date" >
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </div>
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>