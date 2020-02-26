<%@ Page Title=" 游戏币管理 》 二级密码记录" Language="C#" AutoEventWireup="true"  MasterPageFile="~/M/main.Master" CodeBehind="StrongBox.aspx.cs" Inherits="MF.Admin.UI.M.Currency.StrongBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var dicType={1:"定(即)时赛报名",2:"复活",3:"VIP每日赠送",4:"金券兑换", 5:"定(即)时赛获胜", 6:"定(即)时赛退赛",7:"兑换战斗力", 8:"邮箱提取", 9:"商城兑换",10:"注册赠送",15:"战斗力回兑",
                        16:"领取救济",18:"存入保险箱",19:"保险箱取出",21:"开红包" ,22:"绑定手机",23:"完成俱乐部任务",24:"购买房卡",25:"系统赠送",26:"银票兑换",27:"管理员派发",28:"主/子账号互转"};
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
            var args = [parseInt($("#type").val()),"<%=account%>", checktime, startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getStrongBoxRecord(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, new Date("2012/10/01").dateAdd("s",o.Time).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.Account, 1); 
            addCell(tr, getTypeName(o.Type), 2);
            addCell(tr, o.Num, 3);
            addCell(tr, o.Original, 4);
            addCell(tr, o.IP.split(":")[0], 5);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null,new Date().Format("yyyy-MM-dd") , new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["时间","用户账号","变更类型","变更数量","原元宝数量","IP"];
            jsonPager.init(ajax.getStrongBoxRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if("<%=account%>" != "")
                search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">查看用户二级密码记录</div>
            <div class="search">&nbsp;&nbsp;
                <select id="type">
	                <option value="-1">变更类型</option>
	                <option value="18">存入保险箱</option>
	                <option value="19">保险箱取出</option>
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
