using MF.Data;
using MF.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace MF.Protocol
{

    public class ExcelData
    {
        #region 读取excel
        private static DataSet ReadExcel(string path)
        {
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";//.xls版本
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
        /// <summary>
        /// 读取excel内容
        /// </summary>
        /// <typeparam name="T">组装返回对象
        /// excel第1行为属性字段名
        /// </typeparam>
        /// <param name="path">excel路径</param>
        /// <returns></returns>
        public static Result<List<T>> ReadExcel<T>(string path)
        {
            Result<List<T>> res = new Result<List<T>>();
            try
            {
                DataSet ds = ReadExcel(path);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    res.Code = Code.Fail;
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
                res.Code = Code.OK;
            }
            catch (Exception ex)
            {
                res.Code = Code.Fail;
                res.Message = "ex:" + ex.Message;
                Base.WriteError("ReadExcel<T> ex:", ex.Message, ",path:", path);
            }
            return res;
        }
        #endregion
    }
}
