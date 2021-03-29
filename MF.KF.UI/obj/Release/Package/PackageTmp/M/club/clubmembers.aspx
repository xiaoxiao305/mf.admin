<%@ Page Title=" 俱乐部管理》俱乐部成员" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="clubmembers.aspx.cs" Inherits="MF.KF.UI.M.club.clubmembers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            if (parseInt($("#filed").val()) == 1 && $("#key").val().trim().substring(0, 3).toUpperCase() == "AAA") return;//2021-03-29 不能查询AAA账号 @赵凯
            var args = [parseInt($("#filed").val()), $("#key").val()];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getMembersList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                console.log("data.result:", data.result);
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
            addCell(tr,o.Id, 0);
            addCell(tr, o.Name, 1);
            //o.Founder临时存储当前查询用户chargeid
            addCell(tr, "<a href='javascript:void (0);' onclick=\"kickClubMembers('" + o.Founder+"','" + o.Id + "');\">踢出俱乐部</a>", 2);
            return tr;
        }
        function kickClubMembers(member_id, club_id) {
            if (confirm("你确定要踢出 " + member_id + "?")) {
                ajax.kickClubMembers("kickClubMembers", [member_id, club_id], winresult);
            }
        }
        $(document).ready(function() {
            var pagerTitles = ["ID","俱乐部","操作"];
            jsonPager.init(ajax.getMembersList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部成员列表</div>
    <div class="search">&nbsp;&nbsp;    
        <select id="filed">
            <option value="1">UID</option>
            <option value="2">账号</option>
        </select>
        <input type="text" id="key" class="box" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>