using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ConferenceRESTSystem
{
    public class ServiceAPI : IServiceAPI
    {
        SqlConnection dbConnection;

        public ServiceAPI()
        {
            dbConnection = DBConnect.getConnection();
        }

        public DataTable getSignupOption()
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            DataTable table = new DataTable();
            table.Columns.Add("Gender", typeof(DataTable));
            table.Columns.Add("Title", typeof(DataTable));
            table.Columns.Add("Country", typeof(DataTable));

            String query = "SELECT * FROM [Gender];";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable gender = new DataTable();
            if (reader.HasRows)
            {
                gender.Columns.Add("GenderId", typeof(String));
                gender.Columns.Add("Name", typeof(String));

                while (reader.Read())
                {
                    gender.Rows.Add(
                        reader["GenderId"],
                        reader["Name"]
                    );
                }
            }
            reader.Close();

            query = "SELECT * FROM [Title];";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            DataTable title = new DataTable();
            if (reader.HasRows)
            {
                title.Columns.Add("TitleId", typeof(String));
                title.Columns.Add("Name", typeof(String));

                while (reader.Read())
                {
                    title.Rows.Add(
                        reader["TitleId"],
                        reader["Name"]
                    );
                }
            }
            reader.Close();

            query = "SELECT * FROM [Country];";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            DataTable country = new DataTable();
            if (reader.HasRows)
            {
                country.Columns.Add("CountryId", typeof(String));
                country.Columns.Add("Name", typeof(String));

                while (reader.Read())
                {
                    country.Rows.Add(
                        reader["CountryId"],
                        reader["Name"]
                    );
                }
            }
            reader.Close();

            table.Rows.Add(
                gender,
                title,
                country
            );

            dbConnection.Close();

            return table;
        }

        public bool signup(String username, String password, String email)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "INSERT INTO [User] (Username, encryptedPassword, Email) VALUES ('" + username + "','" + password + "','" + email + "');";

            SqlCommand command = new SqlCommand(query, dbConnection);
            int result = command.ExecuteNonQuery();

            dbConnection.Close();

            return result > 0;
        }

        public long login(String username, String password)
        {
            long userId = -1;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }
            
            String query = "SELECT * FROM [User] WHERE Username='" + username + "' AND encryptedPassword='" + password + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    userId = Convert.ToInt64(reader["UserId"]);
                }
            }

            reader.Close();
            dbConnection.Close();

            return userId;
        }

        public DataTable getUserDetail(String userId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Email", typeof(String));
            table.Columns.Add("Username", typeof(String));
            table.Columns.Add("FullName", typeof(String));
            table.Columns.Add("Instituition", typeof(String));
            table.Columns.Add("Faculty", typeof(String));
            table.Columns.Add("Department", typeof(String));
            table.Columns.Add("ResearchField", typeof(String));
            table.Columns.Add("Address", typeof(String));
            table.Columns.Add("State", typeof(String));
            table.Columns.Add("PostalCode", typeof(String));
            table.Columns.Add("PhoneNumber", typeof(String));
            table.Columns.Add("FaxNumber", typeof(String));
            table.Columns.Add("RegDate", typeof(String));
            table.Columns.Add("Gender", typeof(String));
            table.Columns.Add("Country", typeof(String));
            table.Columns.Add("Title", typeof(String));

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "SELECT [User].*, g.Name AS Gender, c.Name AS Country, t.Name AS Title FROM [User]"
                 + " LEFT JOIN [Gender] AS g on g.GenderId = [User].GenderId"
                 + " LEFT JOIN [Country] AS c on c.CountryId = [User].CountryId"
                 + " LEFT JOIN [Title] AS t on t.TitleId = [User].TitleId"
                 + " WHERE [User].UserId = " + userId;
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table.Rows.Add(
                        reader["Email"],
                        reader["Username"],
                        reader["FullName"],
                        reader["Instituition"],
                        reader["Faculty"],
                        reader["Department"],
                        reader["ResearchField"],
                        reader["Address"],
                        reader["State"],
                        reader["PostalCode"],
                        reader["PhoneNumber"],
                        reader["FaxNumber"],
                        reader["RegDate"],
                        reader["Gender"],
                        reader["Country"],
                        reader["Title"]
                    );
                }
            }

            reader.Close();
            dbConnection.Close();

            return table;
        }

        public DataTable getEvents()
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "SELECT * FROM [Conference];";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();

            if (reader.HasRows)
            {
                table.Columns.Add("ConferenceId", typeof(long));
                table.Columns.Add("Short_Name", typeof(String));
                table.Columns.Add("Date", typeof(String));
                table.Columns.Add("ConferenceVenue", typeof(String));
                table.Columns.Add("Logo", typeof(String));

                while (reader.Read())
                {
                    table.Rows.Add(
                        reader["ConferenceId"], 
                        reader["Short_Name"], 
                        reader["Date"], 
                        reader["ConferenceVenue"],
                        Convert.ToBase64String((byte[])reader["Logo"])
                    );
                }
            }

            reader.Close();
            dbConnection.Close();

            return table;
        }

        public DataTable getRegisterEventOption(String conferenceId) {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            DataTable table = new DataTable();
            table.Columns.Add("Fee", typeof(DataTable));
            table.Columns.Add("UserType", typeof(DataTable));

            String query = "SELECT * FROM [Fee] WHERE ConferenceId = " + conferenceId + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable fee = new DataTable();
            if (reader.HasRows)
            {
                fee.Columns.Add("FeeId", typeof(String));
                fee.Columns.Add("Category", typeof(String));
                fee.Columns.Add("EaryBird", typeof(String));
                fee.Columns.Add("Normal", typeof(String));

                while (reader.Read())
                {
                    fee.Rows.Add(
                        reader["FeeId"],
                        reader["Category"],
                        reader["EarlyBird"],
                        reader["Normal"]
                    );
                }
            }
            reader.Close();

            query = "SELECT * FROM [UserType];";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            DataTable usertype = new DataTable();
            if (reader.HasRows)
            {
                usertype.Columns.Add("UserTypeId", typeof(String));
                usertype.Columns.Add("Name", typeof(String));

                while (reader.Read())
                {
                    usertype.Rows.Add(
                        reader["UserTypeId"],
                        reader["Name"]
                    );
                }
            }
            reader.Close();

            table.Rows.Add(
                fee,
                usertype
            );

            dbConnection.Close();

            return table;
        }

        public bool registerEvents(String conferenceId, String feeId, String userId, String userTypeId)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "INSERT INTO [Attendees] (ConferenceId, FeeId, UserId, UserTypeId) VALUES (" + conferenceId + "," + feeId + "," + userId + "," + userTypeId + ");";

            SqlCommand command = new SqlCommand(query, dbConnection);
            int result = command.ExecuteNonQuery();

            dbConnection.Close();

            return result > 0;
        }
    }
}