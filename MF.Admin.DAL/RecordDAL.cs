using System;
using System.Collections.Generic;
using MF.Data;
using System.Data;
using MF.Protocol;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MF.Admin.DAL
{
    public class RecordDAL : BaseDAL
    {
        /// <summary>
        /// 元宝变更记录、金豆记录、欢乐卡记录
        /// </summary>  
        public List<CurrencyRecord> GetCurrcryRecord(Search<CurrencyRecord> search,long searchtype, out int rowCount)
        {
            rowCount = 0;
            try
            {
                SearchCondition<CurrencyRecord> current = SearchCondition<CurrencyRecord>.Current;
                CurrencyRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                current.AddInt(p => p.GameId, model.GameId);
                current.AddInt(p => p.MatchId, model.MatchId);
                current.AddInt(p => p.RuleId, model.RuleId);
                current.AddInt(p => p.Type, model.Type);
                if (search.IsChkTime)
                    current.AddBetween(p => p.Time, search.StartTime, search.OverTime);
                string protocolname = searchtype == 1 ? "get_currency_list" 
                    : searchtype == 2 ? "get_bean_list" :
                    searchtype == 3 ? "get_happycard_list" : "";
                if (string.IsNullOrEmpty(protocolname)) return null;
                var res = PostRecordServer<CurrencyRecord>(RecordServerUrl + protocolname, current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_currency_list ex:", ex.Message);
            }
            return null;
        }
       
        /// <summary>
        ///j金豆记录
        /// </summary>  
        public List<BeanRecord> GetUserBeanRecord(Search<BeanRecord> search, out int rowCount)
        {
            rowCount = 0; try
            {
                SearchCondition<BeanRecord> current = SearchCondition<BeanRecord>.Current;
                BeanRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                current.AddInt(p => p.GameId, model.GameId);
                current.AddInt(p => p.MatchId, model.MatchId);
                current.AddInt(p => p.Type, model.Type);
                if (search.IsChkTime)
                    current.AddBetween(p => p.Time, search.StartTime, search.OverTime);
                var res = PostRecordServer<BeanRecord>(RecordServerUrl + "get_bean_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_bean_list ex:", ex.Message);
            }
            return null;
        }

        /// <summary>
        ///二级密码记录
        /// </summary>  
        public List<CurrencyRecord> GetStrongBoxRecord(Search<CurrencyRecord> search, out int rowCount)
        {
            rowCount = 0; try
            {
                SearchCondition<CurrencyRecord> current = SearchCondition<CurrencyRecord>.Current;
                CurrencyRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                if (model.Type > 0)
                    current.AddInt(p => p.Type, model.Type);
                else
                    current.AddBetween(p => p.Type, 18, 19);
                if (search.IsChkTime)
                    current.AddBetween(p => p.Time, search.StartTime, search.OverTime);
                var res = PostRecordServer<CurrencyRecord>(RecordServerUrl + "get_currency_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_currency_list--->strongbox ex:", ex.Message);
            }
            return null;
        }

        /// <summary>
        ///所有二级密码记录
        /// </summary>  
        public List<StrongBoxRecord> GetAllStrongBoxRecord(Search<StrongBoxRecord> search, out int rowCount)
        {
            rowCount = 0; try
            {
                SearchCondition<StrongBoxRecord> current = SearchCondition<StrongBoxRecord>.Current;
                StrongBoxRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                current.Add(p => p.ChargeId, model.ChargeId);
                if (model.Type > 0)
                    current.AddInt(p => p.Type, model.Type);
                if (search.IsChkTime)
                    current.AddBetween(p => p.Date, search.StartTime, search.OverTime);
                var res = PostRecordServer<StrongBoxRecord>(RecordServerUrl + "get_strongbox_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_strongbox_list--->strongbox ex:", ex.Message);
            }
            return null;
        }
        public List<QmallRecord> GetQmallRecord(Search<QmallRecordSearch> search, out int rowCount)
        {
            rowCount = 0;
            try
            {
                SearchCondition<QmallRecordSearch> current = SearchCondition<QmallRecordSearch>.Current;
                QmallRecordSearch model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                if (model.status != -1)
                    current.AddNumber(p => p.status, model.status);
                current.Add(p => p.Product_Id, TOpeart.IN, search.SearchObj.Product_Id);
                string cur = current.ToString();
                var res = GetQmallRecordServer<QmallRecord>(RecordServerUrl + "get_qmall_record", cur);
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_qmall_record ex:", ex.Message);
            }
            return null;
        }
        QmallRecord makeQmallRecord(DataRow dr)
        {
            var r = new QmallRecord();
            if (dr["ID"] != null && !string.IsNullOrEmpty(dr["ID"].ToString()))
                r.ID = int.Parse(dr["ID"].ToString());
            r.Account = dr["Account"].ToString();
            r.Charge_Id = dr["Charge_Id"].ToString();
            if (dr["Create_Date"] != null && !string.IsNullOrEmpty(dr["Create_Date"].ToString()))
                r.Create_Date = dr["Create_Date"].ToString();
            r.Order_Num = dr["Order_Num"].ToString();
            r.Product_Name = dr["Product_Name"].ToString();
            if (dr["status"] != null && !string.IsNullOrEmpty(dr["status"].ToString()))
                r.status = long.Parse(dr["status"].ToString());
            if (dr["User_Input"] != null && !string.IsNullOrEmpty(dr["User_Input"].ToString()))
            {
                var jarry = Newtonsoft.Json.JsonConvert.DeserializeObject(dr["User_Input"].ToString()) as Newtonsoft.Json.Linq.JArray;
                if (jarry != null)
                {
                    List<object> list = new List<object>();
                    foreach (Newtonsoft.Json.Linq.JToken item in jarry)
                    {
                        JTokenResolver.Resolve(list, item);
                    }
                    if (list != null && list.Count > 0)
                    {
                        r.User_Input = list[list.Count - 1].ToString();
                    }
                }
            }
            return r;
        }
        public static Result<List<T>> GetQmallRecordServer<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(requestUrl) || string.IsNullOrEmpty(param))
                {
                    Base.WriteError("GetQmallRecordServer requestUrl is empty or param is empty.");
                    return null;
                }
                WriteDebug("param:", param);
                WriteDebug("requestUrl:", requestUrl);
                var request = HttpWebRequest.Create(requestUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = param.Length;
                request.Timeout = 20000;
                try
                {
                    var sw = new StreamWriter(request.GetRequestStream());
                    sw.Write(param);
                    if (sw != null)
                        sw.Close();
                    var response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var res = reader.ReadToEnd();
                        WriteDebug("GetQmallRecordServer res is ",res);
                        res = res.Replace("\\\"", "'");
                        Base.WriteLog("recordserver'res is ", res);
                        try
                        {
                            var jarry = JsonConvert.DeserializeObject(res) as JArray;
                            if (jarry == null)
                            {
                                Base.WriteError("DeserializeRecord JsonConvert.DeserializeObject(res) is null");
                                return default(Result<List<T>>);
                            }
                            List<object> list = new List<object>();
                            foreach (JToken item in jarry)
                            {
                                JTokenResolver.Resolve(list, item);
                            }
                            if (list == null || list.Count < 1)
                            {
                                Base.WriteError("DeserializeRecord JTokenResolver.Resolve'list is null.");
                                return default(Result<List<T>>);
                            }
                            if (list[0] == null || string.IsNullOrEmpty(list[0].ToString()))
                            {
                                Base.WriteError("DeserializeRecord res not has data.");//返回约定格式有误
                                return default(Result<List<T>>);
                            }
                            if (int.Parse(list[0].ToString()) < 1)
                                return default(Result<List<T>>);
                            if (list.Count != 3)
                                return default(Result<List<T>>);
                            Result<List<T>> r = new Result<List<T>>() { Code = int.Parse(list[0].ToString()) };
                            var keys = list[1] as List<object>;
                            var values = list[2] as List<object>;
                            List<object> valuelist = null;
                            var sb = new StringBuilder();
                            sb.Append("[");
                            for (int i = 0; i < values.Count; i++)
                            {
                                valuelist = values[i] as List<object>;
                                sb.Append("{");
                                for (int j = 0; j < valuelist.Count; j++)
                                {
                                    if (valuelist[j] != null && !string.IsNullOrEmpty(valuelist[j].ToString()))
                                        sb.AppendFormat("\"{0}\":\"{1}\",", keys[j], valuelist[j]);
                                }
                                sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                                sb.Append("},");
                            }
                            sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                            sb.Append("]");
                            r.R = JsonConvert.DeserializeObject<List<T>>(sb.ToString());
                            response.Close();
                            return r;
                        }
                        catch (Exception ex)
                        {
                            Base.WriteError("DeserializeRecord convert ex:", ex.Message, ";URL: ", request.RequestUri.PathAndQuery, ";res：", res);
                            response.Close();
                        }



                    }
                }
                catch (Exception ex)
                {
                    Base.WriteError("DeserializeRecord convert ex:", ex.Message, ";URL: ", request.RequestUri.PathAndQuery);
                    return null;
                }
                return null;


            }
            catch (Exception ex2)
            {
                Base.WriteError("PostRecordServer ex:", ex2.Message, "url:", requestUrl + "?" + param);
            }
            return default(Result<List<T>>);
        }



        public List<SystemLog> GetSystemlogRecord(SystemLogSearch search, out int rowCount)
        {
            rowCount = 0;
            DataTable dt = GetSearchData(search, DBName.Manage, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<SystemLog>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                    list.Add(makeSystemLogRecord(dr));
            }
            catch (Exception ex)
            {
                WriteError("RecordDAL.GetSystemlogRecord ex:", ex.Message);
            }
            return list;
        }
        SystemLog makeSystemLogRecord(DataRow row)
        {
            var model = new SystemLog();
            if (row["ID"] != null && row["ID"].ToString() != "")
            {
                model.ID = int.Parse(row["ID"].ToString());
            }
            if (row["Account"] != null)
            {
                model.Account = row["Account"].ToString();
            }
            if (row["OperTime"] != null && row["OperTime"].ToString() != "")
            {
                model.OperTime = DateTime.Parse(row["OperTime"].ToString());
            }
            if (row["IP"] != null)
            {
                model.IP = row["IP"].ToString();
            }
            if (row["Operation"] != null)
            {
                model.Operation = row["Operation"].ToString();
            }
            if (row["Page"] != null)
            {
                model.Page = row["Page"].ToString();
            }
            if (row["Remark"] != null)
            {
                model.Remark = row["Remark"].ToString();
            }
            if (row["OprState"] != null && row["OprState"].ToString() != "")
            {
                model.OprState = int.Parse(row["OprState"].ToString());
            }
            if (row["type"] != null && row["type"].ToString() != "")
            {
                model.Type = int.Parse(row["type"].ToString());
            }
            return model;
        }
        public List<LoginLog> GetLoginLogRecord(LoginLogSearch search, out int rowCount)
        {
            rowCount = 0;
            DataTable dt = GetSearchData(search, DBName.Manage, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<LoginLog>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                    list.Add(makeLoginLog(dr));
            }
            catch (Exception ex)
            {
                WriteError("RecordDAL.GetLoginLogRecord ex:", ex.Message);
            }
            return list;
        }
        LoginLog makeLoginLog(DataRow row)
        {
            var model = new LoginLog();
            if (row["ID"] != null && row["ID"].ToString() != "")
            {
                model.ID = int.Parse(row["ID"].ToString());
            }
            if (row["Account"] != null)
            {
                model.Account = row["Account"].ToString();
            }
            if (row["LoginTime"] != null && row["LoginTime"].ToString() != "")
            {
                model.LoginTime = DateTime.Parse(row["LoginTime"].ToString());
            }
            if (row["IP"] != null)
            {
                model.IP = row["IP"].ToString();
            }
            if (row["LoginState"] != null && row["LoginState"].ToString() != "")
            {
                model.LoginState = int.Parse(row["LoginState"].ToString());
            }
            if (row["Message"] != null)
            {
                model.Message = row["Message"].ToString();
            }
            return model;
        }
    }
}
