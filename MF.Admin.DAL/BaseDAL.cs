using System.Data;
using MF.Data;
using System.Data.SqlClient;
using System;
using MF.Common.Json;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Net;
using System.Text;
using MF.Common.Security;
using Newtonsoft.Json;
using System.Diagnostics;
using MF.Protocol;
using System.Data.OleDb;

namespace MF.Admin.DAL
{
    public class DBConst
    {
        public int dbname { get; set; }
        public string connectionstring { get; set; }
    }
    public class BaseDAL
    {
        protected static string ChargeServerUrl = ConfigurationManager.AppSettings["ChargeURI"];
        protected static string GuildUrl = ConfigurationManager.AppSettings["GuildURI"];
        protected static string RecordServerUrl = ConfigurationManager.AppSettings["RecordServerURI"];
        protected static string ModifyUserServerUrl = ConfigurationManager.AppSettings["ModifyUserURI"];
        public static string WebKey = ConfigurationManager.AppSettings["WEB_SECURITY"];
        public static string WBCUrl = ConfigurationManager.AppSettings["WBCURI"];
        public static string PUSHURI = ConfigurationManager.AppSettings["PUSHURI"];
        public static string ClubsURI = ConfigurationManager.AppSettings["ClubsURI"];
        public static string BlackURI = ConfigurationManager.AppSettings["BlackURI"];
        public static string GameCoinURI = ConfigurationManager.AppSettings["GameCoinURI"];
        public static string GameIncome = ConfigurationManager.AppSettings["GameIncome"];
        public static string RecURI = ConfigurationManager.AppSettings["RecURI"];
        public static string ShiChuiURI = ConfigurationManager.AppSettings["ShiChuiURI"];
        

        public BaseDAL()
        {
            LoadDBConfig();
        }

        public static bool IsDebug
        {
            get { return MF.Protocol.Base.IsDebug; }
        }
        #region 数据库连接
        public static DBName dbname = DBName.MF_DY;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string CurrencyConnectionStr
        {
            get
            {
                foreach (DBConst dbconst in dbconfig)
                {
                    //if (dbname == (DBName)dbconst.dbname)
                    if ((int)dbname == dbconst.dbname)
                        return dbconst.connectionstring;
                }
                return "";
            }
        }
        private static List<DBConst> _dbconfig;
        public static List<DBConst> dbconfig
        {
            get
            {
                if (_dbconfig == null)
                    LoadDBConfig();
                return _dbconfig;
            }
        }
        private static void LoadDBConfig()
        {
            try
            {
                string text = LoadFile("db.config");
                text = AES.Decrypt(text);
                _dbconfig = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DBConst>>(text);
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("加载 db.config 文件异常：", ex.Message);
            }
        }
        public static string LoadFile(string filename)
        {
            try
            {
                string filepath = ConfigurationManager.AppSettings["configpath"];
                filename = filepath + filename;
                if (!File.Exists(filename))
                {
                    BaseDAL.WriteError(filename + "文件不存在");
                    return "";
                }
                var fs = File.Open(filename, FileMode.Open);
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                var BOM = new byte[] { 0xEF, 0xBB, 0xBF };
                var index = 0;
                if (bytes.Length > BOM.Length)
                {
                    if (bytes[0] == BOM[0] && bytes[1] == BOM[1] && bytes[2] == BOM[2])
                        index = 3;
                }
                var text = System.Text.Encoding.UTF8.GetString(bytes, index, bytes.Length - index);
                fs.Close();
                return text;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("加载 db.config 文件异常：", ex.Message);
            }
            return "";
        }
        #endregion

        #region 执行数据库
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="search">search.DBName可不传值</param>
        ///<param name="db">db为数据库名称 必传参</param>
        public static DataTable GetSearchData(Search search, DBName db, out int rowCount)
        {
            rowCount = 0;
            try
            {
                BaseDAL.dbname = db;
                var args = new SqlParameter[] {
                    new SqlParameter("@pageSize",search.PageSize),
                    new SqlParameter("@pageIndex",search.PageIndex),
                    new SqlParameter("@fields",search.Fields),
                    new SqlParameter("@pk",search.PrimaryKey),
                    new SqlParameter("@table",search.Table),
                    new SqlParameter("@where",search.Where),
                    new SqlParameter("@orderby",search.OrderBy),
                    new SqlParameter("@rowCount",SqlDbType.Int)
                };
                args[7].Direction = ParameterDirection.Output;
                string searchinfo = string.Format("table:{0},fields:{1},where:{2}", search.Table, search.Fields, search.Where);
                WriteDebug("searchinfo:", searchinfo);
                DataTable dt = DataHelper.GetDataTable("mf_P_Pager", args);
                if (args[7].Value != null && !string.IsNullOrEmpty(args[7].Value.ToString()))
                    rowCount = (int)args[7].Value;
                return dt;
            }
            catch (Exception e)
            {
                WriteError("执行分页获取数据存储过程异常:", e.Message, "!args:", Json.SerializeObject(search));
            }
            return null;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedureName">存储过程名称</param>
        /// <param name="sqlParams">存储过程所需参数</param>
        /// <param name="db">db为数据库名称 必传参</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strProcedureName, SqlParameter[] sqlParams, DBName db)
        {
            BaseDAL.dbname = db;
            return DataHelper.GetDataTable(strProcedureName, sqlParams);
        }
        /// <summary>
        /// 执行sql语句 db为必传参
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="db">db为数据库名称 必传参</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, DBName db)
        {
            BaseDAL.dbname = db;
            return DataHelper.GetDataTable(sql);
        }

        #endregion

        #region 日志
        public static void WriteError(params object[] err)
        {
            Base.WriteError(err);
        }
        public static void WriteLog(params string[] log)
        {
            Base.WriteLog(log);
        }
        public static void WriteDebug(params string[] log)
        {
            Base.WriteDebug(log);
        }
        #endregion

        #region 请求服务器
        public static Result<List<T>> PostRecordServer<T>(string requestUrl, string param)
        {
            PostResult<List<T>> res = Http.PostRecordServer<T>(requestUrl, param);
            if (res == null)
                return null;
            return new Result<List<T>>() { Code = res.Code, Ex = res.Ex, Message = res.Message, R = res.R };
        }
        public static T PostClubServer<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(requestUrl))
                {
                    WriteError("requestUrl is empty.");
                    return default(T);
                }
                WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var res = reader.ReadToEnd();
                    WriteDebug("sever'res is ", res);
                    try
                    {
                        var t = JsonConvert.DeserializeObject<T>(res);
                        if (t == null)
                            WriteError("Deserialize<T> t is null.");
                        response.Close();
                        return t;
                    }
                    catch (Exception ex2)
                    {
                        WriteError("PostServer Deserialize Convert ex：", ex2.Message, ";URL: ", request.RequestUri.PathAndQuery, " requestUrl is ", requestUrl, ".param is ", param, ";res：", res);
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("PostServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        public static T PostServer<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(requestUrl))
                {
                    WriteError("requestUrl is empty2.");
                    return default(T);
                }
                WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var res = reader.ReadToEnd();
                    WriteDebug("sever'res is ", res);
                    try
                    {
                        var t = JsonConvert.DeserializeObject<T>(res);
                        if (t == null)
                            WriteError("Deserialize<T> t is null.");
                        response.Close();
                        return t;
                    }
                    catch (Exception ex2)
                    {
                        WriteError("PostServer Deserialize Convert ex：", ex2.Message, ";URL: ", request.RequestUri.PathAndQuery, " requestUrl is ", requestUrl, ".param is ", param, ";res：", res);
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("PostServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        public static string GetServer(string requestUrl, string param)
        {
            try {
                if (string.IsNullOrEmpty(requestUrl))
                {
                    WriteError("GetServer requestUrl is empty.");
                    return "";
                }
                WriteDebug("GetServer requestUrl is ", requestUrl, ".param is ", param);

                if (!string.IsNullOrEmpty(param))
                    requestUrl += "?" + param;
                System.Net.WebRequest wrq = System.Net.WebRequest.Create(requestUrl);
                wrq.Method = "GET";
                System.Net.WebResponse wrp = wrq.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(wrp.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                var res = sr.ReadToEnd();
                WriteDebug("GetServer res is ", res);
                return res;
            }
            catch(Exception ex) {

                WriteError("GetServer str ex:",ex.Message);
            }
            return "";
        }
        public static T GetServer<T>(string requestUrl, string param)
        {
            try
            {
                var res=GetServer(requestUrl, param);
                if (res == "")
                {
                    WriteError("GetServer str res is empty.");
                    return default(T);
                }
                try
                {
                    var t = JsonConvert.DeserializeObject<T>(res);
                    if (t == null)
                        WriteError("GetServer Deserialize<T> t is null.");
                    return t;
                }
                catch (Exception ex2)
                {
                    WriteError("GetServer Deserialize Convert ex：", ex2.Message, ";URL: ", requestUrl, ";res：", res);
                }
            }
            catch (Exception ex)
            {
                WriteError("GetServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        #endregion

        #region 读取excel
        public static DataSet ReadExcel(string path)
        {
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;'";//.xls版本
                DataSet ds = new DataSet();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
                oada.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                Base.WriteError("ReadExcel ex:", ex.Message, ",path:", path);
            }
            return null;
        }
        public static Result<List<T>> ReadExcel<T>(string path)
        {
            Result<List<T>> res = new Result<List<T>>();
            try
            {
                DataSet ds = ReadExcel(path);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    res.Code = -2;
                    res.Message = "文件数据为空或读取数据异常.path: " + path;
                    return res;
                }
                List<string> keys = new List<string>();
                var sb = new StringBuilder();
                sb.Append("[");
                string colname = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("{");
                    for (int col = 0; col < ds.Tables[0].Columns.Count; col++)
                    {
                        colname = ds.Tables[0].Columns[col].ColumnName;
                        sb.AppendFormat("\"{0}\":\"{1}\",", colname, row[colname]);
                    }
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                    sb.Append("},");
                }
                sb = new StringBuilder(sb.ToString().Substring(0, sb.Length - 1));
                sb.Append("]");
                res.R = JsonConvert.DeserializeObject<List<T>>(sb.ToString());
                res.Code = 1;
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = "ex:" + ex.Message;
                Base.WriteError("ReadExcel<T> ex:", ex.Message, ",path:", path);
            }
            return res;
        }
        #endregion

        #region 关于时间
        /// <summary>
        /// 以2012-10-01开启的基准时间
        /// </summary>
        public static DateTime BaseTime
        {
            get
            {
                return DateTime.Parse("2012-10-01 0:00:00");
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="datepart">时间类型s m h d M y</param>
        /// <param name="timeSpan">时间刻度数</param>
        /// <returns>返回【在2012-10-01基准时间上添加刻度】后的时间</returns>
        public static DateTime ConvertSpanToDate(string datepart, int timeSpan)
        {
            if (datepart.ToLower() == "s" || datepart.ToLower() == "ss")
            {
                return BaseTime.AddSeconds((double)timeSpan);
            }
            else if (datepart.ToLower() == "m" || datepart.ToLower() == "mm")
            {
                return BaseTime.AddMinutes((double)timeSpan);
            }
            else if (datepart.ToLower() == "h" || datepart.ToLower() == "hh")
            {
                return BaseTime.AddHours((double)timeSpan);
            }
            else if (datepart.ToLower() == "d" || datepart.ToLower() == "dd")
            {
                return BaseTime.AddDays((double)timeSpan);
            }
            else if (datepart.ToLower() == "M" || datepart.ToLower() == "MM")
            {
                return BaseTime.AddMonths(timeSpan);
            }
            else if (datepart.ToLower() == "y" || datepart.ToLower() == "yy")
            {
                return BaseTime.AddYears(timeSpan);
            }
            return BaseTime;
        }
        /// <summary>
        /// 时间比较
        /// </summary>
        /// <param name="time">需要比较的时间</param>
        /// <param name="datepart">时间类型s m h d M y</param>
        /// <returns>返回【与2012-10-01基准时间比较后的刻度数量】
        /// 大于0表示该时间在基准时间之后
        /// 小于0表示该时间在基准时间之前
        /// </returns>
        public static int ConvertDateToSpan(DateTime time, string datepart)
        {
            if (datepart.ToLower() == "s" || datepart.ToLower() == "ss")
            {
                return (int)(time - BaseTime).TotalSeconds;
            }
            else if (datepart.ToLower() == "m" || datepart.ToLower() == "mm")
            {
                return (int)(time - BaseTime).TotalMinutes;
            }
            else if (datepart.ToLower() == "h" || datepart.ToLower() == "hh")
            {
                return (int)(time - BaseTime).TotalHours;
            }
            else if (datepart.ToLower() == "d" || datepart.ToLower() == "dd")
            {
                return (int)(time - BaseTime).TotalDays;
            }
            return 0;
        }
        #endregion


        #region POST FORM表单提交方式
        static T requestServerUrl<T>(string requestUrl, string param)
        {
            try
            {
                Base.WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.ContentLength = buffer.Length;
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                    var response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var res = reader.ReadToEnd();
                        Base.WriteDebug("sever'res is ", res);
                        try
                        {
                            var t = JsonConvert.DeserializeObject<T>(res);
                            if (t == null)
                                Base.WriteError("Deserialize<T> t is null.");
                            response.Close();
                            return t;
                        }
                        catch (Exception ex2)
                        {
                            Base.WriteError("PostServer Deserialize Convert ex：", ex2.Message, ";URL: ", request.RequestUri.PathAndQuery, " requestUrl is ", requestUrl, ".param is ", param, ";res：", res);
                            response.Close();
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("PostServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }
        public static T Post<T>(string requestUrl, string param)
        {
            try
            {
                if (string.IsNullOrEmpty(param))
                {
                    Base.WriteError("Post<T> param is empty.");
                    return default(T);
                }
                return requestServerUrl<T>(requestUrl, param);
            }
            catch (Exception ex2)
            {
                Base.WriteError("Post ex：", ex2.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return default(T);
        }


        static string requestServerUrl(string requestUrl, string param)
        {
            try
            {
                Base.WriteDebug("requestUrl is ", requestUrl, ".param is ", param);
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                var buffer = Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.ContentLength = buffer.Length;
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                    var response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var res = reader.ReadToEnd();
                        Base.WriteDebug("sever'res is ", res);
                        response.Close();
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("PostServer requestUrl ex:", ex.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return "";
        }
        public static string Post(string requestUrl, string param)
        {
            try
            {
                //if (string.IsNullOrEmpty(param))
                //{
                //    Base.WriteError("Post<T> param is empty.");
                //    return "";
                //}
                return requestServerUrl(requestUrl, param);
            }
            catch (Exception ex2)
            {
                Base.WriteError("Post ex：", ex2.Message, ",requestUrl:", requestUrl, ",param:", param);
            }
            return "";
        }
        #endregion

    }
}
