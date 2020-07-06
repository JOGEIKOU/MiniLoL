using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Maticsoft.DBUtility
{
    public abstract class DbHelperMySQL
    {
        public static string connectionString = "Server = 119.29.9.189;Database = LOL;Uid = Ace;Pwd = 123456;";

        public DbHelperMySQL()
        {
        }

        #region 公的メソッド
        /// <summary>
        /// 最大値を取得
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static int GetMaxID(string FieldName,string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if(obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// 存在か
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdres;
            if((Object.Equals(obj,null)) || (Object.Equals(obj,System.DBNull.Value)))
            {
                cmdres = 0;
            }
            else
            {
                cmdres = int.Parse(obj.ToString());
            }
            if(cmdres == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 存在か（base of MySqlParameter）
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql,params MySqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdres;
            if((Object.Equals(obj,null)) || (Object.Equals(obj,System.DBNull.Value)))
            {
                cmdres = 0;
            }
            else
            {
                cmdres = int.Parse(obj.ToString());
            }
            if(cmdres == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region 簡単なSQLメソッド


        /// <summary>
        /// メソッド一つ実行すると、Object結果を戻す
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns>検査結果(object)</returns>
        public static object GetSingle(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if((Object.Equals(obj,null)) || (Object.Equals(obj,System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch(MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static object GetSingle(string SQLString,int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        object obj = cmd.ExecuteScalar();
                        if((Object.Equals(obj,null)) || (Object.Equals(obj,System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch(MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        #endregion

        #region 引数付きのSQLメソッド

        /// <summary>
        /// 検索メソッド一つ実行すると、Object結果を戻す
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object GetSingle(string SQLString , params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        if((Object.Equals(obj , null)) || (Object.Equals(obj,System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch(MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn , MySqlTransaction trans , string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null) cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;                //cmdType
            if(cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion


    }
}
