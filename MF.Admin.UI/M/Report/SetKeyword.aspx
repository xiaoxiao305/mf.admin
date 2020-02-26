<%@ Page Title=" 报表管理 》 设置游戏关键字" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function SetKeyword(id,state)
        {
            ajax.setGameKeyword("setgamekeyword", [id, state], winresult);
        }
         function search() {
            $("#loading").show();
            jsonPager.queryArgs = [];
            jsonPager.pageSize = 1000;
            ajax.getKeywordsList(jsonPager.makeArgs(1), searchResult);
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
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, o.id, 0);
            addCell(tr, o.name, 1);
            if(o.state == 1)
                addCell(tr, "<a href='javascript:;' onclick='SetKeyword("+o.id+",2)'>停用</a>", 2);
            else if (o.state == 2)
                addCell(tr, "<a href='javascript:;' onclick='SetKeyword("+o.id+",1)'>启用</a>", 2);
            else
                addCell(tr,o.state, 2);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["编号", "关键字","操作"];
            jsonPager.init(ajax.getKeywordsList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">关键字列表</div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>
