using MF.Admin.DAL;
using MF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MF.Admin.BLL
{
    public class CPSUsersBLL
    {

        private static CPSUsersDAL dal = new CPSUsersDAL();
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddCPSUsers(CPSUsersAdmin model)
        {
            if (model == null)
                return -1;
            return dal.AddCPSUsers(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateCPSUsers(CPSUsersAdmin model)
        {
            if (model == null || model.id < 1)
                return -1;
            return dal.UpdateCPSUsers(model);
        }

        public static List<CPSUsersAdmin> GetCPSUserList(long pageSize, long pageIndex, long exact, long queryFiled, string keyword, out int rowCount)
        {
            var userlist = GetCPSUsers(pageSize, pageIndex, exact, queryFiled, keyword, out rowCount);
            if (rowCount < 1)
                return null;
            Dictionary<int, int> dic_admin = new Dictionary<int, int>();
            Dictionary<int, CPSUsersAdmin> dic_all = new Dictionary<int, CPSUsersAdmin>();
            foreach (CPSUsers cpsusermodel in userlist)
            {
                if (dic_all.ContainsKey(cpsusermodel.id))
                    continue;
                CPSUsersAdmin m = new CPSUsersAdmin()
                {
                    admin_id = cpsusermodel.admin_id,
                    bank_acc = cpsusermodel.bank_acc,
                    bank_addr = cpsusermodel.bank_addr,
                    bank_name = cpsusermodel.bank_name,
                    business_link = cpsusermodel.business_link,
                    channel = cpsusermodel.channel,
                    channel_name = cpsusermodel.channel_name,
                    channel_num = cpsusermodel.channel_num,
                    device = cpsusermodel.device,
                    email = cpsusermodel.email,
                    id = cpsusermodel.id,
                    idnum = cpsusermodel.idnum,
                    percent = cpsusermodel.percent,
                    protocol = cpsusermodel.protocol,
                    qq = cpsusermodel.qq,
                    telephone = cpsusermodel.telephone
                };
                if (cpsusermodel.admin_id > 0 && !dic_admin.ContainsKey(cpsusermodel.admin_id))
                    dic_admin.Add(cpsusermodel.admin_id, cpsusermodel.id);
                dic_all.Add(cpsusermodel.id, m);
            }
            Dictionary<string, Administrator> administartors = AdminDAL.administartors;
            if (administartors == null || administartors.Count < 1)
                return dic_all.Values.ToList<CPSUsersAdmin>();
            List<Administrator> adminlist = administartors.Values.ToList<Administrator>();
            int cpsid;
            foreach (Administrator adminmodel in adminlist)
            {
                if (adminmodel.ID < 1 || !dic_admin.ContainsKey(adminmodel.ID))
                    continue;
                cpsid = dic_admin[adminmodel.ID];
                dic_all[cpsid].admin_account = adminmodel.Account;
                dic_all[cpsid].admin_flag = adminmodel.Flag;
                dic_all[cpsid].admin_name = adminmodel.Name;
                dic_all[cpsid].admin_password = adminmodel.Password;
                dic_all[cpsid].admin_token = adminmodel.Token;
            }
            return dic_all.Values.ToList<CPSUsersAdmin>();
        }
        public static List<CPSUsers> GetCPSUsers(long pageSize, long pageIndex, long exact, long queryFiled, string keyword, out int rowCount)
        {
            var search = new CPSUsersSearch();
            rowCount = 0;
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            search.Where = " 1=1 ";
            if (keyword.Trim() != "")
            {
                if (exact == 1)
                {
                    if (queryFiled == 1)
                        search.Where += string.Format(" and channel='{0}'", keyword);
                    else if (queryFiled == 2)
                        search.Where += string.Format(" and channel_name='{0}'", keyword);
                    else if (queryFiled == 3)
                        search.Where += string.Format(" and id={0}", keyword);
                }
                else
                {
                    if (queryFiled == 1)
                        search.Where += string.Format(" and channel LIKE '%{0}%'", keyword);
                    else if (queryFiled == 2)
                        search.Where += string.Format(" and channel_name LIKE '%{0}%'", keyword);
                }
            }
            return dal.GetCPSUserList(search, out rowCount);
        }
        public static List<CPSUsers> GetALLChannelList()
        {
            return dal.GetALLChannelList();
        }
        public static CPSUsersAdmin GetCPSUserModel(int id, out int rowCount)
        {
            List<CPSUsersAdmin> list = CPSUsersBLL.GetCPSUserList(1, 1, 1, 3, id.ToString(), out rowCount);
            if (rowCount < 1)
                return null;
            else return list[0];
        }
        public static CPSUsersAdmin GetCPSUserModel(string admin_account)
        {
            int rowCount = 0;
            List<CPSUsersAdmin> list = CPSUsersBLL.GetCPSUserList(10000, 1, 0, 0, "", out rowCount);
            if (rowCount < 1)
                return null;
            foreach (CPSUsersAdmin model in list)
            {
                if (string.IsNullOrEmpty(model.admin_account))
                    continue;
                if (model.admin_account.ToUpper().Equals(admin_account.ToUpper()))
                    return model;
            }
            return null;
        }
    }
}
