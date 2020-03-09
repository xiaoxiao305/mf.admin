<%@ Page  Title=" 游戏币管理 》 二级密码记录" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="AllStrongBox.aspx.cs" Inherits="MF.Admin.UI.M.Currency.AllStrongBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var boxType = ["","创建", "存入", "取出", "销毁","找回"];
        function search() {
            var checktime = $("#time").is(":checked") ? 1 : 0;
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
            var chargeid = "", account = "", value = "";
            value = $("#value").val();
            if (value != "") {
                if ($("#field").val() == 1)
                    chargeid = value;
                else
                    account = value;
            } 
            var args = [parseInt($("#type").val()), checktime, startTime, overTime,chargeid,account];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getAllStrongBoxRecord(jsonPager.makeArgs(1), searchResult);
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
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            getTypeName = function(_type) { 
                if (dicType[_type]) return dicType[_type];
                return _type; 
            };
            addCell(tr, new Date("2012/10/01").dateAdd("s",o.Date).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.Account, 1); 
            addCell(tr, o.ChargeId, 2);
            addCell(tr, o.Currency, 3);
            addCell(tr, boxType[o.Type], 4);
            addCell(tr, o.BoxId, 5);
            addCell(tr, o.info == null ? "" : o.info.Identity, 6);
            addCell(tr, o.info == null ? "" :o.info.Name, 7);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', { minday: 2 },new Date().Format("yyyy-MM-dd") , new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["时间", "账号", "UID", "数量", "类型","保险箱号","身份证","姓名"];
            jsonPager.init(ajax.getAllStrongBoxRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            for (var id in boxType) {
                if (boxType[id] == "") continue;
                $("#type").append("<option value=\"" + id + "\">" + boxType[id] + "</option>");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">二级密码记录</div>
            <div class="search">&nbsp;&nbsp;
                <select id="type">
	                <option value="-1">变更类型</option>
                </select>
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                <span id="divTime" class="date" >
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </span>
                <select id="field">
                    <option value="1">UID</option>
                    <option value="2">账号</option>
                </select>
                <input type="text" id="value" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
