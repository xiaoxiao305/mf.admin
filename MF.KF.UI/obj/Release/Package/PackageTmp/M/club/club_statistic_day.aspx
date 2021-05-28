﻿<%@ Page Title=" 俱乐部管理 》 俱乐部每日统计" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="club_statistic_day.aspx.cs" Inherits="MF.KF.UI.M.club.club_statistic_day" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <style type="text/css">
.box2
{
	 
	color:#3B6EA5;
	height:15px;
	width:60px;
	border:1px solid #BBDDE5;
	background-color:#FFFFFF;
	line-height:18px;
}
.add,.add:hover,.add:visited {
    width:25px;
    height:25px;  
    margin-left:0.5rem;
    margin-top:0.5rem;
    cursor:pointer; 
    border:none;
}  
.list_table th,td {width:16%;}
    </style>
    <script language="javascript" type="text/javascript">
        function search() {
            $("#loading").show();
            $("#container").show();
            $("#tbcon").hide(); 
            $(".tb").hide(); 
            var args = [$("#starttime").val(),$("#endtime").val(),-1];
            jsonPager.queryArgs = args; 
            ajax.getClubStatisticDay(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) { 
            $("#loading").hide(); 
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
                if (data.rowCount == 0) {
                    $("#tbcon").show();
                    $(".tr").hide();
                    $("#container").hide();
                } else { 
                    $("#container").show(); 
                    $("#tbcon").hide();
                }
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            }; 
            addCell(tr, o.date,0);
            addCell(tr, o.club_id + "<img src='/common/images/del.png' class='add' onclick='DelClub("+o.club_id+")'/>", 1);
            addCell(tr, o.clubname, 2);
            addCell(tr, o.round, 3);
            addCell(tr, o.online,4);
            addCell(tr, o.tax,5);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            attachCalenderbox('#endtime', '', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "俱乐部ID", "俱乐部名称", "总局数","成员上线数","总收益"]
            jsonPager.init(ajax.getClubStatisticDay,[],searchResult,pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1,0);
        });
        function SearchClub() {
            if ($("#txtclubid").val() == "")
                return;
            var clubid = parseInt($("#txtclubid").val());
            if (clubid <1)
                return; 
             var clubids = new Array; 
            clubids = $("#clubids").val().split(','); 
            if (clubids == "" || $("#clubids").val().indexOf(clubid) == -1) {
                clubids.push(clubid);
                $("#clubids").val(clubids); 
            }
            else
                return;
            var args = [$("#starttime").val(),$("#endtime").val(), parseInt(clubid)];
            jsonPager.queryArgs = args; 
            ajax.getClubStatisticDay(jsonPager.makeArgs(1), searchResult2);
            $("#txtclubid").val("");
        }
        function searchResult2(data) {
              $("#loading").hide(); 
            if (data.code == 1) {
                if (data.rowCount == 0) return;
                for (var i = 0; i < data.result.length; i++) {
                    var resObj = data.result[i];
                    if (resObj != null && resObj.club_id > 0) {
                        var con = "<tr class='tr'><td>" + resObj.date
                          + "</td><td>" + resObj.club_id
                          + "<img src='/common/images/del.png' class='add' onclick='DelClub(" + resObj.club_id
                          + ")'/></td><td>" + resObj.clubname + "</td><td>" + resObj.round
                            + "</td><td>" + resObj.online
                            + "</td><td>" + resObj.tax + "</td></tr>";
                      if ($("#container").is(':hidden') && !$("#tbcon").is(':hidden'))
                          $("#tbcon").append(con);
                      else if (!$("#container").is(':hidden') && $("#tbcon").is(':hidden'))
                          $("#container").append("<table class='list_table tb'>" + con + "</table>");
                  }
                }
            } else {
                alert(data.msg);
            }
        }
        function DelClub(clubid) {
            if (clubid < 1)
                return;
            var args = [parseInt(clubid)];
            jsonPager.queryArgs = args;
            ajax.DelClubStatisticClubId("delclubstatisticclubid", [parseInt(clubid)], search);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部统计数据</div>
    <div class="search">&nbsp;&nbsp; 
            统计时间<input type="text" id="starttime" class="box w100" readonly="readonly" />&nbsp;&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;
        <input type="text" id="endtime" class="box w100" readonly="readonly" />
            <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" id="btnsearch" /> 
    </div>
    <div id="container" style="display:none;"></div>
    <table class="list_table" style="border-bottom:none;" id="tbcon">
        <tr><th>日期</th><th>俱乐部ID</th><th>俱乐部名称</th><th>总局数</th><th>成员上线数</th><th>总收益</th></tr>
    </table>  
    <table class="list_table" style="border-top:none;">
        <tr><td></td><td>
            <input type="text"  class="box" id="txtclubid" />
            <img src="/common/images/ok.png" class="add" onclick="SearchClub()"/>
            <label style="color:#616161;"> &nbsp;&nbsp;&nbsp;&nbsp;输入俱乐部ID即可</label></td>
            <td></td><td></td><td></td><td></td></tr>
    </table>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
    <input type="hidden"  id="clubids"/>
</asp:Content>

