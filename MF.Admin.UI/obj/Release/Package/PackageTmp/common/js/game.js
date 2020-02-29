var blackGameInfo2 = [];
var gameRecInfo = [];
var blackGameMap = [];
initGameJsonData();
function initGameJsonData() {
    $.ajaxSettings.async = false;
    $.getJSON("/common/js/gameback.json?v=1.0.0", function (data) {
        $.each(eval(data), function (key, val) {
            blackGameInfo2.push(val);
            blackGameMap[key] = val;
        });
    });
    $.getJSON("/common/js/gamerec.json?v=1.0.0", function (data) {
        $.each(eval(data), function (key, val) {
            gameRecInfo.push(val);
        });
    });
    $.ajaxSettings.async = true;
}
function GetGameBlackModel(gameid) {
    if (gameid < 1) return null;
    for (var i = 0; i < blackGameInfo2.length; i++) {
        if (blackGameInfo2[i]["matchid"] == gameid) {
            return blackGameInfo2[i];
        }
    }
}
function GetGameName(gameid) {
    var gameModel = GetGameBlackModel(gameid);
    if (gameModel && gameModel != null)
        return gameModel.name;
    return "";
}

function GetGameRecModel(gameid) {
    if (gameid < 1) return null;
    for (var i = 0; i < gameRecInfo.length; i++) {
        if (gameRecInfo[i]["matchid"] == gameid) {
            return gameRecInfo[i];
        }
    }
}

function getBlackLevelStr(level) {
    if (!level || level == "") return "";
    level = level.toUpperCase();
    if (level == "LOW")
        return "低";
    else if (level == "MIDDLE")
        return "中";
    else if (level == "HIGH")
        return "高";
    return level;
}

function showInputVal(area) {
    if ($('#inputVal' + area).is(':checked')) {
        $("#valLi" + area).show();
        $("#tokenLi" + area).show();
    } else {
        $("#valLi" + area).hide();
        $("#tokenLi" + area).hide();
    }
}


function downGameLogData(gameid, chargeid, model, type) {
    var xhr = new XMLHttpRequest();
    var date = (new Date()).format("yyyyMMdd");
    var fileName = date + "_" + gameid + "_" + chargeid + "_" + (model == 1 ? "SHICHUI" : "ALL") + "_" + type + ".txt";
    xhr.open("POST", "/HandlerDownFile.ashx?gameid=" + gameid + "&chargeid=" + chargeid + "&model=" + model + "&type=" + type, true);
    xhr.responseType = "blob";
    xhr.onload = function () {
        if (this.status == 200) {
            var blob = this.response;
            var reader = new FileReader();
            reader.readAsDataURL(blob);
            reader.onload = function (e) {
                var a = document.createElement('a');
                a.download = fileName;
                a.href = e.target.result;
                if ($("#loading"))
                    $("#loading").hide();
                if (a.href.length < 10) {
                    alert("没有数据");
                    clearInterval(logTime);
                    return;
                }
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            }
        }
    };
    xhr.send();
}
var logTime;
function downloadLog(o, type) {
    $("#loading").show();
    downGameLogData(o.GameId, o.ChargeId, type, "REC");
    logTime = setInterval(function () {
        downGameLogData(o.GameId, o.ChargeId, type, "LOG");
        clearInterval(logTime);
    }, 1000);
}


var oprObjTmp;
function setWinMoney(o) {
    oprObjTmp = o;
    showAddMoneyWin(2);
    $("#tgame2").text(games[o.GameId]);
    $("#tchargeid2").text(o.ChargeId);
    $("#keepVal").val(o.Money);
    $("#msgtitle").text("设置输赢值");
}
function setWinMoneyConfirm() {
    if (!oprObjTmp || oprObjTmp == null) {
        alert("数据错误，请重试");
        return;
    }
    var gameid = oprObjTmp.GameId;
    var gameInfo = GetGameBlackModel(gameid);
    if (gameInfo == null) return;
    var type = gameInfo.type;
    var account = oprObjTmp.Account;
    var chargeid = oprObjTmp.ChargeId;
    var token = $("#token2").val();
    var val = $('input:radio[name="gameval2"]:checked').val();
    if ($('#inputVal2').is(':checked')) {
        if (token == "") {
            $("#lblerr2").text("请输入安全令");
            return;
        }
        val = $("#tval2").val();
    }
    //请选择游戏
    if (type == "") {
        $("#lblerr2").text("游戏错误");
        return;
    }
    //else if (chargeid == "") {
    //    $("#lblerr2").text("请选择需要删除的游戏账号");
    //    return;
    //}
    //保留时val可为空
    ajax.setWinMoney("setwinnmoney", [gameid, account, type, chargeid, val, token], delwinresult);
}
function delwinresult(res) {
    $("#loading").hide();
    if (res.code == 1) {
        var oprAcc = oprObjTmp.Account.toUpperCase();
        var oprGameId = oprObjTmp.GameId;
        oprObjTmp = null;
        alert("操作成功");
        $('.theme-popover-mask').hide();
        $('.theme-popover').slideUp(200);
        var dataOld = jsonPager.data;
        var index = 0;
        for (var i = 0; i < dataOld.length; i++) {
            if (dataOld[i]["Account"].trim().toUpperCase() == oprAcc
                && dataOld[i]["GameId"].trim().toUpperCase() == oprGameId) {
                index = i;
                break;
            }
        }
        dataOld.splice(index, 1);
        jsonPager.data = dataOld;
        jsonPager.dataBind(res.index, dataOld.length);
    } else {
        if (res.msg != "")
            alert(res.msg);
        else
            alert("操作失败：" + res.code);
    }
}
function initValData(id, gameid, level) {
    $("#valTypeArea" + id).html("");
    var gameInfo = GetGameBlackModel(gameid);
    if (gameInfo == null) return;
    var isLow = "", isMiddle = "", isHigh = "";
    if (level && level != "") {
        if (level == "LOW") isLow = "checked";
        else if (level == "MIDDLE") isMiddle = "checked";
        else if (level == "HIGH") isHigh = "checked";
    }
    $("#valTypeArea" + id).append("<input type='radio' value='[" + gameInfo.low + "]' name='gameval' " + isLow + " /><label style='display:none'>LOW</label>低");
    $("#valTypeArea" + id).append("<input type='radio' value='[" + gameInfo.middle + "]' name='gameval' " + isMiddle + " /><label style='display:none'>MIDDLE</label>中");
    $("#valTypeArea" + id).append("<input type='radio' value='[" + gameInfo.high + "]' name='gameval' " + isHigh + " /><label style='display:none'>HIGH</label>高");
}
function initLevel(id) {
    $("#valTypeArea" + id).html("");
    $("#valTypeArea" + id).append("<input type='radio' value='1' name='gameval'/><label style='display:none'>LOW</label>低");
    $("#valTypeArea" + id).append("<input type='radio' value='2' name='gameval'/><label style='display:none'>MIDDLE</label>中");
    $("#valTypeArea" + id).append("<input type='radio' value='3' name='gameval'/><label style='display:none'>HIGH</label>高");
}
function addBlackUser() {
    showAddMoneyWin(4);
    initLevel(4);
    $("#msgtitle").text("添加游戏黑名单");
}
function addBlackUserConfirm() {
    var gameids = [];
    $("input:checkbox[name='chkGame']:checked").each(function () {
        if ($(this).val() > 0)
            gameids.push($(this).val());
    });
    if (gameids.length < 1) {
        $("#lblerr4").text("请选择游戏");
        return;
    }
    var acc = $("#tacc4").val();
    if (acc == "") {
        $("#lblerr4").text("请输入UID");
        return;
    }
    var level = $('input:radio[name="gameval"]:checked').val();
    if (level == "") {
        $("#lblerr4").text("请选择输赢值档位");
        return;
    }
    var levels = [];
    var levelStrs = [];
    for (var i = 0; i < gameids.length; i++) {
        var m = GetGameBlackModel(gameids[i]);
        if (m == null || (!m.low && !m.middle && !m.high) || (m.low == "" && m.middle == "" && m.high == "")) {
            levels.push([]);
            levelStrs.push("");
        }
        else {
            switch (level) {
                case "1":
                    levels.push(m.low);
                    levelStrs.push("LOW");
                    break;
                case "2":
                    levels.push(m.middle);
                    levelStrs.push("MIDDL");
                    break;
                case "3":
                    levels.push(m.high);
                    levelStrs.push("HIGH");
                    break;
            }
        }
    }
    if (levels.length < 1 || levelStrs.length < 1) {
        $("#lblerr4").text("输赢值数据有误.");
        return;
    }
    var remark = $("#txtRemark4").val();
    if (!remark || remark == "") {
        $("#lblerr4").text("请输入备注");
        return;
    } 
    var gameidsStr = gameids.join("|");
    var levelsStr = levels.join("|");
    var levelStrsStr = levelStrs.join("|");
    var token = "";
    ajax.addBlackUser("addblackuser", [gameidsStr, acc, levelsStr, levelStrsStr, remark, token], addwinresult);
}
function addwinresult(res) {
    $("#loading").hide();
    if (res.code == 1) {
        alert("操作成功");
        $('.theme-popover-mask').hide();
        $('.theme-popover').slideUp(200);
    } else {
        if (res.msg != "")
            alert(res.msg);
        else
            alert("操作失败：" + res.code);
    }
}
var oprUpdateObj;
function updateBlackUser(o) {
    oprUpdateObj = o;
    showAddMoneyWin(1);
    initValData("", o.GameId, o.Level);
    $("#lblgame").text(GetGameName(o.GameId));
    $("#lblaccount").text(o.Account);
    $("#lblchargeid").text(o.ChargeId);
    $("#txtRemark").text(o.Remark);
    $("#msgtitle").text("修改黑名单用户");
}
function updateUserConfirm() {
    if (!oprUpdateObj || oprUpdateObj == null) {
        alert("操作有误");
        return;
    }
    var val = $('input:radio[name="gameval"]:checked').val();
    var levelStr = $('input:radio[name="gameval"]:checked').next("label").text();
    if ($('#inputVal1').is(':checked')) {
        val = "";
        if (token == "") {
            $("#lblerr").text("请输入安全令");
            return;
        }
        if ($("#tval11").val() != "" && $("#tval12").val() != "")
            val = "[" + $("#tval11").val() + "," + $("#tval12").val() + "]";
        //获取低中高的中文字符
        levelStr = "";
    }
    var gameid = oprUpdateObj.GameId;
    var acc = oprUpdateObj.Account;
    var chargeid = oprUpdateObj.ChargeId;
    var remark = $("#txtRemark").val();
    var token = $("#token").val();
    if (gameid == "" || parseInt(gameid) < 1 || acc == "" || chargeid == "") {
        $("#lblerr").text("参数错误");
        return;
    }
    else if (!val || val == "") {
        $("#lblerr").text("请设置值");
        return;
    }
    else if (!remark || remark == "") {
        $("#lblerr").text("请输入备注");
        return;
    }
    ajax.updateBlackUser("updateblackuser", [gameid, acc, chargeid, val, levelStr, remark, token], updatewinresult);
}
function updatewinresult(res) {
    $("#loading").hide();
    if (res.code == 1) {
        var oprAcc = oprUpdateObj.Account.toUpperCase();
        var oprGameId = oprUpdateObj.GameId;
        oprUpdateObj = null;
        alert("操作成功");
        $('.theme-popover-mask').hide();
        $('.theme-popover').slideUp(200);
        var dataOld = jsonPager.data;
        var newModel = res.msg;
        var oldModel;
        var index = 0;
        for (var i = 0; i < dataOld.length; i++) {
            if (dataOld[i]["Account"].trim().toUpperCase() == oprAcc && dataOld[i]["GameId"].trim().toUpperCase() == oprGameId) {
                oldModel = dataOld[i];
                index = i;
                break;
            }
        }
        oldModel["Value"] = newModel["Value"];
        oldModel["Level"] = newModel["Level"];
        oldModel["Remark"] = newModel["Remark"];
        dataOld[index] = oldModel;
        jsonPager.data = dataOld;
        jsonPager.dataBind(res.index, dataOld.length);
    } else {
        if (res.msg != "")
            alert(res.msg);
        else
            alert("操作失败：" + res.code);
    }
}
function getGameMoney(o) {
    var gameInfo = GetGameBlackModel(o.GameId);
    if (gameInfo == null) return;
    ajax.getUserGameMoney([o.ChargeId, gameInfo.type], function (res) {
        if (res.code == 1) {
            showAddMoneyWin(5);
            $("#lblgame5").text(GetGameName(o.GameId));
            $("#lblaccount5").text(o.Account);
            $("#lblchargeid5").text(o.ChargeId);
            $("#lblMoney5").text(res.msg);
            $("#lblRemark5").text(o.Remark);
        } else
            alert("获取游戏输赢值出错");
    });
    $("#msgtitle").text("查看玩家输赢值");
}
function checkAll(o) {
    if (o.checked) {
        $("input:checkbox[name='chkGame']").each(function () {
            this.checked = true;
        });
    } else {
        $("input:checkbox[name='chkGame']:checked").each(function () {
            this.checked = false;
        });
    }
}
function initNick(nickList) {
    if (nickList == "") {
        return "";
    }
    var nickArr = nickList.split(",");
    var list = "";
    for (var j = 0; j < nickArr.length; j++) {
        var nick = getEmoji(nickArr[j]);
        list += nick + "</br>";
    }
    return list;
}

function getEmoji(nick) {
    if (nick == "") {
        return "";
    }
    if (nick.indexOf("\\U000") >= 0) {
        var nicks = nick.split(" ");
        var newNick = "";
        for (var i = 0; i < nicks.length; i++) {
            var emojiIndex = nicks[i].indexOf("\\U000");
            if (emojiIndex >= 0) {
                var before = nicks[i].substring(0, emojiIndex) || "";
                var emojiTmp = nicks[i].substring(emojiIndex, emojiIndex + 10);
                var emojiStr = emojiTmp.replace("\\U000", "0x");
                var emoji = String.fromCodePoint(emojiStr) || "";
                var after = nicks[i].substring(emojiIndex + 10) || "";
                newNick += before + emoji + after + " ";
            } else
                newNick += nicks[i] + " ";
        }
        return newNick;
    }
    else {
        return nick;
    }
}