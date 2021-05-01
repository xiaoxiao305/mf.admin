var ajax = {
    url: "/m/ajax.ashx",
    login: function () { },
    signOut: function () { $.ajax({ url: this.url, data: { m: "out", args: "[]", r: Math.random() }, success: function () { parent.location.href = "/"; }, error: function (xhr, status, err) { parent.location.href = "/"; } }); },
    getAdminHdInfo: function (method, args, callback) {
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
    getAllStrongBoxRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "allstrongboxrecord", args: JSON.stringify(args), r: Math.random() },
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
    getExtendChannelKeywordRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "extendchannelkeywordrecord",
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
    getSystemLogRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "systemlogrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getLoginLogRecord: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "loginlogrecord", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getadidregReport: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "adidregreport", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getbaiduadReport: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "baiduadreport", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    getKeywordsList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "gamekeywordslist",
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
    setGameKeyword: function (method, args, callback) {
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
            data: { m: method,  r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    setChargeActiveState: function (method, args, callback) {
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
    getChargeActiveState: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "getchargeactivestate",
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
    getRecommondClubsList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "getrecommondclubslist",
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
    setPushNews: function (method, args, callback) {
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
    getLeagueClubMembersList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getleagueclubmemberslist", args: JSON.stringify(args), r: Math.random() },
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
    deleteClub: function (method, args, callback) {
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
    closeRoom: function ( args, callback) {
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
    setGameSetting: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "setgamesetting", args: JSON.stringify(args), r: Math.random() },
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
    getClubMembersList: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getclubmemberslist", args: JSON.stringify(args), r: Math.random() },
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
    GetDeskMates: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getdeskmates", args: JSON.stringify(args), r: Math.random() },
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





    setRedAlert: function (method, args, callback) {
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

    getRedAlert: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getredalert", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    //delRedAlert: function (method, args, callback) {
    //    $.ajax({
    //        url: ajax.url,
    //        data: { m: method, args: JSON.stringify(args), r: Math.random() },
    //        dataType: "json",
    //        cache: false,
    //        success: callback,
    //        error: function (xhr, status, err) {
    //            $("#loading").hide();
    //            alert(err);
    //        }
    //    });
    //},
    getRedAlertPlayer: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getredalertplayer", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    GetGameMoney: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getgamemoney", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    SendBroadCast: function (method, args, callback) {
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
    getNewGameUsers: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "getnewgameusers", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    kickClubMembers: function (method, args, callback) {
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
    existLeague: function (method, args, callback) {
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
    getHighTaxClub: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: { m: "gethightaxclub", args: JSON.stringify(args), r: Math.random() },
            dataType: "json",
            cache: false,
            success: callback,
            error: function (xhr, status, err) {
                $("#loading").hide();
                alert(err);
            }
        });
    },
    addHighTaxClub: function (method, args, callback) {
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
    setHighTaxClub: function (method, args, callback) {
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
    delHighTaxClub: function (method, args, callback) {
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
    getSponsor: function (args, callback) {
        $.ajax({
            url: ajax.url,
            data: {
                m: "getsponsor",
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
    setSponsor: function (method, args, callback) {
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
    }, delSponsor: function (method, args, callback) {
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
    }, joinClub: function (method, args, callback) {
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
};
