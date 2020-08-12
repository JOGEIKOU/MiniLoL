using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;


namespace MyLoLServer.Dao.Model
{
    public class AccountModel
    {
        public int id;
        public string account;
        public string password;
    }
}
