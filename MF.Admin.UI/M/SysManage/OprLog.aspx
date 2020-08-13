﻿<%@ Page Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="OprLog.aspx.cs" Inherits="MF.Admin.UI.M.SysManage.OprLog"  Title="系统管理 》 操作日志列表"%>
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
            var args = [parseInt($("#type").val()), parseInt($("#flag").val()), exact, parseInt($("#filed").val()), $("#key").val(), checktime, startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getSystemLogRecord(jsonPager.makeArgs(1), searchResult);
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
        var typearr = ["", "修改密码", "解绑手机号", "解绑安全令", "解除本机锁定", "解除安全令锁定", "冻结账号", "解冻账号", "充值补单", "设置俱乐部活跃", "推荐俱乐部", "处理保证金", "加元宝", "加金豆", "加用户房卡", "加俱乐部房卡",
            "开启充值活动", "关闭充值活动", "添加游戏黑名单", "删除游戏黑名单", "设置输赢值",
            "修改游戏黑名单", "黑名单确认实锤", "设置输赢值警报配置", "删除输赢值警报配置"
            , "设置游戏系统广播", "踢出俱乐部成员", "退出俱乐部联盟", "设置俱乐部状态", "添加高税俱乐部", "删除高税俱乐部", "添加禁止同桌游戏ID","删除禁止同桌游戏ID"];
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, o.Account, 0);
            addCell(tr, typearr[o.Type], 1);
            addCell(tr, o.OprState == 1 ? "成功" : "失败", 2);
            addCell(tr, o.IP, 3);
            addCell(tr, o.Operation, 4);
            addCell(tr, new Date(o.OperTime).Format("yyyy-MM-dd hh:mm:ss"), 5);
            return tr;
        } 
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["账号", "模块", "状态", "IP", "操作信息","操作时间"];
            jsonPager.init(ajax.getSystemLogRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">操作日志列表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="type">
	                <option value="-1">功能</option>
	                <option value="1">修改密码</option>
	                <option value="2">解绑手机号</option>
	                <option value="3">解绑安全令</option>
	                <option value="4">解除本机锁定</option>
	                <option value="5">解除安全令锁定</option>
	                <option value="6">冻结账号</option>
	                <option value="7">解冻账号</option>
	                <option value="8">充值补单</option>
	                <option value="9">设置俱乐部活跃</option>
                    <option value="10">推荐俱乐部</option>
	                <option value="11">处理保证金</option>
	                <option value="12">加元宝</option>
	                <option value="13">加金豆</option>
	                <option value="14">加用户房卡</option>
	                <option value="15">加俱乐部房卡</option>
                    <option value="16">开启充值活动</option>
                    <option value="17">关闭充值活动</option>
                    <option value="18">添加游戏黑名单</option>
                    <option value="19">删除游戏黑名单</option>
                    <option value="20">设置输赢值</option>
                    <option value="21">修改游戏黑名单</option>
                    <option value="22">黑名单确认实锤</option>
                    <option value="23">设置输赢值警报配置</option>
                    <option value="24">删除输赢值异常警报</option>
                    <option value="25">设置游戏系统广播</option>
                    <option value="26">踢出俱乐部成员</option>
                    <option value="27">退出俱乐部联盟</option>
                    <option value="28">设置俱乐部状态</option>
                    <option value="29">添加高税俱乐部</option>
                    <option value="30">删除高税俱乐部</option>
                    <option value="31">添加禁止同桌游戏ID</option>
                    <option value="32">删除禁止同桌游戏ID</option>
                </select>
                <select id="flag">
	                <option value="-1">操作状态</option>
	                <option value="1">成功</option>
	                <option value="0">失败</option>
                </select>
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
	                <option value="1">账号</option>
	                <option value="2">IP</option>
                </select>
                <input  type="text" id="key" class="box" />
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">操作时间</label>
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