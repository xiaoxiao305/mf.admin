using MF.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
  public  class GameDAL
    {

        public static string BlackURI = ConfigurationManager.AppSettings["BlackURI"];
        public static string ShiChuiURI = ConfigurationManager.AppSettings["ShiChuiURI"];
        public static int AddBlackUser(string[] gameidList, string chargeId,
         string[] valueList, string[] levelStrList, string remark, long isConfirm)
        {
            try
            {
                if (gameidList == null || gameidList.Length < 1 || string.IsNullOrEmpty(chargeId)
                || valueList == null || valueList.Length < 1
                || levelStrList == null || levelStrList.Length < 1) return -1;
                string gameId = "";
                string value = "";
                string levelStr = "";
                string account = "";
                if (isConfirm == 1)
                    account = new UserDAL().GetAccByChargeId(chargeId);
                for (int i = 0; i < gameidList.Length; i++)
                {
                    Thread.Sleep(1000);
                    gameId = gameidList[i];
                    value = valueList[i];
                    levelStr = levelStrList[i];
                    value = "[" + value + "]";
                    var r = AddBlackUser(gameId, chargeId, value, levelStr, remark);
                    if (r != null)
                    {
                        if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                        {
                            if (bool.Parse(r["succeed"].ToString()))
                            {

                                if (isConfirm == 1)
                                {
                                    var conR = ConfirmBlackUser(account, chargeId, "NONE_" + gameId);
                                    if (conR != null)
                                        return 1;
                                    else
                                        return -4;
                                }
                                return 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL AddBlackUser ex:"+ ex.Message + " gameidList:" + String.Join(",", gameidList) +
                    " chargeId:" + chargeId + " valueList:" + String.Join(",", valueList)
                   + " levelStrList:" + String.Join(",", levelStrList) + " remark:", remark);
                return -3;
            }
            return -2;
        }
        public static Dictionary<string, string> AddBlackUser(string gameId, string chargeId, string value, string levelStr, string remark)
        {
            string param = string.Format("gameId={0}&chargeid={1}&value={2}&level={3}&remark={4}", gameId, chargeId, value, levelStr, remark);
            var res = PostHelper.Post<Dictionary<string, string>>(BlackURI + "adduser", param);
            return res;
        }
        public static Dictionary<string, string> ConfirmBlackUser(string account, string chargeid, string confirmData)
        {
            try
            {
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(chargeid) || string.IsNullOrEmpty(confirmData)) return null;
                Dictionary<string, string> r = ConfirmBlackUserDAL(account, chargeid, confirmData);
                int oprState = 0;
                string msg = string.Format("审核黑名单，确认实锤{0}数据为{1}失败。", chargeid, confirmData);
                if (r != null)
                {
                    if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                    {
                        if (bool.Parse(r["succeed"].ToString()))
                        {
                            oprState = 1;
                            msg = string.Format("审核黑名单，确认实锤{0}数据为{1}成功。", chargeid, confirmData);
                        }
                        else
                            msg += r["message"];
                    }
                    else
                        msg += " res is err";
                }
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL ConfirmBlackUser ex:"+ ex.Message);
            }
            return null;
        }
        public static Dictionary<string, string> ConfirmBlackUserDAL(string account, string chargeid, string confirmData)
        {
            string param = string.Format("account={0}&chargeid={1}&indexs={2}", account, chargeid, confirmData);
            var res = PostHelper.Post<Dictionary<string, string>>(ShiChuiURI + "api/game/shichui", param);
            return res;
        }
        public static List<GameBlackUserInfo> GetGameBlackUsers(long gameId, string account, string chargeid, int audit)
        {
            string par = "gameId={0}&account={1}&chargeid={2}&audit={3}";
            string gameid = gameId > 0 ? gameId.ToString() : "";
            string auditTag = audit == 2 ? "NO" : "YES";
            par = string.Format(par, gameid, account, chargeid, auditTag);
            //WriteLog("getblackuser par:", par);
            var res = PostHelper.Post<List<GameBlackUserInfo>>(BlackURI + "getusers", par);
            return res;
        }
    }
}
