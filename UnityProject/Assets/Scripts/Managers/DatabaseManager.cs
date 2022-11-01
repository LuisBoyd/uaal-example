using System;
using MySql.Data.MySqlClient;

namespace RCR.Managers
{
    public static class DatabaseManager
    {
        private const string m_databaseConnectionInfo =
            "server=82.165.169.82:8443;uid=admin_wnavadmin;pwd=Zk2~2wx4;database=WN_Game";

        public static void TestConnection()
        {
            using (var sqlConnection = new MySqlConnection())
            {
                
            }
        }

    }
}