<%@ Page Title=" 报表管理 》 推广页面渠道关键字报表" MasterPageFile="~/M/main.Master"  Language="C#" AutoEventWireup="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">        
        function search() {
            var startTime = 0;
            var overTime = 0;
            var checktime = $("#time").is(":checked") ? 1 : 0;
            if (checktime == 1) {
                if ($("#starttime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                }
                startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("d");
            }
            $("#loading").show();
            var args = [checktime,startTime];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getExtendChannelKeywordRecord(jsonPager.makeArgs(1), searchResult);
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
        var types={ 101:"捕鱼平台",102:"街机捕鱼游戏",103:"新版捕鱼游戏",104:"街机捕鱼下载",105:"金蟾千炮捕鱼",
            106:"捕鱼游戏下载",107:"手机游戏捕鱼",108:"游戏大厅捕鱼",109:"最火的捕鱼游戏",110:"捕鱼送金币",
            111:"捕鱼网络游戏",112:"网络打鱼游戏",113:"游戏捕鱼游戏",114:"网上打鱼",115:"金蟾捕鱼",
            116:"好玩的捕鱼游戏",117:"街机打鱼游戏",118:"打鱼游戏网络版",119:"捕鱼街机游戏",120:"网络版捕鱼游戏",
            121:"网络捕鱼游戏",122:"打鱼游戏大全",123:"电玩捕鱼游戏",124:"游戏捕鱼",125:"手机捕鱼游戏下载",
            126:"捕鱼游戏大全",127:"打鱼游戏大厅",128:"网上捕鱼游戏",129:"千炮捕鱼游戏下载",130:"打鱼游戏网络",
            131:"网上捕鱼游戏平台",132:"下载游戏捕鱼",133:"在线捕鱼",134:"网络捕鱼游戏大厅",135:"捕鱼游戏哪个好玩",
            136:"千炮捕鱼",137:"捕鱼游戏网络版",138:"打鱼游戏下载",139:"捕鱼下载",140:"捕鱼游戏大厅",
            141:"1000捕鱼",142:"打鱼游戏平台",143:"捕鱼游戏游戏",144:"网上打鱼游戏",145:"手机捕鱼",
            146:"手机打鱼游戏",147:"捕鱼游戏平台",148:"打鱼游戏",149:"捕鱼游戏",
            301:"三人斗地主",302:"真人斗地主",303:"斗地主下载",304:"斗地主游戏",305:"斗地主在线玩",
            306:"下载斗地主",307:"手机斗地主",308:"斗地主"};
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };   
            var date=new Date("2012/10/1")
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, types[o.KeyWord]?types[o.KeyWord]:o.KeyWord, 1);
            addCell(tr, o.PCLoad + o.AndroidLoad + o.iOSLoad>0?o.PCLoad + o.AndroidLoad + o.iOSLoad:"", 2);
            addCell(tr, o.PCLoad>0?o.PCLoad:"", 3);
            addCell(tr, o.AndroidLoad>0?o.AndroidLoad:"", 4);
            addCell(tr, o.iOSLoad>0?o.iOSLoad:"", 5);
            addCell(tr, o.LoadTimeAvg>0?o.LoadTimeAvg:"", 6);
            addCell(tr, o.Stay>0?o.Stay:"", 7);
            addCell(tr, o.PCDown + o.AndroidDown + o.iOSDown>0?o.PCDown + o.AndroidDown + o.iOSDown:"", 8);
            addCell(tr, o.PCDown>0?o.PCDown:"", 9);
            addCell(tr, o.AndroidDown>0?o.AndroidDown:"", 10);
            addCell(tr, o.iOSDown>0?o.iOSDown:"", 11);
            return tr;
        }
        $(document).ready(function () {
            attachCalenderbox('#starttime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));           
            var pagerTitles = ["日期", "关键字","加载", "PC", "Android", "iOS", "加载平均耗时(ms)", "停留", "下载", "PC", "Android", "iOS"];
            jsonPager.init(ajax.getExtendChannelKeywordRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">推广页面渠道关键字报表</div>
            <div class="search">&nbsp;&nbsp;
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                查询日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                <input type="button" value="Redis数据" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
