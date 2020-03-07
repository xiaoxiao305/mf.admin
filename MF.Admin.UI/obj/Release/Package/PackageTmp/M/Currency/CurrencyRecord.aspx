<%@ Page Title=" 游戏币管理 》 元宝记录" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="CurrencyRecord.aspx.cs" Inherits="MF.Admin.UI.M.Currency.CurrencyRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        var match= eval(<%=matchDic %>);
        var games =<%=gameDic %>;
        var gWin = 0;
        var dzWin = 0;
        var xData = [], yData = [], yDataMax = [],chartData =[];
        jsonPager.pageSize = 30;
        function search() {
             $.ajaxSettings.async=false;
            xData = []; yData =[]; yDataMax =[];chartData=[];
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
            var args = [parseInt($("#game").val()), parseInt($("#matchlist").val()),parseInt($("#type").val()),"<%=account%>","<%=chargeId%>", checktime, startTime, overTime,1];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getCurrencyRecord(jsonPager.makeArgs(1), searchResult);                        
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                gWin = 0;
                dzWin = 0;
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
                $("#gameWin").text(gWin);
                $("#dzWin").text(dzWin);
                drawLine();
            }else{
                alert(data.msg);
            }
        }
        function getTypeName(_type) {
             if (dicType[_type]) return dicType[_type];
                return _type; 
        }
        function getMatchName(gameid,matchid) {            
            var list = match[gameid]; 
            if(list ==null)
                return "";
            for (var i=0;i<list.length;i++){
                if(matchid== list[i][0]) 
                    return list[i][1];
            } 
            return "";
        };
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            if (o.Type == 6 || o.Type == 7) {
                chartData.unshift(o);
            }
            addCell(tr, new Date("2012/10/01").dateAdd("s",o.Time).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.Account, 1); 
            addCell(tr, games[o.GameId.toString()]?games[o.GameId.toString()]:"", 2);
            addCell(tr, getMatchName(o.GameId.toString(),o.MatchId.toString())?getMatchName(o.GameId.toString(),o.MatchId.toString()):"", 3);
            addCell(tr, getTypeName(o.Type), 4);
            addCell(tr, o.Num, 5);
            addCell(tr, o.Original, 6);
            addCell(tr, o.IP == "" ? "" : o.IP, 7);
            if (o.Type == 6 || o.Type == 7)
                countWin(o.GameId, o.Num, o.Type);
            return tr;
        }
        function getChartData() {
            for (var m = 0; m < chartData.length; m++) {
                //首条记录不为兑换战斗力OR 末尾记录不为退赛    舍弃
                if ((m == 0 && chartData[m].Type != 7)|| (m == chartData.length-1 && chartData[m].Type != 6))
                    continue;
                if (m == chartData.length - 1) continue;
                if (chartData[m].Type == 7) {
                    var x = "", y = 0;
                    //console.log("m[", chartData[m].Type + ":" + chartData[m].Num + "]", "m+1[", chartData[m + 1].Type + ":" + chartData[m + 1].Num + "]"
                    //," v:",(chartData[m + 1].Num - chartData[m].Num));
                    if (chartData[m + 1].Type == 6) {
                        x = new Date("2012/10/01").dateAdd("s", chartData[m + 1].Time).Format("MM-dd hh:mm:ss");
                        y = parseInt((chartData[m + 1].Num - chartData[m].Num) / 10000);
                    } else {
                        x = new Date("2012/10/01").dateAdd("s", chartData[m].Time).Format("MM-dd hh:mm:ss");
                        y = parseInt((0 - chartData[m].Num) / 10000);
                    }
                    if (y == 0 || y == -0) continue;
                    xData.push(x);
                    yData.push(y);
                    yDataMax.push(y);
                }
            }
            //for (var i = 0; i < yData.length; i++) {
            //    console.log("xdata[" + i + "]:", xData[i],"ydata["+i+"]:", yData[i]);
            //}
            //if (xDataAll.length < 1 || yDataAll.length < 1) return;
            //for (var i = 0; i < xDataAll.length; i++) {
            //    //console.log("xdata[" + i + "]:", xDataAll[i],"ydata["+i+"]:", yDataAll[i]);
            //    if (i == 0) continue;
            //    xData.push(xDataAll[i]);
            //    var preYData = yDataAll[i - 1];
            //    //console.log("preYData[" + (i - 1) + "]:", preYData," ",parseInt(preYData - yDataAll[i]));
            //    yData.push(parseInt(preYData - yDataAll[i]));
            //    yDataMax.push(yData[i]);
            //}
            //for (var j = 0; j < yDataMax.length; j++) {
            //    console.log("yDataMax["+j+"]:", yDataMax[j]);
            //}
            yDataMax = yDataMax.sort(compareYData);
            //for (var n = 0; n < yDataMax.length; n++) {
            //    console.log("yDataMax2["+n+"]:", yDataMax[n]);
            //} 
            //console.log("min:", yDataMax[0], " max:", yDataMax[yDataMax.length - 1]);
        }
        function compareYData(a,b){        
            if (a > b) { return 1; }
            else if (a < b) { return -1; }
            else {return 0;}
        }
        function countWin(gameid,num,type)
        {
            if (gameid == 52) {   
                if (type == 6)//-
                    dzWin -= num;
                else if (type == 7)//+
                    dzWin += num; 
            }
            else {
                if (type == 6)//+
                    gWin += num;  
                else if (type == 7)//-
                    gWin -= num;
            }
        }
        function selectGame(id){
            var list = match[id];
            $("#matchlist").empty();
            $("#matchlist").append("<option value=\"-1\">所有游戏场</option>");
            if(list ==null)
                return;
            for(var i =0;i<list.length;i++){
                $("#matchlist").append("<option value=\""+list[i][0]+"\">"+list[i][1]+"</option>");
            }
        } 
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null,new Date().Format("yyyy-MM-dd") , new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            for (var id in games)
                $("#game").append("<option value=\""+id+"\">"+games[id]+"</option>");
            var pagerTitles = ["时间","用户账号","游戏名称","场名称","变更类型","变更数量","原元宝数量","IP"];
            jsonPager.init(ajax.getCurrencyRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);            
            if ("<%=account%>" != "" || "<%=chargeId%>" != "")
                search();
        });
    </script>
     <script type="text/javascript" src="/common/js/echarts.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">查看用户元宝记录</div>
            <div class="search">&nbsp;&nbsp;
               <select id="game" onchange="selectGame(this.value)">
	                <option value="-1">所有游戏</option>
                </select>
                <select id="matchlist">
	                <option value="-1">所有游戏场</option>
                </select>
                <select id="type">
	                <option value="-1">变更类型</option>
	                <option value="1">定(即)时赛报名</option>
	                <option value="2">复活</option>
	                <option value="3">VIP每日赠送</option>
	                <option value="4">金券兑换</option>
	                <option value="5">获胜</option>
	                <option value="6">退赛</option>
	                <option value="7">兑换战斗力</option>
	                <option value="8">邮箱提取</option>
	                <option value="9">商城兑换</option>
	                <option value="10">注册赠送</option>
	                <option value="15">战斗力回兑</option>
	                <option value="16">领取救济</option>
	                <option value="18">存入保险箱</option>
	                <option value="19">保险箱取出</option>
	                <option value="22">绑定手机</option>
	                <option value="23">俱乐部任务</option>
	                <option value="24">购买房卡</option>
	                <option value="25">管理员发放</option>
	                <option value="26">银票兑换</option>
	                <option value="29">首充奖励</option>
                </select>
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                <span id="divTime" class="date" >
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </span>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
                <input type="button" value="增加元宝" onclick="showAddMoneyWin(1)" class="ui-button-icon-primary oprbutton"/>
                <span class="red">　　　　　普通游戏输赢：<label id="gameWin"></label>　　　　　德州游戏输赢：<label id="dzWin"></label></span>
            </div>
      <div id="lineChart" style="height: 400px;width:auto;background:white;margin:20px 0 0;"></div>
    <script type="text/javascript">
        function drawLine() { 
            getChartData();
            // 绘制图表，准备数据
            var lineChart = {
                dataZoom: {
                    type: 'inside',//slider
                    filterMode:'empty'
                    //orient:'horizontal'//vertical
                },
                toolbox: {
                    show: true,
                    feature: {
                        dataZoom: { yAxisIndex: 'none' }
                    }
                },
                title: {
                    text: ''
                },
                tooltip: {
                    trigger: 'axis'
                },
                xAxis: {
                    show: false,
                    type: 'category',
                    //data: ['16:18:55', '16:18:55', '16:18:55', '16:18:55', '16:18:55', '16:18:55', '16:18:51'],
                    //data: ['1', '2', '3', '4'],
                    data: xData,
                    axisLabel: {
                        interval: 0,
                    }
                },
                yAxis: {
                    type: 'value',
                    min: yDataMax[0],
                    max: yDataMax[yDataMax.length-1]
                },
                grid: {
                    left:100
                },
                series: [{
                    //data: [91776, 21460, 91260, 41403, 81403, 21276, 11856],
                    //data: [0, -127, 1943, 8],
                    data: yData,
                    type: 'line',
                    smooth: true
                }]
            };

            //初始化echarts实例
            var myLineChart = echarts.init(document.getElementById('lineChart'));

            //使用制定的配置项和数据显示图表
            myLineChart.setOption(lineChart);
        }
    </script>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
    <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">添加元宝</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li>元宝数量：<input class="ipt" type="text" id="num" /></li>
            <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addusermoney('<%=account %>',1)" /></li>
            <li class="err red">注：该元宝退还至用户邮件，请提醒用户领取。</li>
        </ul>
        <ul id="T2">
            <li class="red">请仔细核对如下信息，确认是否添加元宝</li>
            <li>　　　账&nbsp;&nbsp;&nbsp;&nbsp;号：<%=account %></li>
            <li>元宝数量：<span id="confirmnum"></span></li>                
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="confirmopr('<%=account %>',1)" /></li>
        </ul>
        <input type="hidden"  id="hidNum" />
        <input type="hidden"  id="hidToken" />
    </div>
</asp:Content>
