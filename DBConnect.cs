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
        //private static string conStr = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        private static string conStr = ConfigurationManager.ConnectionStrings["MyDBhost"].ConnectionString;
        private static string MyDBconStr = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        private static string MyDBhostconStr = ConfigurationManager.ConnectionStrings["MyDBhost"].ConnectionString;

        public static SqlConnection getConnection()
        {
            System.Diagnostics.Debug.WriteLine(MyDBconStr);
            System.Diagnostics.Debug.WriteLine(MyDBhostconStr);
            sqlConnection = new SqlConnection(conStr);
            return sqlConnection;
        }

        public DBConnect()
        {

        }
    }
}