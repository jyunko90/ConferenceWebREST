using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ConferenceRESTSystem
{
    public class DBConnect
    {
        private static SqlConnection sqlConnection;
        private static string conStr = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

        public static SqlConnection getConnection()
        {
            sqlConnection = new SqlConnection(conStr);
            return sqlConnection;
        }

        public DBConnect()
        {

        }
    }
}