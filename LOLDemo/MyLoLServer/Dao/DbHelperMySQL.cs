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

        #endregion

        #region 引数付きのSQLメソッド

        #endregion


    }
}
