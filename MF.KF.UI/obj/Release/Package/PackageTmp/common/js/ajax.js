var ajax = {
    url: "/m/ajax.ashx",
    login: function () { },
    signOut: function () { $.ajax({ url: this.url, data: { m: "out", args: "[]", r: Math.random() }, success: function () { parent.location.href = "/"; }, error: function (xhr, status, err) { parent.location.href = "/"; } }); },
    getUserList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "userlist",
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
    getCurrencyRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "currencyrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getChargeRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "chargerecord",
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
    getBeanRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "beanrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getSubUserList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "subuserlist",
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
    getGameReport: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "gamereport",
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
    getSceneReport: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "scenereport",
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
    getGuildList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "guildlist", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getClubGameLink: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "clubgamelink", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },    
    getGuildApplyRecord: function(args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "guildapplyrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function(xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getStrongBoxRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "strongboxrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getRoomCardRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "roomcardrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getDZCurrencyRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "dzcurrencyrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getGameServerList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "gameserverlist",
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
    flushGameServer: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    flushMatchGame: function (method, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    setUserInfo: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    setUserMoneyChk: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    setUserMoney: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    dealChargeOrder: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getReportList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "report",
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
    getQmallRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "qmallrecord",
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
    setGuildActive: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getExchangeReportList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "exchangereport",
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
    getExtendChannelRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "extendchannelrecord",
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
    setApplyGuildFlag: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getClubStatisticDay: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "clubstatisticday",
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
    DelClubStatisticClubId: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getGuildList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "guildlist", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    setRecomClubs: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    verifyGuildStatus: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getGuildLinkList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "guildlinklist", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getMemberActive: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getmemberactive", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },  
    getClubActive: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getclubactive", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },    
    setClubLink: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    }, 
    getClubActiveCount: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getclubactivecount", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    closeRoom: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "closeroom", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getMembersList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getmemberslist", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getGameBlackUsers: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getgameblackusers", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getAuditBlackUsers: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getauditblackusers", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },   
    getGameIncome: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getgameincome", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    GetLastGameRecords: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getlastgamerecords", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getGameRec: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getgamerec", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },   
    setWinMoney: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    addBlackUser: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    updateBlackUser: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    confirmBlackUser: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    downConfirmBlackUserLog: function (method, args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: method, args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getUserGameMoney: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getusergamemoney", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    
};
