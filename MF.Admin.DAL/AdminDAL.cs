using System;
using System.Collections.Generic;
using MF.Data;
using System.Data;
using MF.Common.Security;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;

namespace MF.Admin.DAL
{
    public class AdminDAL : BaseDAL
    {
        public static Dictionary<string, Administrator> administartors;
        static AdminDAL()
        {
            BaseDAL.dbname = DBName.Manage;
            administartors = new Dictionary<string, Administrator>();
            LoadAdminstators();
        }
        /// <summary>
        /// 将管理员的操作日志写入数据库
        /// </summary>
        /// <param name="acc">管理员账号</param>
        /// <param name="ip">操作IP</param>
        /// <param name="connect">操作内容</param>
        /// <param name="page">操作的页面->事件</param>
        /// <param name="oprstate">操作状态 1成功 否则失败</param>
        public void WriteSystemLog(string acc, string ip, string connect, string page, int oprstate, SystemLogEnum type)
        {
            try
            {
                var args = new SqlParameter[] {
                    new SqlParameter("@Account",acc),
                    new SqlParameter("@IP",ip),
                    new SqlParameter("@Content",connect),
                    new SqlParameter("@Page",page),
                    new SqlParameter("@oprstate",oprstate),
                    new SqlParameter("@type",type)
                };
                BaseDAL.dbname = DBName.Manage;
                DataHelper.ExecuteProcedure("mf_P_SystemLog", args);
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("写入数据库操作日志异常:", e.Message, "args:[", acc, ip, connect, page, "]");
            }
        }
        public void WriteLoginLog(string acc, string ip, int flag, string msg)
        {
            try
            {
                var args = new SqlParameter[] {
                    new SqlParameter("@Account",acc),
                    new SqlParameter("@IP",ip),
                    new SqlParameter("@Flag",flag),
                    new SqlParameter("@Message",msg)
                };
                BaseDAL.dbname = DBName.Manage;
                DataHelper.ExecuteProcedure("mf_P_LoginLog", args);
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("写入数据库登录日志时异常:", e.Message, "args:[", acc, ip, flag, msg, "]");
            }
        }
        public Dictionary<string, List<string[]>> GetMatchMap()
        {
            if (Cache.MatchList == null || Cache.MatchList.Count < 1)
                Cache.LoadGameList();
            return Cache.MatchList;
        }
        public Dictionary<string, string> GetGameMap()
        {
            if (Cache.GameList == null || Cache.GameList.Count < 1)
                Cache.LoadGameList();
            return Cache.GameList;
        }
        public static void LoadAdminstators()
        {
            administartors = new Dictionary<string, Administrator>();
            //var sql = "select ID,Account, [Password], Flag, Name, LastIP, LastLogin, Token, Powers,[Type],hd from [Admin]";
            var sql = "select ID,Account, [Password], Flag, Name, LastIP, LastLogin, Token, Powers,[Type],IsAdmin from [Admin]";
            var dt = GetDataTable(sql, DBName.Manage);
            if (dt == null || dt.Rows.Count < 1)
                return;
            foreach (DataRow dr in dt.Rows)
            {
                var admin = new Administrator();
                admin.Account = dr["Account"].ToString();
                if (dr["Flag"] != null && !string.IsNullOrEmpty(dr["Flag"].ToString()))
                    admin.Flag = int.Parse(dr["Flag"].ToString());
                if (dr["ID"] != null && !string.IsNullOrEmpty(dr["ID"].ToString()))
                    admin.ID = int.Parse(dr["ID"].ToString());
                admin.LastIP = dr["LastIP"].ToString();
                if (dr["LastLogin"] != null && !string.IsNullOrEmpty(dr["LastLogin"].ToString()))
                    admin.LastLogin = int.Parse(dr["LastLogin"].ToString());
                admin.Name = dr["Name"].ToString();
                admin.Password = dr["Password"].ToString();
                admin.Powers = dr["Powers"].ToString();
                admin.Token = dr["Token"].ToString();
                if (dr["Type"] != null && !string.IsNullOrEmpty(dr["Type"].ToString()))
                    admin.Type = int.Parse(dr["Type"].ToString());
                if (dr["Type"] != null && !string.IsNullOrEmpty(dr["Type"].ToString()))
                    admin.Type = int.Parse(dr["Type"].ToString());
                if (dr["IsAdmin"] != null && !string.IsNullOrEmpty(dr["IsAdmin"].ToString()))
                    admin.IsAdmin = int.Parse(dr["IsAdmin"].ToString());
                // admin.hd = dr["hd"].ToString();
                if (administartors.ContainsKey(admin.Account.ToLower()))
                    administartors[admin.Account.ToLower()] = admin;
                else
                    administartors.Add(admin.Account.ToLower(), admin);
            }
        }
        public Administrator LoginCPS(string acc, string pwd, string ip)
        {
            AdminDAL log = new AdminDAL();
            LoadAdminstators();
            acc = acc.ToLower();
            if (!administartors.ContainsKey(acc))
            {
                log.WriteLoginLog(acc, ip, 0, "cps账号不存在");
                return null;
            }
            if (administartors[acc].Flag == 0)
            {
                log.WriteLoginLog(acc, ip, 0, "cps账号已冻结");
                return null;
            }
            if (administartors[acc].Password.ToLower() != pwd.ToLower())
            {
                log.WriteLoginLog(acc, ip, 0, "cps密码错误");
                return null;
            }
            log.WriteLoginLog(acc, ip, 1, "");
            //var admin = new Administrator { LastIP = administartors[acc].LastIP, Account = administartors[acc].Account, Flag = administartors[acc].Flag, ID = administartors[acc].ID, LastLogin = administartors[acc].LastLogin, Name = administartors[acc].Name, Token = administartors[acc].Token, Type = administartors[acc].Type,hd = administartors[acc].hd };
            var admin = administartors[acc];
            administartors[acc].LastIP = ip;
            administartors[acc].LastLogin = ConvertDateToSpan(DateTime.Now, "s");
            return admin;
        }
        public Administrator Login(string acc, string pwd, string token, string ip, string hdmd5)
        {
            AdminDAL log = new AdminDAL();
            if (administartors == null || administartors.Count < 1) LoadAdminstators();
            acc = acc.ToLower();
            if (administartors == null || administartors.Count < 1)
            {
                log.WriteLoginLog(acc, ip, 0, "账号不存在2");
                return null;
            }
            if (!administartors.ContainsKey(acc))
            {
                log.WriteLoginLog(acc, ip, 0, "账号不存在");
                return null;
            }
            if (administartors[acc].Flag == 0)
            {
                log.WriteLoginLog(acc, ip, 0, "账号已冻结");
                return null;
            }
            if (administartors[acc].Password.ToLower() != pwd.ToLower())
            {
                log.WriteLoginLog(acc, ip, 0, "密码错误");
                return null;
            }
            //if (!Token.CheckMD5(hdmd5, administartors[acc].hd))
            //{
            //    log.WriteLoginLog(acc, ip, 0, "设备信息错误.HD:" + hdmd5);
            //    return null;
            //}
            if (!IsDebug)
            {

                if (!Token.CheckToken(token, administartors[acc].Token))
                {
                    log.WriteLoginLog(acc, ip, 0, "手机安全令动态密码错误");
                    return null;
                }
            }
            log.WriteLoginLog(acc, ip, 1, "");
            //  var admin = new Administrator { LastIP = administartors[acc].LastIP, Account = administartors[acc].Account, Flag = administartors[acc].Flag, ID = administartors[acc].ID, LastLogin = administartors[acc].LastLogin, Name = administartors[acc].Name, Token = administartors[acc].Token, Type = administartors[acc].Type };
            var admin = administartors[acc];
            administartors[acc].LastIP = ip;
            administartors[acc].LastLogin = ConvertDateToSpan(DateTime.Now, "s");
            return admin;
        }
        public Administrator Login(string acc, string pwd, string token, string ip)
        {
            AdminDAL log = new AdminDAL();
            if (administartors == null || administartors.Count < 1) LoadAdminstators();
            acc = acc.ToLower();
            if (administartors == null || administartors.Count < 1)
            {
                log.WriteLoginLog(acc, ip, 0, "账号不存在1");
                return null;
            }
            if (!administartors.ContainsKey(acc))
            {
                log.WriteLoginLog(acc, ip, 0, "账号不存在");
                return null;
            }
            if (administartors[acc].Flag == 0)
            {
                log.WriteLoginLog(acc, ip, 0, "账号已冻结");
                return null;
            }
            if (administartors[acc].Password.ToLower() != pwd.ToLower())
            {
                log.WriteLoginLog(acc, ip, 0, "密码错误");
                return null;
            }
            if (!IsDebug)
            {
                if (!Token.CheckToken(token, administartors[acc].Token))
                {
                    log.WriteLoginLog(acc, ip, 0, "手机安全令动态密码错误");
                    return null;
                }
            }
            log.WriteLoginLog(acc, ip, 1, "");
            var admin = new Administrator { LastIP = administartors[acc].LastIP, Account = administartors[acc].Account, Flag = administartors[acc].Flag, ID = administartors[acc].ID, LastLogin = administartors[acc].LastLogin, Name = administartors[acc].Name, Token = administartors[acc].Token, Type = administartors[acc].Type ,IsAdmin=administartors[acc].IsAdmin};
            administartors[acc].LastIP = ip;
            administartors[acc].LastLogin = ConvertDateToSpan(DateTime.Now, "s");
            return admin;
        }
        public List<Administrator> GetAdministatorList()
        {
            if (administartors == null || administartors.Count < 1) LoadAdminstators();
            var list = new List<Administrator>();
            foreach (var k in administartors.Keys)
                list.Add(administartors[k]);
            return list;
        }
    }
}
