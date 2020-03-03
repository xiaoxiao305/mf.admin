$$ = function(id) {
    if (document.getElementById(id))
        return document.getElementById(id);
    return document.getElementById("ctl00_p_" + id);
}
var dicType = {
    1: "报名", 2: "复活", 3: "VIP每日赠送", 4: "金券兑换", 5: "获胜", 6: "退赛", 7: "兑换战斗力", 8: "邮箱提取", 9: "商城兑换", 10: "注册赠送",
    11: "推广现金兑换", 12: "艾普积分兑换", 13: "开推广礼包", 14: "投诉保证金", 15: "战斗力回兑", 16: "领取救济",
    17: "保险箱手续费", 18: "存入保险箱", 19: "保险箱取出", 20: "恢复执照分", 21: "开红包获奖",
    22: "首次绑定手机赠送", 23: "完成俱乐部任务", 24: "购买房卡", 25: "系统赠送",
    26: "银票兑换", 27: "管理员派发", 28: "主/子账号互转", 29: "首充奖励", 30: "税"
};
var dicHappycardType = {1: "报名",  6: "退赛", 26: "充值赠送"};
deviceType = function (_device) { if (_device == 2) { return "PC"; } else if (_device == 3) { return "iPad"; } else if (_device == 5) { return "iOS"; } else if (_device == 6) { return "Android"; } else { return _device; } }
function showTimeBox(o) {
    if (o.checked == true)
        $("#divTime").show();
    else
        $("#divTime").hide();
}
function winresult(res) {
    $("#loading").hide();
    if (res.code == 1) {
        alert("操作成功");
        window.location.href = window.location.href;
    } else {
        if (res.msg != "")
            alert(res.msg);
        else
            alert("操作失败：" + res.code);
    }
}
/*----------------------------------------时间控件----------------------------------------*/
var todayDate = new Date();
var currDate = todayDate.getFullYear() + "-" + (parseInt(todayDate.getMonth()) + 1) + "-" + todayDate.getDate();
var currMonthFDay = todayDate.getFullYear() + "-" + (parseInt(todayDate.getMonth()) + 1) + "-1";
var currWeekFDay = todayDate.getFullYear() + "-" + (parseInt(todayDate.getMonth()) + 1) + "-1";
//时间  参数Id 如: #StartDate, #EndDate
function attachCalenderbox(start, end, options,startTime,overTime) {
    var se = "";
    if (start != "" && end != "")
        se = start + "," + end;
    else {
        if (end == "" || end == null)
            se = start;
    }
    var mindate = null;
    if (options != null) {
        if (options.minday > 0) {
            mindate = new Date();
            mindate.setDate(mindate.getDate() - options.minday);
        }
    }
    var options = { minDate: mindate };
    var defaults = { minDate: options.minDate };
    $.extend(defaults, options);
    var dates = $(se).datepicker({
        minDate: defaults.minDate,
        maxDate: new Date(),
        changeYear: true,
        //showButtonPanel: true,
        changeMonth: true,
        onSelect: function(selectedDate) {
            var option = this.id == $(start)[0].id ? "minDate" : "maxDate",
					instance = $(this).data("datepicker");
            date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });
    if (startTime != null) {
        $(start).val(startTime);
    }
    if (overTime != null) {
        $(end).val(overTime);
    } 
}
function attachCalenderbox2(start, end, options, startTime, overTime) {
    var se = "";
    if (start != "" && end != "")
        se = start + "," + end;
    else {
        if (end == "" || end == null)
            se = start;
    }
    var mindate = null;
    if (options != null) {
        if (options.minday > 0) {
            mindate = new Date();
            mindate.setDate(mindate.getDate() - options.minday);
        }
    }
    var options = { minDate: mindate };
    var defaults = { minDate: options.minDate };
    $.extend(defaults, options);
    var dates = $(se).datepicker({
        minDate: defaults.minDate,
        maxDate: 365,
        changeYear: true,
        //showButtonPanel: true,
        changeMonth: true,
        onSelect: function (selectedDate) {
            var option = this.id == $(start)[0].id ? "minDate" : "maxDate",
                instance = $(this).data("datepicker");
            date = $.datepicker.parseDate(
                instance.settings.dateFormat ||
                $.datepicker._defaults.dateFormat,
                selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });
    if (startTime != null) {
        $(start).val(startTime);
    }
    if (overTime != null) {
        $(end).val(overTime);
    }
}
function ConvertToDate(datepart, number) {
    alert(datepart);
    var date = new Date("2012/10/01 00:00:00");
    return date.dateAdd(datepart, number);
}
/*****************************************
*函数名称:dateAdd
*功能:时间添加
*返回值:时间值
*示例:date.dateAdd("s", 123)
******************************************/
Date.prototype.dateAdd = function(datepart, number) {//datepart:y,M,d,h,m,s,ms
    var d = this;
    var interval = { "y": "FullYear", "q": "Month", "m": "Month", "w": "Date", "d": "Date", "h": "Hours", "n": "Minutes", "s": "Seconds", "ms": "MilliSeconds" };
    var n = { "q": 3, "w": 7 };
    eval("d.set" + interval[datepart] + "(d.get" + interval[datepart] + "()+" + ((n[datepart] || 1) * number) + ")");
    return d;
}
Date.prototype.format = function(format) {
    var o = { "M+": this.getMonth() + 1, "d+": this.getDate(), "h+": this.getHours(), "m+": this.getMinutes(), "s+": this.getSeconds(), "q+": Math.floor((this.getMonth() + 3) / 3), "S": this.getMilliseconds() }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    }
    return format;
}
//返回距2012-10-01的天数或秒数
Date.prototype.dateDiff = function(datepart) {
    var date = new Date("2012/10/01 00:00:00");
    if (datepart == "d" || datepart == "dd") {
        return Math.floor(this.getTime() / 86400000) - Math.floor(date.getTime() / 86400000);
    } else if (datepart == "s" || datepart == "ss") {
        return Math.floor(this.getTime() / 1000) - Math.floor(date.getTime() / 1000);
    }
    return 0;
}

// 对Date的扩展，将 Date 转化为指定格式的String 
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function(fmt) {
    //author: meizz 
    var o = {
        "M+": this.getMonth() + 1,            //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                  //小时 
        "m+": this.getMinutes(),             //分 
        "s+": this.getSeconds(),              //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
        fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

/*----------------------------------------string.js----------------------------------------*/
/****************************************************
*函数名称:isFloat()
*功能:校验是否是float类型
*返回值:转换成功返回数字,失败返回0
*示例: var float="123.00";if(float.isFloat())alert("是的")
*****************************************************/
String.prototype.isFloat = function() {
    var patrn = /^(0|[1-9]\d*)$|^(0|[1-9]\d*)\.(\d{1,2})$/
    if (patrn.exec(this))
        return true;
    else
        return false;
}

//对浮点数取小数位数
//obj:对象
//dec:小数位数
String.prototype.toFloat = function(o) {
    var number = o.value;
    if (number.isNumber() || number.isFloat()) {
        number = number * 100 + 0.5;
        number = parseInt(number) / 100;
        o.value = number;
    }
    else
        o.value = "0.00";
}
/****************************************************
*函数名称:toNumber()
*功能:将字符串转换为整型数字
*返回值:转换成功返回数字,失败返回0
*示例: var number="123";number=number.toNumber()+1;
*****************************************************/
String.prototype.toNumber = function() {
    var patrn = /^[0-9]{1,30}$/;
    if (patrn.exec(this))
        return parseInt(this);
    else
        return 0;
}

/****************************************************
*函数名称:trim()
*功能:清除字符串前后的空格
*返回值:清除字符串两端的空格后的字符
*示例: var str=" abc "; str=str.trim();
*****************************************************/
String.prototype.trim = function() { return this.replace(/(^\s*)|(\s*$)/g, ""); }

/****************************************************
*函数名称:ltrim()
*功能:清除字符串左边的空格
*返回值:清除字符串左边的空格后的字符
*示例: var str=" abc "; str=str.trim(); 
*****************************************************/
String.prototype.ltrim = function() { return this.replace(/(^\s*)/g, ""); }

/****************************************************
*函数名称:rtrim()
*功能:清除字符串右边的空格
*返回值:清除字符串右边的空格后的字符
*示例: var str=" abc "; str=str.trim();
*****************************************************/
String.prototype.rtrim = function() { return this.replace(/(\s*$)/g, ""); }

/****************************************************
*函数名称:isNumber()
*功能:校验是否全由数字组成
*返回值:验证通过返回true,失败返回false
*示例:  var str="123";if(str.isNumber())alert("成功")
*****************************************************/
String.prototype.isNumber = function() {
    var patrn = /^[0-9]{1,30}$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validChinaWord()
*功能:验证是否为汉字
*返回值:验证通过返回true,失败返回false
*示例: var str="卡塞蒂";if(str.validChinaWord())alert("成功")
*****************************************************/
String.prototype.validChinaWord = function() {
    var patrn = /^[\u4E00-\u9FA5]+$/gi;
    var charlist = this.split('');
    for (i = 0; i < charlist.length; i++) {
        if (patrn.test(charlist[i])) {
            return true;
        }
    }
    return false;
}

/****************************************************
*函数名称:isFristWord()
*功能:验证一个字符串的第一个字母是否是一个汉字开头
*返回值:成功返回true,失败返回false
*示例: 
*****************************************************/
String.prototype.isFristWord = function() {
    var patrn = /^[\u4E00-\u9FA5]/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:replaceSpecial()
*功能:替换系统关键字
*返回值:替换后的字符串
*示例: 
*****************************************************/
String.prototype.replaceSpecial = function() {
    var valueArray;
    var StrValue = "";
    var strarray = new Array();

    if (this.length > 0) {
        try {
            strarray = this.split("");
            for (var i = 0; i < strarray.length; i++) {
                valueArray = strarray[i].replace("'", "’").replace("%", "％").replace("&", "＆").replace("#", "＃").replace("!", "！").replace("<", "&lt;").replace(">", "&gt;").replace("=", "＝").replace(";", "；");
                StrValue += valueArray;
            }
        }
        catch (e) { }
    }
    StrValue = StrValue.replace("undefined", "");
    return StrValue;
}


/****************************************************
*函数名称:filterSpecial()
*功能:过滤系统关键字
*返回值:过滤系统关键字
*示例: 
*****************************************************/
String.prototype.filterSpecial = function() {
    var valueArray;
    var StrValue = "";
    var strarray = new Array();

    if (this.length > 0) {
        try {
            strarray = this.split("");
            for (var i = 0; i < strarray.length; i++) {
                valueArray = strarray[i].replace("'", "").replace("%", "").replace("&", "").replace("#", "").replace("!", "").replace("<", "").replace(">", "").replace("=", "").replace(";", "");
                StrValue += valueArray;
            }
        }
        catch (e) { }
    }
    StrValue = StrValue.replace("undefined", "");
    return StrValue;
}

/****************************************************
*函数名称:checkKeyword()
*功能:验证是否存在系统关键字
*返回值:存在返回true 不存在返回false
*示例: 
*****************************************************/
String.prototype.checkKeyword = function() {
    var strarray = new Array();
    if (this.length > 0) {
        try {
            strarray = this.split("");
            for (var i = 0; i < strarray.length; i++) {
                if (strarray[i] == "'" || strarray[i] == "%" || strarray[i] == "#" || strarray[i] == "!" || strarray[i] == "<" || strarray[i] == ">" || strarray[i] == "=" || strarray[i] == ";")
                    return true;
            }
        }
        catch (e) { }
    }
    return false;
}



/****************************************************
*函数名称:validLetter()
*功能: 验证是否字母组成，不区分大小写的
*返回值:验证通过返回true,失败返回false
*示例: var str="卡塞蒂";if(str.validLetter())alert("成功")
*****************************************************/
String.prototype.validLetter = function() {
    var patrn = /^[a-zA-Z]$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validWord()
*功能:验证是否由汉字、字母、和数字组成
*返回值:验证通过返回true,失败返回false
*示例: var str="卡塞蒂";if(str.validWord())alert("成功")
*****************************************************/
String.prototype.validWord = function() {
    var patrn = /^[a-zA-Z0-9\u4E00-\u9FA5]+$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validIp()
*功能:验证IP格式是否正确
*返回值:验证通过返回true,失败返回false
*示例:var ip="127.0.0.1";if(ip.validIp())alert("成功");
*****************************************************/
String.prototype.validIp = function() {
    var patrn = /^(([3-9]\d?|[01]\d{0,2}|2\d?|2[0-4]\d|25[0-5])\.){3}([3-9]\d?|[01]\d{0,2}|2\d?|2[0-4]\d|25[0-5])$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validEmail()
*功能:验证Email格式是否正确
*返回值:验证通过返回true,失败返回false
*示例:var email="clumsy.boy@163.com";if(email.validEmail())alert("成功");
*****************************************************/
String.prototype.validEmail = function() {
    var patrn = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validPhone()
*功能:验证电话格式是否正确
*返回值:验证通过返回true,失败返回false
*示例:var phone="028-12345678";if(phone.validPhone())alert("成功");
*****************************************************/
String.prototype.validPhone = function() {

    var patrn = /^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validpostCode()
*功能:验证邮政编码是否正确
*返回值:验证通过返回true,失败返回false
*示例:var postCode="610000";if(postCode.validPhone())alert("成功");
*****************************************************/
String.prototype.validpostCode = function() {
    var patrn = /\d{6}$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}

/****************************************************
*函数名称:validMobile()
*功能:
*返回值:
*示例:
*****************************************************/
String.prototype.validMobile = function() {

    var patrn = /^(13[0-9]|15[0-9]|18[0-9])\d{8}$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}
/*****************************************
*函数名称:validQQ()
*功能:
*返回值:
*示例:
******************************************/
String.prototype.validQQ = function() {

    var patrn = /\d{5,9}$/;
    if (patrn.exec(this))
        return true;
    else
        return false;
}
/*****************************************
*函数名称:validIDCard()
*功能:验证身份证格式是否正确
*返回值:
*示例:
******************************************/
String.prototype.validIDCard = function() {
    var isIDCard1 = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/;
    var isIDCard2 = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}[xX\d]$/;
    var cityCode = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江",
        34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州",
        53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外"
    };
    if (this.trim().length == 15) {

        if (isIDCard1.exec(this)) {
            if (cityCode[parseInt(this.substr(0, 2))] == null) return false;
            var datePatrn = /^[40-99](0\d{1})|[10-12](0\d{1})|[10-31]$/;
            var birthady = this.substr(6, 6);
            if (datePatrn.exec(birthady))
                return true;
            else
                return false;
        }
        else
            return false;
    }
    else if (this.trim().length == 18) {

        if (isIDCard2.exec(this)) {
            var serail = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2";
            serail = serail.split(',');
            var validcode = "1,0,X,9,8,7,6,5,4,3,2";
            validcode = validcode.split(",");
            var resultcode = "0,1,2,3,4,5,6,7,8,9,10";
            resultcode = resultcode.split(",");
            if (cityCode[parseInt(this.substr(0, 2))] == null) return false;
            var id = this.substr(0, 17).split('');
            var s = 0;
            for (var i = 0; i < 17; i++)
                s += parseInt(serail[i]) * parseInt(id[i]);
            var ret = s % 11;
            for (var i = 0; i < 11; i++) {
                if (ret == resultcode[i]) {
                    if (validcode[i] == this.substr(17, 1).toUpperCase())
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        else
            return false;
    }
    else
        return false;
}
//获取字符串长度
String.prototype.len = function() {
    var l = 0;
    var a = this.split("");
    for (var i = 0; i < a.length; i++) {
        if (a[i].charCodeAt(0) < 299)
            l++;
        else
            l += 2;
    }
    return l;
}
//格式化字符串
String.prototype.format = function(args) {
    var result = this;
    if(arguments.length<1)
        return result;
    var data =arguments;
    if(arguments.length ==1 && typeof(args) =="object")
        data = args;
    for (var i = 0; i < data.length; i++) {
        var value = data[i];     
        if(undefined != value)
            result = result.replace("{"+i+"}",value);
    }
    return result;
}




/*--------------------------------------------------window.js--------------------------------------------------*/
/*****************************************
*函数名称:OpenDialog()
*功能:按指定大小弹出模态窗口
*返回值:
*示例:
******************************************/
function OpenDialog(url, swidth, sheight, args) {
    var str = new String(url);
    var join = str.indexOf('?') == -1 ? '?' : '&';
    url = url + join + 'op=' + Math.random(); //dialog
    var H = (screen.height - sheight) / 2 - 31;
    var W = (screen.width - swidth) / 2;
    if (args == "undefined") args = "window";
    var arr = showModalDialog(url, args, "dialogWidth:" + swidth + "px; dialogHeight:" + sheight + "px;dialogTop=" + H + "px;dialogLeft=" + W + "px;edge: raised; center: yes; help: no; resizable: no; status: no; scroll:on;");
    return arr;
}



/*--------------------------------------------------添加货币弹出窗--------------------------------------------------*/
var moneyType=["","元宝","金豆","用户房卡","俱乐部房卡"]
//显示弹出窗
    //type为需要显示窗体的id[T1] 一般T1为输入窗，T2为消息确认窗
function showAddMoneyWin(type) {
    $('.theme-popover-mask').show();
    $('.theme-popover').slideDown(200);
    $('.theme-popover').css("width", 380);
    $('.theme-popover').css("left", "55%");
    $('.theme-popover').css("top", "50%");
    $("#content").html($("#T" + type).html());
    $('.theme-poptit .close').click(function () { $('.theme-popover-mask').hide(); $('.theme-popover').slideUp(200); });
}
//type 1元宝 2金豆 3用户房卡 4俱乐部房卡
function addusermoney(account, type) {
    var num = $("#num").val();
    var token = $("#token").val();
    if (num == "") {
        $("#lblerr").text("请输入" + moneyType[type] + "数量");
        return;
    }
    else if (parseInt(num) != num) {
        $("#lblerr").text("请输入正确的" + moneyType[type] + "数量");
        return;
    }
    else if (parseInt(num) > 100000000) {
        $("#lblerr").text("请输入正确的" + moneyType[type] + "数量");
        return;
    }
    else if (token == "") {
        $("#lblerr").text("请输入安全令");
        return;
    }
    $("#hidNum").val(num);
    $("#hidToken").val(token);
    if (type == 4) {
        var n = $('#lblName1').text();//在此之前为隐藏窗体赋值有缓存
        $('#lblName1').text(n);
        $('#lblName2').text(n);
    }
    ajax.setUserMoneyChk("setusermoneychk", [account, parseFloat(num), token], chkResult);
}
function chkResult(res) {
    $("#loading").hide();
    if (res.code == 1) {
        $("#confirmnum").text($("#hidNum").val());
        showAddMoneyWin(2);
    }
    else {
        $("#lblerr").text(res.msg);
        return;
    }
}
function confirmopr(account, type) {
    if (confirm("确认要进行【添加" + moneyType[type] + "】操作？"))
    {
        if (type == 4)//添加俱乐部房卡
            ajax.setUserMoney("setclubsroomcard", [account, parseFloat($("#hidNum").val()), $("#hidToken").val()], winresult);
        else
            ajax.setUserMoney("setusermoney", [account, type, parseFloat($("#hidNum").val()), $("#hidToken").val()], winresult);
    }
}
//function GetGameBlackModel(gameid) {
//    if (gameid < 1) return null;
//    for (var i = 0; i < blackGameInfo.length; i++) {
//        if (blackGameInfo[i]["matchid"] == gameid) {
//            return blackGameInfo[i];
//        }
//    }
//}
 
var currentClubId = "";