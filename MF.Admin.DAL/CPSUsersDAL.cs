using MF.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MF.Admin.DAL
{
    public class CPSUsersDAL : BaseDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddCPSUsers(CPSUsersAdmin model)
        {
            try
            {
                var args = new SqlParameter[] {
                    new SqlParameter("@Account",model.admin_account),
                    new SqlParameter("@Password",model.admin_password),
                    new SqlParameter("@Flag",model.admin_flag),
                    new SqlParameter("@Name",model.admin_name),
                    new SqlParameter("@Token",model.admin_token),

                    new SqlParameter("@channel",model.channel),
                    new SqlParameter("@channel_name",model.channel_name),
                    new SqlParameter("@channel_num",model.channel_num),
                    new SqlParameter("@idnum",model.idnum),
                    new SqlParameter("@device",model.device),
                    new SqlParameter("@bank_name",model.bank_name),
                    new SqlParameter("@bank_addr",model.bank_addr),
                    new SqlParameter("@bank_acc",model.bank_acc),
                    new SqlParameter("@business_link",model.business_link),
                    new SqlParameter("@telephone",model.telephone),
                    new SqlParameter("@email",model.email),
                    new SqlParameter("@qq",model.qq),
                    new SqlParameter("@percent",model.percent),
                    new SqlParameter("@protocol",model.protocol)
                };
                BaseDAL.dbname = DBName.Manage;
                int cpsuser_id = DataHelper.ExecuteProcedure("mf_P_AddCPSUser", args);
                if (cpsuser_id > 0)
                    AdminDAL.LoadAdminstators();
                return cpsuser_id;
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("添加渠道用户信息异常:", e.Message, "channel:[", model.channel, "]");
            }
            return -2;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateCPSUsers(CPSUsersAdmin model)
        {
            try
            {
                var args = new SqlParameter[] {
                    new SqlParameter("@Account",model.admin_account),
                    new SqlParameter("@Password",model.admin_password),
                    new SqlParameter("@Flag",model.admin_flag),
                    new SqlParameter("@Name",model.admin_name),
                    new SqlParameter("@Token",model.admin_token),

                    new SqlParameter("@channel",model.channel),
                    new SqlParameter("@channel_name",model.channel_name),
                    new SqlParameter("@channel_num",model.channel_num),
                    new SqlParameter("@idnum",model.idnum),
                    new SqlParameter("@device",model.device),
                    new SqlParameter("@bank_name",model.bank_name),
                    new SqlParameter("@bank_addr",model.bank_addr),
                    new SqlParameter("@bank_acc",model.bank_acc),
                    new SqlParameter("@business_link",model.business_link),
                    new SqlParameter("@telephone",model.telephone),
                    new SqlParameter("@email",model.email),
                    new SqlParameter("@qq",model.qq),
                    new SqlParameter("@percent",model.percent),
                    new SqlParameter("@protocol",model.protocol),
                    new SqlParameter("@id",model.id),
                    new SqlParameter("@admin_id",model.admin_id)
                };
                BaseDAL.dbname = DBName.Manage;
                int update_rowcount = DataHelper.ExecuteProcedure("mf_P_UpdateCPSUser", args);
                if (update_rowcount > 0)
                    AdminDAL.LoadAdminstators();
                return update_rowcount;
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("修改渠道用户信息异常:", e.Message, "channel:", model.channel, ",admin_id:" + model.admin_id);
            }
            return -2;
        }

        public List<CPSUsers> GetCPSUserList(CPSUsersSearch search, out int rowCount)
        {
            rowCount = 0;
            DataTable dt = BaseDAL.GetSearchData(search, DBName.Manage, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<CPSUsers>();
            if (dt == null || dt.Rows.Count < 1)
                return null;
            foreach (DataRow dr in dt.Rows)
            {
                var u = makeCPSUsers(dr);
                list.Add(u);
            }
            return list;
        }
        public List<CPSUsers> GetALLChannelList()
        {
            int rowCount = 0;
            CPSUsersSearch search = new CPSUsersSearch();
             DataTable dt = BaseDAL.GetSearchData(search, DBName.Manage, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<CPSUsers>();
            if (dt == null || dt.Rows.Count < 1)
                return null;
            foreach (DataRow dr in dt.Rows)
            {
                var u = makeCPSUsers(dr);
                list.Add(u);
            }
            return list;
        }
        private CPSUsers makeCPSUsers(DataRow row)
        {
            CPSUsers model = new CPSUsers();
            try
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["channel"] != null)
                {
                    model.channel = row["channel"].ToString();
                }
                if (row["channel_name"] != null)
                {
                    model.channel_name = row["channel_name"].ToString();
                }
                if (row["idnum"] != null)
                {
                    model.idnum = row["idnum"].ToString();
                }
                if (row["device"] != null)
                {
                    model.device = row["device"].ToString();
                }
                if (row["bank_name"] != null)
                {
                    model.bank_name = row["bank_name"].ToString();
                }
                if (row["bank_addr"] != null)
                {
                    model.bank_addr = row["bank_addr"].ToString();
                }
                if (row["bank_acc"] != null)
                {
                    model.bank_acc = row["bank_acc"].ToString();
                }
                if (row["business_link"] != null)
                {
                    model.business_link = row["business_link"].ToString();
                }
                if (row["telephone"] != null)
                {
                    model.telephone = row["telephone"].ToString();
                }
                if (row["email"] != null)
                {
                    model.email = row["email"].ToString();
                }
                if (row["qq"] != null)
                {
                    model.qq = row["qq"].ToString();
                }
                if (row["percent"] != null && row["percent"].ToString() != "")
                {
                    model.percent = decimal.Parse(row["percent"].ToString());
                }
                if (row["admin_id"] != null && row["admin_id"].ToString() != "")
                {
                    model.admin_id = int.Parse(row["admin_id"].ToString());
                }
                if (row["protocol"] != null)
                {
                    model.protocol = row["protocol"].ToString();
                }
                if (row["channel_num"] != null && row["channel_num"].ToString() != "")
                {
                    model.channel_num = int.Parse(row["channel_num"].ToString());
                }
            }
            catch (Exception ex)
            {
                WriteError("makeCPSUsers ex:", ex.Message);
            }
            return model;
        }
    }
}
