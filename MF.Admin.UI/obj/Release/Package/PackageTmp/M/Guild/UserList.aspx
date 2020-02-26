<%@ Page Title="公会管理》公会用户列表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="MF.Admin.UI.M.Guild.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
    var dicGuild = <%=guildMap %>;
        function search() {
            var exact = $("#exact").is(":checked") ? 1 : 0;
            var checktime = $("#time").is(":checked") ? 1 : 0;
            var startTime = 0;
            var overTime = 0;
            if (checktime == 1) {
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
            var args = [parseInt($("#guild").val()), exact, parseInt($("#filed").val()), $("#key").val(), checktime, startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getGuildUserList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, o.Account, 0);
            addCell(tr, o.Nickname, 1);
            addCell(tr, dicGuild[o.GuildID.toString()]?dicGuild[o.GuildID.toString()]:o.GuildID, 2);
            addCell(tr, new Date(o.JoinDate).format("yyyy-MM-dd hh:mm:ss"), 3);
            addCell(tr, new Date(o.LastOnLine).format("yyyy-MM-dd hh:mm:ss"), 4);
            addCell(tr, o.ActiveNum, 5);
            addCell(tr, o.LastWeekActiveDay, 6);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            for (var id in dicGuild)
                $("#guild").append("<option value=\""+id+"\">"+dicGuild[id]+"</option>");
            var pagerTitles = ["账号", "昵称", "公会名称", "加入时间", "最后在线", "活跃次数","上周活跃天数"];
            jsonPager.init(ajax.getGuildUserList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">公会用户列表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="guild">
	                <option value="-1">公会名称</option>
                </select>
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
	                <option value="1">玩家账号</option>
	                <option value="2">昵称</option>
                </select>
                <input  type="text" id="key" class="box" />
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">加入时间</label>
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