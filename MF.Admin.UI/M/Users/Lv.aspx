<%@ Page Title=" 用户管理》等级分布" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Lv.aspx.cs" Inherits="MF.Admin.UI.M.Users.Lv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            var start = 0;
            var over = 0;
            if ($("#start").val().trim() != "" || $("#end").val().trim() != "") {
                if ($("#start").val().trim() == "" || $("#end").val().trim() == "") {
                    alert("请输入要查询 " + $("#field1").find("option:selected").text() + " 的正确范围");
                    return;
                }
                start = parseInt($("#start").val());
                over = parseInt($("#end").val());
                if (over < start || start < 0) {
                    alert("请输入要查询 " + $("#field1").find("option:selected").text() + " 的正确范围");
                    return;
                }
            }
            var exact=0;
            if ($("#exact").is(":checked")) exact = 1;
            var checktime = 0;
            var timeType = 0;
            var startTime = 0;
            var overTime = 0;
            if ($("#time").is(":checked")) {
                checktime = 1;
                if ($("#radioLogin").is(":checked")) timeType = 1;
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                }
                startTime = new Date($("#starttime").val().replace("-", "/")).dateDiff("s");
                overTime = new Date($("#overtime").val().replace("-", "/") + " 23:59:59").dateDiff("s");
                if (overTime < startTime) {
                    alert("查询截止时间不能小于开始时间");
                    return;
                }
            }
            $("#loading").show();
            var args = [parseInt($("#flag").val()), parseInt($("#field1").val()), start, over, exact, parseInt($("#filed").val()), $("#key").val(), parseInt($("#order").val()), checktime, timeType, startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getUserList(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, '<a href="userinfo.aspx?id='+o.ID+'">'+o.Account+"</a>", i);
            for (var i = 1; i < jsonPager.title.length; i++)
                addCell(tr, "", i);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["等级", "用户数量", "AI数量", "15日内活跃", "30日内活跃", "60日内活跃", "90日内活跃", "游戏款数", "付费率"];
            jsonPager.init(ajax.getUserList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">用户列表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="flag">
	                <option value="-1">用户状态</option>
	                <option value="1">正常</option>
	                <option value="0">冻结</option>
                </select>
                <select  id="field1">
	                <option value="1">元宝</option>
	                <option value="2">登录次数</option>
	                <option value="3">在线时长</option>
                </select>
                <input  type="text" id="start" class="box w60" />至<input  type="text" id="end" class="box w60" />
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
	                <option value="1">主账号</option>
	                 <option value="2">子账号</option>
	                <option value="2">昵称</option>
                </select>
                <input  type="text" id="key" class="box" />
                <select id="order">
	                <option value="-1">排序方式</option>
	                <option value="1">注册时间</option>
	                <option value="2">登录时间</option>
	                <option value="3">累计充值</option>
	                <option value="4">元宝数量</option>
	                <option value="5">登录次数</option>
	                <option value="6">在线时长</option>
                </select>
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
                <div id="divTime" class="date" >
                    <input id="radioRegist" type="radio" value="0" checked="checked" name="timegroup" /><label for="radioRegist">创建时间</label>
                    <input id="radioLogin" type="radio" name="timegroup" value="1" /><label for="radioLogin">登录时间</label>&nbsp;&nbsp;&nbsp;&nbsp;
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </div>
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>