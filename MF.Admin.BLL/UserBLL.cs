using System;
using System.Collections.Generic;
using MF.Data;
using MF.Admin.DAL;
using System.Text.RegularExpressions;
using MF.Common.Security;
using System.Linq;

namespace MF.Admin.BLL
{
    public class UserTest
    {
        public string account { get; set; }
        public int id { get; set; }
        public string identity { get; set; }
    }
    public class UserBLL : Base
    {
        private static UserDAL dal = new UserDAL();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="searchdbType">1备份数据库 2实时数据库</param>
        /// <param name="dbsource">1备份数据库 2生成数据库</param>
        /// <param name="dbname">-1全库查找 100USER_0 ...</param>
        /// <param name="exact">1精确查找</param>
        /// <param name="queryFiled"></param>
        /// <param name="keyword"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Users> GetUserList2(long pageSize, long pageIndex, long dbsource, long dbname,
            long exact, long queryFiled, string keyword, out int rowCount)
        {
            rowCount = 0;
            if (dbsource == 1)//备份数据库
                return GetBackUserList(pageSize, pageIndex, dbname, exact, queryFiled, keyword, out rowCount);
            else if (dbsource == 2)//生产数据库
            {
                if (string.IsNullOrEmpty(keyword.Trim()))
                    return null;
                if (queryFiled == 1)//UID
                    return dal.GetUserList("", keyword, out rowCount);
                else if (queryFiled == 2)//账号
                    return dal.GetUserList(keyword, "", out rowCount);
                else if (queryFiled == 3)//GUID
                {
                    Users user = new Users() { GUID=keyword};
                    return dal.GetUserList(user, out rowCount);
                }
                
            }
            return null;
        }
        public static List<Users> GetUserList(long pageSize, long pageIndex, long dbsource, long dbname,
          long exact, long queryFiled, string keyword, out int rowCount)
        {
            rowCount = 0;
            List<Users>  list=GetUserList2(pageSize, pageIndex, dbsource, dbname,exact, queryFiled, keyword,out rowCount);
            if (list == null || list.Count < 1) return list;
            List<int> ids = list.Select(t => t.ID).ToList();
            if (ids == null || ids.Count < 1) return list;
            List<TotalWinLose> totalList = dal.GetTotalWinLoseList(ids,dbsource,(DBName)dbname); 
            if     (totalList == null || totalList.Count < 1) return list;
            List<Users> newList = new List<Users>();
            foreach (Users u in list)
            {
                foreach (TotalWinLose twl in totalList)
                {
                    if (u.ID == twl.ID)
                    {
                        u.TotalWinLoseValue = twl.Value;
                        break;
                    }
                }
                newList.Add(u);
            }
            foreach (Users uuu in newList)
            {
                WriteError("newList LIST uuu:", uuu.TotalWinLoseValue.ToString());
            }
            return newList;
        }
        private static List<Users> GetBackUserList(long pageSize, long pageIndex, long dbname,
            long exact, long queryFiled, string keyword, out int rowCount)
        {
            var search = new UserSearch();
            rowCount = 0;
            var res = new PagerResult<List<Users>>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            search.Where = " IsBot=0 and [Master]='' ";
            if (keyword.Trim() != "")
            {
                if (exact == 1)
                {
                    if (queryFiled == 1)
                        search.Where += string.Format(" and ChargeId='{0}'", keyword);
                    else if (queryFiled == 2)
                        search.Where += string.Format(" and Account='{0}'", keyword);
                    else if (queryFiled == 3)
                        search.Where += string.Format(" and Nickname='{0}'", keyword);
                    else if (queryFiled == 4)
                        search.Where += string.Format(" and Mobile='{0}'", keyword);
                    else if (queryFiled == 5)
                        search.Where += string.Format(" and [Identity]='{0}'", keyword);
                    else if (queryFiled == 6)
                        search.Where += string.Format(" and GUID='{0}'", keyword);
                }
                else
                {
                    if (queryFiled == 1)
                        search.Where += string.Format(" and ChargeId LIKE '%{0}%'", keyword);
                    else if (queryFiled == 2)
                        search.Where += string.Format(" and Account LIKE '%{0}%'", keyword);
                    else if (queryFiled == 3)
                        search.Where += string.Format(" and Nickname LIKE '%{0}%'", keyword);
                    else if (queryFiled == 4)
                        search.Where += string.Format(" and Mobile LIKE '%{0}%'", keyword);
                    else if (queryFiled == 5)
                        search.Where += string.Format(" and [Identity] LIKE '%{0}%'", keyword);
                    else if (queryFiled == 6)
                        search.Where += string.Format(" and GUID LIKE '%{0}%'", keyword);
                }
            }
            search.DBName = (DBName)dbname;
            //WriteLog("UserBLL GetUserList start.dbname:", dbname.ToString(),",where:",search.Where);
            return dal.GetBackUserList(search, out rowCount);
        }
        public static Result<Users> GetUserInfo(DBSource dbsource, string account)
        {
            if (string.IsNullOrEmpty(account))
                return null;
            return dal.GetUserInfo(dbsource, account);
        }
        public static List<Users> GetSubUserList(long pageSize, long pageIndex, string account, out int rowCount)
        {
            rowCount = 0;
            if (string.IsNullOrEmpty(account))
                return null;
            var search = new Search<Users>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            Users model = new Users();
            model.Master = account;
            search.SearchObj = model;
            return dal.GetSubUserList(search, out rowCount);
        }

        #region  修改密码
        public static int UpdatePwd(string account, string pwd)
        {
            int pwdLv = GetPwdLevel(pwd);
            int res = dal.UpdatePwd(account, MD5.Encrypt(pwd), pwdLv);
            string msg = res == 1 ? string.Format("操作账号{0}【修改密码】成功", account) : string.Format("操作账号{0}【修改密码】失败", account);
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "UserBLL.UpdatePwd", res == 1 ? res : 0, SystemLogEnum.UPDATEPWD);
            return res;
        }
        //密码--->密码强度
        private static int GetPwdLevel(string pwd)
        {
            bool isEng = false;
            bool isNum = false;
            bool isSpecial = false;
            int level = 0;
            for (int i = 0; i < pwd.Length; i++)
            {
                string p = pwd[i].ToString();
                if (!isEng && (new Regex("^[a-z]+$").IsMatch(p) || new Regex("^[A-Z]+$").IsMatch(p)))
                {
                    isEng = true;
                    level++;
                }
                if (!isNum && (new Regex("^[0-9]+$").IsMatch(p)))
                {
                    isNum = true;
                    level++;
                }
                if (!isSpecial && ValidSpecialTag(p))
                {
                    isSpecial = true;
                    level++;
                }
            }
            return level;
        }
        //特殊字符
        private static bool ValidSpecialTag(string tag)
        {
            string filterstring = "<,>./?:;\"'{[}]|\\+=-))(*&^%$#@!` 　";
            string[] a = tag.Split(' ');
            for (int i = 0; i < a.Length; i++)
            {
                if (filterstring.IndexOf(a[i]) >= 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region 操作用户
        public static int SetUserInfo(string account, long type)
        {
            int res = dal.SetUserInfo(account, type);
            string[] typearray = new string[] { "","", "解绑手机号",
                "解绑安全令", "解除本机锁定", "解除安全令锁定", "冻结账号", "解冻账号" };
            string msg = "";
            if (res == 1)
                msg = string.Format("操作账号{0}【{1}】成功", account, typearray[type]);
            else
                msg = string.Format("操作账号{0}【{1}】失败", account, typearray[type]);            
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "UserBLL.SetUserInfo", res == 1 ? res : 0, (SystemLogEnum)type);
            return res;
        }
        #endregion

        public static int SetUserMoney(string account, long type, long num)
        {
            if (string.IsNullOrEmpty(account) || (num < 1 || num > 100000000)
                || (type != 1 && type != 2 && type != 3))
                return -2001;
            int res = dal.SetUserMoney(account, type, num);
            string[] typearray = new string[] { "", "加元宝", "加金豆","加用户房卡" };
            string msg = "";
            if (res == 1)
                msg = string.Format("操作账号{0}【{1}】成功", account, typearray[type]);
            else
                msg = string.Format("操作账号{0}【{1}】失败", account, typearray[type]);
            SystemLogEnum oprtypeenum = SystemLogEnum.UNDEFINE;
            if (type == 1)
                oprtypeenum = SystemLogEnum.ADDCURRENCY;
            else if (type == 2)
                oprtypeenum = SystemLogEnum.ADDBEN;
            else if (type == 3)
                oprtypeenum = SystemLogEnum.ADDROOMCARD;
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "UserBLL.SetUserMoney",res==1?res:0,oprtypeenum);
            return res;
        }


        public static ClubsRes<Dictionary<string,object>> SetChargeIdCache(string[] chargeids)
        {
            var chargeidExcept = chargeids;
            if (Cache.CacheChargeidList != null)
                chargeidExcept = chargeids.Except(Cache.CacheChargeidList.Keys.ToArray()).ToArray();
          return  dal.QueryUserList(chargeidExcept);
        }
        public static Users GetChargeIdCache(string chargeid)
        {
            return dal.GetCacheUserByChargeIdFromCache(chargeid);
        }
    }
}
