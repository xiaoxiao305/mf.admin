using System;
using System.Collections.Generic;
using MF.Data;
using System.Web;
using System.Web.Security;
using MF.Common.Security;
using MF.Common.Json;
using System.Threading;
using MF.Admin.DAL;
using System.Web.SessionState;

namespace MF.Admin.BLL
{
    public class AdministratorTicket
    {
        public string Account { get; set; }
        public string LoginTime { get; set; }
        public string Guid { get; set; }
        public string IP { get; set; }
    }
    public class AdminBLL : Base, IRequiresSessionState, IReadOnlySessionState
    {
        private static AdminDAL dal = new AdminDAL();
        static HttpSessionState CurrentSession = HttpContext.Current.Session;
        static AdminBLL()
        {
            loginTickets = new Dictionary<string, AdministratorTicket>();
            administrators = new Dictionary<string, Administrator>();
        }
        public static void WriteSystemLog(string acc, string ip, string connect, string page, int oprstate, SystemLogEnum type)
        {
            dal.WriteSystemLog(acc, ip, connect, page, oprstate, type);
        }

        #region 登录
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="pwd">密码</param>
        /// <param name="token">安全令</param>
        /// <param name="type">管理员类型
        /// 1超级管理员
        /// 2客服
        /// 3运营
        /// 4
        /// </param>
        public static bool Login(string acc, string pwd, string token, int type)
        {
            try
            {
                if (administrators == null) administrators = new Dictionary<string, Administrator>();
                if (loginTickets == null) loginTickets = new Dictionary<string, AdministratorTicket>();
                if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(token))
                {
                    Base.WriteError("登录参数有误");
                    return false;
                }
                var admin = dal.Login(acc, pwd, token, ClientIP);
                if (admin != null && admin.ID > 0)
                {
                    if (admin.Type == 1 || (admin.Type != 1 && admin.Type == type))
                    {
                        SetLoginInfo(admin, ClientIP, 20);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("管理员登录时异常:", ex.Message, "args:[", acc, pwd, token, "]");
            }
            return false;
        }
        public static bool Login(string acc, string pwd, string token, int type,string hdmd5)
        {
            try
            {
                if (administrators == null) administrators = new Dictionary<string, Administrator>();
                if (loginTickets == null) loginTickets = new Dictionary<string, AdministratorTicket>();
                if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(token))
                {
                    Base.WriteError("登录参数有误");
                    return false;
                }
                var admin = dal.Login(acc, pwd, token, ClientIP,hdmd5);
                if (admin != null && admin.ID > 0)
                {
                    if (admin.Type == 1 || (admin.Type != 1 && admin.Type == type))
                    {
                        SetLoginInfo(admin, ClientIP, 20);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("管理员登录时异常:", ex.Message, "args:[", acc, pwd, token, "]");
            }
            return false;
        }
        public static bool LoginCPS(string acc, string pwd)
        {
            try
            {
                if (administrators == null) administrators = new Dictionary<string, Administrator>();
                if (loginTickets == null) loginTickets = new Dictionary<string, AdministratorTicket>();
                if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(pwd))
                {
                    Base.WriteError("登录参数有误");
                    return false;
                }
                var admin = dal.LoginCPS(acc, pwd, ClientIP);
                if (admin != null && admin.ID > 0)
                {
                    if (admin.Type == 1 || (admin.Type != 1 && admin.Type == 4))
                    {
                        SetLoginInfo(admin, ClientIP, 20);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("cps管理员登录时异常:", ex.Message, "args:[", acc, pwd, "]");
            }
            return false;
        }
        static Dictionary<string, AdministratorTicket> loginTickets;
        public static Dictionary<string, Administrator> administrators;
        static void SetLoginInfo(Administrator admin, string ip, int expires)
        {
            var Session = HttpContext.Current.Session;
            try
            {
                admin.Account = admin.Account.ToLower();
                Session["administrator"] = admin;
                Session["account"] = admin.Account;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(admin.Account, false, expires);
                System.Web.Security.FormsIdentity formsIdentity = new FormsIdentity(ticket);
                FormsAuthentication.SetAuthCookie(admin.Account, false);
                if (administrators.ContainsKey(admin.Account))
                    administrators.Remove(admin.Account);
                administrators.Add(admin.Account, admin);
                SetTokenCookie(admin.Account, ip, expires);
            }
            catch (ThreadAbortException) { }
            catch (Exception e)
            {
                Base.WriteError("用户登录时验证账号密码已成功, 在设置用户登录信息时出错:", e.Message);
            }
        }
        static void SetTokenCookie(string acc, string ip, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["mf.token.admin"];
            var repeatCookie = true;
            if (cookie == null)
            {
                cookie = new HttpCookie("mf.token.admin");
                repeatCookie = false;
            }
            string guid = Guid.NewGuid().ToString();
            var ticket = new AdministratorTicket { Account = acc, IP = ip, LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Guid = guid };
            if (loginTickets.ContainsKey(acc))
                loginTickets.Remove(acc);
            loginTickets.Add(acc, ticket);
            cookie.Value = AES.Encrypt(Json.SerializeObject(ticket));
            //cookie.Expires = DateTime.Now.AddMinutes(expires);
            cookie.Expires = DateTime.MaxValue;
            //Base.WriteLog("设置[",acc,"]cookie过期时间为：", cookie.Expires.ToString("yyyyMMddhhmmss"));
            cookie.HttpOnly = true;
            if (repeatCookie)
                HttpContext.Current.Response.SetCookie(cookie);
            else
                HttpContext.Current.Response.AppendCookie(cookie);
        }
        public static void SignOut()
        {
            var Session = HttpContext.Current.Session;
            FormsAuthentication.SignOut();
            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;
            var cookie = Request.Cookies["mf.token.admin"];
            if (cookie != null)
            {
                cookie.Values.Clear();
                cookie.Expires = DateTime.Now;
                Response.AppendCookie(cookie);
                Response.SetCookie(cookie);
            }
            try
            {
                Session.Clear();
                Session.Abandon();
            }
            catch (ThreadAbortException) { }
            var acc = Session["account"] as string;
            var admin = Session["administrator"] as Administrator;
            if (string.IsNullOrEmpty(acc) || admin == null) return;
            if (loginTickets.ContainsKey(acc)) loginTickets.Remove(acc);
            if (loginTickets.ContainsKey(admin.Account)) loginTickets.Remove(admin.Account);
            if (administrators.ContainsKey(acc)) administrators.Remove(acc);
            if (administrators.ContainsKey(admin.Account)) administrators.Remove(admin.Account);
            Session["account"] = null;
            Session["administrator"] = null;
        }
        public static bool CheckIsLogin()
        {
            var Session = HttpContext.Current.Session;
            if (Session == null)
                Session = CurrentSession;
            var Request = HttpContext.Current.Request;
            try
            {
                if (Session["account"] == null || Session["administrator"] == null) return false;
                var acc = Session["account"] as string;
                var admin = Session["administrator"] as Administrator;
                if (string.IsNullOrEmpty(acc) || admin == null) return false;
                if (loginTickets == null || !loginTickets.ContainsKey(acc)) return false;
                if (administrators == null || !administrators.ContainsKey(acc)) return false;
                if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null) return false;
                var userIdentiy = HttpContext.Current.User.Identity as FormsIdentity;
                if (userIdentiy == null || userIdentiy.Ticket == null) return false;
                if (userIdentiy.IsAuthenticated && userIdentiy.Ticket.Name == admin.Account && acc == admin.Account)
                {
                    HttpCookie cookie = Request.Cookies["mf.token.admin"];
                    if (cookie == null)
                    {
                        Base.WriteError("验证管理员登录信息时未找到", admin.Account, "的cookies");
                        return false;
                    }
                    var cookieValue = cookie.Value;
                    if (string.IsNullOrEmpty(cookieValue)) { Base.WriteError("验证管理员[", admin.Account, "]登录信息时[", admin.Account, "]的cookies为空") ; return false; }
                    string tickets = "";
                    try
                    {
                        tickets = AES.Decrypt(cookieValue);
                        if (string.IsNullOrEmpty(tickets)) { Base.WriteError("验证管理员登录信息时解密[", admin.Account, "]的cookies为空!cookies:", cookieValue); return false; }
                    }
                    catch (Exception ex)
                    {
                        Base.WriteError("验证管理员登录信息时解密[", admin.Account, "]的cookies异常!cookies:", cookieValue, "Error:", ex.Message);
                        return false;
                    }
                    var token = new AdministratorTicket();
                    try
                    {
                        token = Json.DeserializeObject<AdministratorTicket>(tickets);
                    }
                    catch (Exception ex)
                    {
                        Base.WriteError("验证管理员登录信息时将[", tickets, "]序列化为[AdministratorTicket]对象时异常!Error:", ex.Message);
                        return false;
                    }
                    if (token == null || token.Account != acc)
                    {
                        Base.WriteError("验证管理员登录信息时用户cookies中的账号与系统登录的账号不一致,cookies中的账号:", token.Account, "系统中的登录账号:", acc);
                        return false;
                    }

                    if (loginTickets.ContainsKey(acc))
                    {
                        if (token.LoginTime == loginTickets[acc].LoginTime && token.Guid == loginTickets[acc].Guid)
                            return true;
                        else
                        {
                            try
                            {
                                Base.WriteError("验证管理员[", acc, "]登录信息时cookes中的Ticket与服务器的用户Ticket不一致", Json.SerializeObject(token), Json.SerializeObject(loginTickets[acc]));
                            }
                            catch { }
                        }
                    }
                    else
                        Base.WriteError("验证管理员登录信息时系统中没有", admin.Account, "的Tickets");
                }
                return false;
            }
            catch (Exception ex)
            {
                Base.WriteError("验证管理员登录时异常:", ex.Message);
                return false;
            }
        }
        #endregion
    }
}
