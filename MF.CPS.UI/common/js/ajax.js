var ajax = {
    url: "/m/ajax.ashx",
    login: function () { },
    signOut: function () { $.ajax({ url: this.url, data: { m: "out", args: "[]", r: Math.random() }, success: function () { parent.location.href = "/"; }, error: function (xhr, status, err) { parent.location.href = "/"; } }); },
    getPromotChargeList: function (args, callback) {
            $.ajax({
                url: ajax.url,
                data: {
                    m: "promotcharge",
                    args: JSON.stringify(args),
                    r: Math.random()
                },
                dataType: "json",
                cache: false,
                success: callback,
                error: function (xhr, status, err) {
                    $("#loading").hide();
                    alert(err);
                }
            });
        },
    getCPSUserList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "cpsuserlist",
                args: JSON.stringify(args),
                r: Math.random()
            },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    } 
    
    
};
