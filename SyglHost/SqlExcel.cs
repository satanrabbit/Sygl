using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace SyglHost
{
    class SqlExcel
    {  
        /// <summary>
        /// 无返回值查询
        /// </summary>
        /// <param name="filePath">指定查询的excel表</param>
        /// <param name="sql">查询语句</param>
        public static void ExcuteNonQuery(string filePath, string sql)
        {
            using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath)))
            {
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        /// <summary>
        /// 执行无返回值的系列查询
        /// </summary>
        /// <param name="filePath">指定查询的excel表</param>
        /// <param name="sqlList">查询语句组</param>
        public static void BatchExcuteNonQuery(string filePath, IList<string> sqlList)
        {
            using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath)))
            {
                OleDbCommand cmd = conn.CreateCommand();
                conn.Open();
                foreach (var sql in sqlList)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 获取查询指定excel表的连接字符串
        /// </summary>
        /// <param name="filePath">指定的excel表</param>
        /// <returns></returns>
        public static string GetConnectionString(string filePath)
        {
            if (filePath.EndsWith(".xls"))
                return string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};"
                    + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=2';", filePath);
            if (filePath.EndsWith(".xlsx"))
                return string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};"
                    + "Extended Properties='Excel 12.0 Xml;HDR=YES';", filePath);
            throw new Exception("wrong file type!");
        }
        /// <summary>
        /// 执行excel表查询
        /// </summary>
        /// <param name="filePath">指定的excel表</param>
        /// <param name="sql">查询语句</param>
        /// <returns>查询结果表</returns>
        public static DataTable ExcuteSelect(string filePath, string sql)
        {
            using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath)))
            {
                OleDbDataAdapter ad = new OleDbDataAdapter(sql, conn);
                DataTable table = new DataTable();
                conn.Open();
                ad.Fill(table);
                return table;
            }
        }
        /// <summary>
        /// 获取excel的表的信息表
        /// </summary>
        /// <param name="filePath">导入excel文件</param>
        /// <returns>excel表的信息表</returns>
        public static DataTable GetExcelTableName(string filePath)
        {
            using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath)))
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                conn.Close();
                return dt;

            }
        }
    }

     
}
