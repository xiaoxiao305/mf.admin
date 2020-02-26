var jsonPager = {
    data: [],
    title: [],
    className: "",
    containerId: "",
    pagerId: "",
    pageSize: 20,
    pageIndex: 1,
    datagrid: null,
    pageCount: 0,
    loadRow: null,
    rowCount: 0,
    queryArgs: [],
    queryFunc: null,
    callback: null,
    nextPage: function() {
        if (this.pageIndex >= this.pageCount) return;
        $("#loading").show();
        var args = this.makeArgs(this.pageIndex + 1);
        this.queryFunc(args, this.callback);
    },
    lastPage: function() {
        $("#loading").show();
        var args = this.makeArgs(this.pageCount);
        this.queryFunc(args, this.callback);
    },
    gotoPage: function() {
        var index = $("#josnpagerbox").val();
        var patrn = /^[0-9]{1,30}$/;
        if (!patrn.exec(index)) {
            alert("请输入正确的页码");
            $("#josnpagerbox").val(jsonPager.pageIndex);
            return false;
        }
        index = parseInt(index);
        if (index > this.pageCount || index < 1) {
            if (index > this.pageCount)
                index = this.pageCount;
            if (index < 1)
                index = 1;
        }
        $("#loading").show();
        var args = this.makeArgs(index);
        this.queryFunc(args, this.callback);
    },
    prevPage: function() {
        if (this.pageIndex <= 1) return;
        $("#loading").show();
        var args = this.makeArgs(this.pageIndex - 1);
        this.queryFunc(args, this.callback);
    },
    firstPage: function() {
        $("#loading").show();
        var args = this.makeArgs(1);
        this.queryFunc(args, this.callback);
    },
    makeArgs: function(index) {
        args = [this.pageSize, index];
        for (var i = 0; i < this.queryArgs.length; i++)
            args.push(this.queryArgs[i]);
        return args;
    },
    init: function(_queryfunc, _queryArgs, _callback, _title, _css, _container, _pagerId, _loadRow) {
        this.queryArgs = _queryArgs;
        this.queryFunc = _queryfunc;
        this.callback = _callback;
        this.title = _title;
        this.className = _css;
        this.containerId = _container;
        this.pagerId = _pagerId;
        this.loadRow = _loadRow;
    },
    enableButton: function(flag) {
        var btnFirst = document.getElementById("btnPagerFirstJosn");
        var btnPre = document.getElementById("btnPagerPreJosn");
        var btnNext = document.getElementById("btnPagerNextJosn");
        var btnLast = document.getElementById("btnPagerLastJosn");
        var btnGoto = document.getElementById("btnPagerGotoJosn");
        switch (flag) {
            case 0:
                btnFirst.disabled = true;
                btnNext.disabled = true;
                btnPre.disabled = true;
                btnLast.disabled = true;
                btnFirst.className = "button first1";
                btnPre.className = "button prev1";
                btnNext.className = "button next1";
                btnLast.className = "button last1";
                btnGoto.disabled = true;
                break;
            case 1:
                btnFirst.disabled = true;
                btnPre.disabled = true;
                btnNext.disabled = false;
                btnLast.disabled = false;
                btnGoto.disabled = false;
                btnFirst.className = "button first1";
                btnPre.className = "button prev1";
                btnNext.className = "button next";
                btnLast.className = "button last";
                break;
            case 2:
                btnFirst.disabled = false;
                btnPre.disabled = false;
                btnNext.disabled = true;
                btnLast.disabled = true;
                btnGoto.disabled = false;
                btnFirst.className = "button first";
                btnPre.className = "button prev";
                btnNext.className = "button next1";
                btnLast.className = "button last1";
                break;
            case 3:
                btnFirst.disabled = false;
                btnPre.disabled = false;
                btnNext.disabled = false;
                btnLast.disabled = false;
                btnFirst.className = "button first";
                btnPre.className = "button prev";
                btnNext.className = "button next";
                btnLast.className = "button last";
                btnGoto.disabled = false;
                break;
        }

    },
    clearList: function() {
    try{
        if (this.datagrid != null)
            document.getElementById(this.containerId).removeChild(this.datagrid);
        this.datagrid = document.createElement("table");
        this.datagrid.className = this.className;
        var trTitle = this.datagrid.insertRow(0);
        for (var i = 0; i < this.title.length; i++) {
            var td = document.createElement("th");
            td.innerHTML = this.title[i];
            td.className = this.titleCss;
            trTitle.appendChild(td);
        }}
        catch(e){}
        // this.datagrid.appendChild(trTitle); IE6中不能使用
    },
    changePageSize: function() {
        var size = document.getElementById("txtJosnPagerPageSize").value;
        var patrn = /^[0-9]{1,30}$/;
        if (!patrn.exec(size)) {
            alert("请输入正确的分页大小");
            document.getElementById("txtJosnPagerPageSize").value = this.pageSize;
            return false;
        }
        if ($.isNumeric(size)) {
            size = parseInt(size);
            if (size > 0 && size < 10000) {
                this.pageSize = size;
            } else {
                document.getElementById("txtJosnPagerPageSize").value = this.pageSize;
            }
        } else {
            document.getElementById("txtJosnPagerPageSize").value = this.pageSize;
        }
    },
    addPager: function() {
        createButton = function(css, evt, func, evtSource, id) {
            var o = document.createElement("input");
            o.type = "button";
            o.id = id;
            if (window.attachEvent)
                o.attachEvent(evt, function() { func.apply(evtSource, arguments); });
            else
                o.addEventListener(evt.replace("on", ""), function() { func.apply(evtSource, arguments); }, false);
            return o;
        };
        createGoButton = function(val, evt, func, evtSource, id) {
            var o = document.createElement("input");
            o.type = "button";
            o.value = val;
            o.id = id;
            o.className = "ui-button-icon-primary";

            if (window.attachEvent)
                o.attachEvent(evt, function() { func.apply(evtSource, arguments); });
            else
                o.addEventListener(evt.replace("on", ""), function() { func.apply(evtSource, arguments); }, false);

            return o;
        };
        createSearchBox = function(func, evtSource, index) {
            var box = document.createElement("input");
            box.type = "text";
            box.className = "input";
            box.id = "josnpagerbox";
            if (window.attachEvent)
                box.attachEvent("onkeypress", function() { if (event.keyCode == 13) { func.apply(evtSource, arguments); return false; } });
            else
                box.addEventListener("onkeypress".replace("on", ""), function() { if (event.which == 13) { func.apply(evtSource, arguments); event.preventDefault(); return false; } }, false); //firefox兼容性暂未解决
            box.value = index;
            return box;
        };
        createPageSizeBox = function(size, func, evtSource) {
            var box = document.createElement("input");
            box.type = "text";
            box.className = "input";
            box.id = "txtJosnPagerPageSize";
            if (window.attachEvent)
                box.attachEvent("onblur", function() { func.apply(evtSource, arguments); return false; });
            else
                box.addEventListener("onblur".replace("on", ""), function() { func.apply(evtSource, arguments); event.preventDefault(); return false; }, false); //firefox兼容性暂未解决
            box.value = size;
            return box;
        };
        var div = document.getElementById(this.pagerId);
        for (var i = 0; i < div.childNodes.length; i++)
            div.removeChild(div.childNodes[i]);
        div.innerHTML = "";
        var span = document.createElement("span");
        span.innerHTML = "共有记录" + this.rowCount + "条　每页显示";
        div.appendChild(span);
        div.appendChild(createPageSizeBox(this.pageSize, this.changePageSize, this));
        span = document.createElement("span");
        span.innerHTML = "条　" + this.pageIndex + "/" + this.pageCount + "页　"
        div.appendChild(span);
        div.appendChild(createButton("button first", "onclick", this.firstPage, this, "btnPagerFirstJosn"));
        div.appendChild(createButton("button prev", "onclick", this.prevPage, this, "btnPagerPreJosn"));
        div.appendChild(createButton("button next", "onclick", this.nextPage, this, "btnPagerNextJosn"));
        div.appendChild(createButton("button last", "onclick", this.lastPage, this, "btnPagerLastJosn"));
        var space = document.createElement("span");
        space.innerHTML = " 转到第 ";
        div.appendChild(space);
        div.appendChild(createSearchBox(this.gotoPage, this, this.pageIndex));
        space = document.createElement("span");
        space.innerHTML = " 页 ";
        div.appendChild(space);
        div.appendChild(createGoButton("GO", "onclick", this.gotoPage, this, "btnPagerGotoJosn"));
        space = document.createElement("span");
        space.innerHTML = "&nbsp;";
        div.appendChild(space);
    },
    dataBind: function(_pageindex, _rowCount) {
        this.rowCount = _rowCount;
        this.pageIndex = _pageindex;
        this.pageCount = Math.ceil(this.rowCount / this.pageSize);
        this.clearList();
        if(this.data !=null && this.data.length >0)
        {
            for (var i = 0; i < this.data.length; i++) {
                var row = this.datagrid.insertRow(i + 1);
                row = this.loadRow(this.data[i], row);
                // this.datagrid.appendChild(row);// IE6中不能使用
            }
            this.addPager();
            document.getElementById(this.containerId).appendChild(this.datagrid);
            if (this.data.length == 0 || ((this.pageIndex == this.pageCount) && this.pageIndex == 1))
                this.enableButton(0);
            else {
                if (this.pageIndex == 1)
                    this.enableButton(1);
                else if (this.pageIndex == this.pageCount)
                    this.enableButton(2);
                else
                    this.enableButton(3);
            }
        }
    }
};