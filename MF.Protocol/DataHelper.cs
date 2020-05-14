using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MF.Common.Security;

namespace MF.Protocol
{
    public sealed class DataHelper
    {
        //private static string _mConnectionString;
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static string mConnectionString { get; set; }
        //{
        //    get
        //    {
        //        if(string.IsNullOrEmpty(_mConnectionString))
        //            _mConnectionString = ConfigurationSettings.AppSettings["webdb"];
        //        return AES.Decrypt(_mConnectionString);  
        //    }
        //}
        #region 构造函数
        /// <summary>
        /// DatHelper构造函数
        /// </summary>
        public DataHelper()
        {

        }
        #endregion

        #region 命令执行类


        /// <summary>
        /// 执行指定SQL语句。（自动获取默认连接串）
        /// </summary>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <returns>返回所受影响的数据行数。</returns>
        public static int ExecuteCommand(string strCommandString)
        {
            return SQLHelper.ExecuteNonQuery(mConnectionString, CommandType.Text, strCommandString);
        }
        /// <summary>
        /// 执行指定SQL语句。（指定连接串）
        /// </summary>
        /// <param name="strConnectionString">数据库连接串。</param>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <returns>返回所受影响的数据行数。</returns>
        public static int ExecuteCommand(string strConnectionString, string strCommandString)
        {
            return SQLHelper.ExecuteNonQuery(strConnectionString, CommandType.Text, strCommandString);
        }
        #endregion

        #region 执行存储过程

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="returnValue"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int ExecuteProcedure(string strProcedureName, SqlParameter[] parameters, out int returnValue, int n)
        {
            returnValue = SQLHelper.ExecuteNonQueryReturn(mConnectionString, CommandType.StoredProcedure, strProcedureName, parameters);
            return returnValue;
        }

        public static int ExecuteProcedure(string strProcedureName, SqlParameter[] parameters)
        {
            return SQLHelper.ExecuteNonQuery(mConnectionString, CommandType.StoredProcedure, strProcedureName, parameters);
        }
        /// <summary>
        /// 执行指定存储过程。（自动获取默认连接串）
        /// </summary>
        /// <param name="strProcedureName">存储过程名称。</param>
        /// <param name="parameters">参数表。</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns>返回所受影响的数据行数。</returns>
        public static int ExecuteProcedure(string strProcedureName, SqlParameter[] parameters, out int rowsAffected)
        {
            rowsAffected = SQLHelper.ExecuteNonQuery(mConnectionString, CommandType.StoredProcedure, strProcedureName, parameters);
            return rowsAffected;
        }


        /// <summary>
        /// 执行指定存储过程。（指定连接串）
        /// </summary>
        /// <param name="strConnectionString">数据库连接串。</param>
        /// <param name="strProcedureName">存储过程名称。</param>
        /// <param name="parameters">参数表。</param>
        /// <returns>返回所受影响的数据行数。</returns>
        public static int ExecuteProcedure(string strConnectionString, string strProcedureName, SqlParameter[] parameters)
        {
            return SQLHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, strProcedureName, parameters);
        }

        #endregion

        #region 获取数据集

        /// <summary>
        /// 根据指定SQL语句返回数据集。
        /// </summary>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSet(string strCommandString)
        {
            string[] strTableNames = "1,2,3,4,5,6,7,8,9,10".Split(',');
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();

            connection.ConnectionString = mConnectionString;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.Text, strCommandString, dstDataSet, strTableNames);
            }
            catch
            { ;}
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return dstDataSet;
        }

        /// <summary>
        /// 根据指定SQL语句返回数据集。
        /// </summary>
        /// <param name="strConnectionString">数据库连接串。</param>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSet(string strConnectionString, string strCommandString)
        {
            string[] strTableNames = "1,2,3,4,5,6,7,8,9,10".Split(',');
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();

            connection.ConnectionString = strConnectionString;
            try
            {

                SQLHelper.FillDataset(connection, CommandType.Text, strCommandString, dstDataSet, strTableNames);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dstDataSet;
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        public static DataSet GetDataSet(DataSet ds, string strProcedureName, SqlParameter[] p, string[] strTableNames)
        {
            DataSet dsNew = ds;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, strProcedureName, dsNew, strTableNames, p);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dsNew;
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 根据指定SQL语句返回数据集。
        /// </summary>
        /// <param name="strConnectionString">数据库连接串。</param>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <param name="strTableNames">返回各个表的表名。</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSet(string strConnectionString, string strCommandString, string[] strTableNames)
        {
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();
            try
            {


                connection.ConnectionString = strConnectionString;
                SQLHelper.FillDataset(connection, CommandType.Text, strCommandString, dstDataSet, strTableNames);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch
            { }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return dstDataSet;
        }

        /// <summary>
        /// 根据指定存储过程返回数据集。
        /// </summary>
        /// <param name="strProcedureName">要执行的存储过程。</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSetByProcedure(string strProcedureName)
        {
            string[] strTableNames = "1,2,3,4,5,6,7,8,9,10".Split(',');
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, strProcedureName, dstDataSet, strTableNames, null);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dstDataSet;
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 根据指定存储过程返回数据集。
        /// </summary>
        /// <param name="strProcedureName">要执行的存储过程。</param>
        /// <param name="sqlParams">参数列表</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSetByProcedure(string strProcedureName, SqlParameter[] sqlParams)
        {
            string[] strTableNames = "1,2,3,4,5,6,7,8,9,10".Split(',');
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, strProcedureName, dstDataSet, strTableNames, sqlParams);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dstDataSet;
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 根据指定存储过程返回数据集。
        /// </summary>
        /// <param name="strProcedureName">要执行的存储过程。</param>
        /// <param name="sqlParams">参数列表</param>
        /// <param name="strTableNames">各个表的表名。</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSetByProcedure(string strProcedureName, SqlParameter[] sqlParams, string[] strTableNames)
        {
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, strProcedureName, dstDataSet, strTableNames, sqlParams);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dstDataSet;
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }


        /// <summary>
        /// 根据指定存储过程返回数据集。(返回单个表的数据集 ,项目特定)
        /// </summary>
        /// <param name="strProcedureName">要执行的存储过程。</param>
        /// <param name="sqlParams">参数列表</param>
        /// <param name="strTableNames">各个表的表名。</param>
        /// <returns>数据集DataSet。</returns>
        public static DataSet GetDataSetByProcedure(string strProcedureName, SqlParameter[] sqlParams, string strTableNames1)
        {
            string[] strTableNames = "1,2,3,4,5,6,7,8,9,10".Split(',');
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, strProcedureName, dstDataSet, strTableNames, sqlParams);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dstDataSet;
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }
        #endregion

        #region 获取数据表

        /// <summary>
        /// 根据SQL命令创建数据表实体。
        /// </summary>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        public static DataTable GetDataTable(string strCommandString)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            DataSet dstDataSet = new DataSet();
            try
            {
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(strCommandString, connection);
                sqlAdapter.Fill(dstDataSet);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return dstDataSet.Tables[0];
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return null;
        }

        /// <summary>
        /// 根据SQL命令创建数据表实体。
        /// </summary>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <param name="strConnectionString">数据库连接串。</param>
        public static DataTable GetDataTable(string strCommandString, string strConnectionString)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = strConnectionString;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strCommandString, connection);
            DataSet dstDataSet = new DataSet();
            try
            {
                sqlAdapter.Fill(dstDataSet);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return dstDataSet.Tables[0];
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 根据SQL命令创建数据表实体。
        /// </summary>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <param name="strConnectionString">数据库连接串。</param>
        /// <param name="strTableName">数据表名称。</param>
        public static DataTable GetDataTable(string strCommandString, string strConnectionString, string strTableName)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = strConnectionString;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strCommandString, connection);
            DataSet dstDataSet = new DataSet();
            try
            {
                sqlAdapter.Fill(dstDataSet);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return dstDataSet.Tables[0];
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 根据指定存储过程返回数据表。
        /// </summary>
        /// <param name="strProcedureName">要执行的存储过程。</param>
        /// <param name="sqlParams">参数列表</param>
        /// <returns>数据集DataSet。</returns>
        public static DataTable GetDataTable(string strProcedureName, SqlParameter[] sqlParams)
        {
            string[] strTableNames = "1,2,3,4,5,6,7,8,9,10".Split(',');
            DataSet dstDataSet = new DataSet();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            try
            {

                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, strProcedureName, dstDataSet, strTableNames, sqlParams);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                return dstDataSet.Tables[0];
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return null;
        }

        #endregion

        #region 获取字段值

        /// <summary>
        /// 获取某个数据表第一行中指定字段的值
        /// </summary>
        /// <param name="strCommandString">要执行的SQL语句。</param>
        /// <param name="fieldName">要返回的字段名。</param>
        /// <returns>String:字段值。</returns>
        public static string GetFieldString(string strCommandString, string fieldName)
        {
            DataRow dr = GetDataRow(strCommandString);
            if (dr != null)
                return dr[fieldName].ToString();
            return "";
        }
        /// <summary>
        /// 获取某个数据表第一行第一列字段的值
        /// </summary>
        /// <param name="fieldNmae">要获取值的字段名</param>
        /// <param name="tableName">表名</param>
        /// <param name="fieldValue">条件值</param>
        /// <param name="conditionField">条件字段</param>
        /// <returns>String:字段值。</returns>
        public static string GetFieldString(string fieldName, string tableName, string fieldValue, string conditionField)
        {
            string commandText = "select top 1 " + fieldName + " from " + tableName + " where " + conditionField + "='" + fieldValue + "'";
            DataRow dr = GetDataRow(commandText);
            if (dr != null)
                return dr[fieldName].ToString();
            return "";
        }




        /// <summary>
        /// 获取指定表中某行数据。
        /// </summary>
        /// <param name="strTableName">数据表名。</param>
        /// <param name="strFieldName">标识字段名。（保证取值唯一）</param>
        /// <param name="strFieldValue">标识字段值。</param>
        /// <returns>返回数据行对象。[DataRow]</returns>
        public static DataRow GetDataRow(string strTableName, string strFieldName, string strFieldValue)
        {
            string strSQL = "SELECT TOP 1 * FROM @strTableName WHERE @strFieldName ='@strFieldValue'";
            strSQL = strSQL.Replace("@strTableName", strTableName);
            strSQL = strSQL.Replace("@strFieldName", strFieldName);
            strSQL = strSQL.Replace("@strFieldValue", strFieldValue);

            DataTable dt = GetDataTable(strSQL);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
            }
            return null;
        }
        /// <summary>
        /// 根据SQL语句获取某行数据
        /// </summary>
        /// <param name="strCommandText">SQL语句</param>
        /// <returns>返回数据行对象。[DataRow]</returns>
        public static DataRow GetDataRow(string strCommandText)
        {
            DataTable dt = GetDataTable(strCommandText);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    return dt.Rows[0];
                }
            }
            return null;
        }

        #endregion

        #region 执行事务
        /// <summary>
        /// 将多个SQL命令放在一个事务中执行
        /// </summary>
        /// <param name="commandText">SQL命令数组</param>
        /// <returns>成功返回1,失败返回-1</returns>
        public static int ExecuteTran(string[] commandText)
        {
            int result = -1;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = mConnectionString;
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction();
            try
            {
                for (int i = 0; i < commandText.Length; i++)
                    SQLHelper.ExecuteNonQuery(tran, CommandType.Text, commandText[i], null);
                tran.Commit();
                result = 1;
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 将多个SQL命令放在一个事务中执行
        /// </summary>
        /// <param name="tran">要处理的T-SQL事务</param>
        /// <param name="commandText">SQL命令数组</param>
        /// <returns>成功返回1,失败返回-1</returns>
        public static int ExecuteTran(SqlTransaction tran, string[] commandText)
        {
            try
            {
                for (int i = 0; i < commandText.Length; i++)
                    SQLHelper.ExecuteNonQuery(tran, CommandType.Text, commandText[i]);
                tran.Commit();
                return 1;
            }
            catch
            {
                tran.Rollback();
            }
            return -1;
        }
        #endregion

        #region 判断数据是否存在
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">要校验的值</param>
        /// <returns></returns>
        public static bool IsExist(string table, string field, string value)
        {
            var sql = string.Format("SELECT TOP 1 {0} FROM {1} WHERE {0}='{3}'",  field, table, value);  
           
            return IsExist(sql);
        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="command">SQLCommand</param>
        /// <returns></returns>
        public static bool IsExist(string command)
        {
            DataRow result = GetDataRow(command);
            if (result == null || result.Table.Rows.Count == 0)
                return false;
            else if (result[0].ToString() == "0" || result[0].ToString() == "")
                return false;
            else
                return true;
        }
        #endregion


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            SqlConnection connection = new SqlConnection(mConnectionString);
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static object GetSingle(string SQLString, int Times)
        {
            SqlConnection connection = new SqlConnection(mConnectionString);
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    cmd.CommandTimeout = Times;
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {

                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
