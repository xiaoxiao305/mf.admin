<%@ Page Title=" 俱乐部管理》成员活跃度" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript">
        jsonPager.pageSize = 1000;
        function dateFormat(fmt, date) {
    let ret;
    let opt = {
        "Y+": date.getFullYear().toString(),        // 年
        "m+": (date.getMonth() + 1).toString(),     // 月
        "d+": date.getDate().toString(),            // 日
        "H+": date.getHours().toString(),           // 时
        "M+": date.getMinutes().toString(),         // 分
        "S+": date.getSeconds().toString()          // 秒
        // 有其他格式化字符需求可以继续添加，必须转化成字符串
    };
    for (let k in opt) {
        ret = new RegExp("(" + k + ")").exec(fmt);
        if (ret) {
            fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
        };
    };
    return fmt;
}
        function search() {
            var club_id = $("#club_id").val();         
            var day = dateFormat("YYYYmmdd",new Date($("#starttime").val()))
            if (club_id == "" || day=="") {return; }
            var args = [club_id, day];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getMemberActive(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, o.member_id, 0);
            addCell(tr, o.nick_name, 1);
            addCell(tr, o.active, 2);
            return tr;
        }
        $(document).ready(function () { 
            attachCalenderbox('#starttime', '', null, new Date().Format("yyyy-MM-dd"), '');
            var pagerTitles = ["Id","昵称", "活跃度"];
            jsonPager.init(ajax.getMemberActive, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">活跃度详情</div>
    <div class="search">&nbsp;&nbsp;
       俱乐部ID<input type="text" id="club_id" class="box" />
       <input type="text" id="starttime" class="box w100" readonly="readonly" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div> 
</asp:Content>