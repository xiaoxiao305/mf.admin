<%@ Page  Title=" 俱乐部管理》俱乐部收益" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="clubtax.aspx.cs" Inherits="MF.KF.UI.M.club.clubtax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            var club_id = $("#club_id").val();
            var s = new Date($("#time").val().replace(/-/g, "/")).dateDiff2("s");
            if (club_id == "") return; 
            var args = [club_id, s];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getClubTaxList(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, o.TaxTime.replace(/^(\d{4})(\d{2})(\d{2})$/, "$1-$2-$3"), 0);
            addCell(tr, o.Id, 1);
            addCell(tr, o.Name,2);
            addCell(tr, o.Tax/10000, 3); 
            addCell(tr, o.Tax_Round / 10000, 4); 
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["时间", "Id", "名称","自由局税", "固定局税"];
            jsonPager.init(ajax.getClubTaxList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            attachCalenderbox('#time', null, null, new Date().Format("yyyy-MM-dd"), null);
        }); 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部收益</div>
    <div class="search">&nbsp;&nbsp;
        俱乐部Id
        <input  type="text" id="club_id" class="box"/> 
        截止<input type="text" id="time" class="box w100" readonly="readonly" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>