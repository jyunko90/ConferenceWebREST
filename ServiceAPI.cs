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

        public bool signup(String Email, String Username, String TitleId, String FullName, String GenderId, String Instituition, String Faculty, String Department, String ResearchField, String Address, String State, String PostalCode, String CountryId, String PhoneNumber, String FaxNumber, String encryptedPassword)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = String.Format("INSERT INTO [User] " +
                " (Email, Username, encryptedPassword, TitleId, FullName, GenderId, Instituition, Faculty, Department, ResearchField, Address, State, PostalCode, CountryId, PhoneNumber, FaxNumber, LoggedIn) " +
                " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', 0);",
                Email, Username, encryptedPassword, TitleId, FullName, GenderId, Instituition, Faculty, Department, ResearchField, Address, State, PostalCode, CountryId, PhoneNumber, FaxNumber);

            SqlCommand command = new SqlCommand(query, dbConnection);
            int result = command.ExecuteNonQuery();

            dbConnection.Close();

            return result > 0;
        }

        public DataTable login(String Username, String encryptedPassword)
        {
            long UserId = -1;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "SELECT * FROM [User] WHERE Username='" + Username + "' AND encryptedPassword='" + encryptedPassword + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    UserId = Convert.ToInt64(reader["UserId"]);
                }
            }
            reader.Close();

            DataTable conference = new DataTable();
            if (UserId != -1)
            {
                query = "SELECT DISTINCT Conference.*, c.WelcomeText AS WelcomeText  FROM [Conference] LEFT JOIN [Content] AS c on c.ConferenceId = Conference.ConferenceId LEFT JOIN [Attendee] AS a on a.ConferenceId = Conference.ConferenceId WHERE a.UserId = " + UserId;
                //query = "SELECT DISTINCT ConferenceId FROM [Attendee] WHERE UserId = " + UserId;
                command = new SqlCommand(query, dbConnection);
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    setConferencesDataTable(conference);

                    while (reader.Read())
                    {
                        setConferencesData(conference, reader);
                    }
                }
                reader.Close();
            }

            setConferenceEvents(conference);

            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(long));
            table.Columns.Add("Event", typeof(DataTable));
            table.Rows.Add(
                UserId,
                conference
            );

            dbConnection.Close();

            return table;
        }

        public DataTable getUserDetail(String UserId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("User", typeof(DataTable));
            table.Columns.Add("Gender", typeof(DataTable));
            table.Columns.Add("Title", typeof(DataTable));
            table.Columns.Add("Country", typeof(DataTable));

            DataTable user = new DataTable();
            user.Columns.Add("Email", typeof(String));
            user.Columns.Add("Username", typeof(String));
            user.Columns.Add("FullName", typeof(String));
            user.Columns.Add("Instituition", typeof(String));
            user.Columns.Add("Faculty", typeof(String));
            user.Columns.Add("Department", typeof(String));
            user.Columns.Add("ResearchField", typeof(String));
            user.Columns.Add("Address", typeof(String));
            user.Columns.Add("State", typeof(String));
            user.Columns.Add("PostalCode", typeof(String));
            user.Columns.Add("PhoneNumber", typeof(String));
            user.Columns.Add("FaxNumber", typeof(String));
            user.Columns.Add("Gender", typeof(String));
            user.Columns.Add("Country", typeof(String));
            user.Columns.Add("Title", typeof(String));
            user.Columns.Add("encryptedPassword", typeof(String));

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "SELECT [User].*, g.Name AS Gender, c.Name AS Country, t.Name AS Title FROM [User]"
                 + " LEFT JOIN [Gender] AS g on g.GenderId = [User].GenderId"
                 + " LEFT JOIN [Country] AS c on c.CountryId = [User].CountryId"
                 + " LEFT JOIN [Title] AS t on t.TitleId = [User].TitleId"
                 + " WHERE [User].UserId = " + UserId;
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Rows.Add(
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
                        reader["Gender"],
                        reader["Country"],
                        reader["Title"],
                        reader["encryptedPassword"]
                    );
                }
            }
            reader.Close();

            query = "SELECT * FROM [Gender];";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

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
                user,
                gender,
                title,
                country
            );

            dbConnection.Close();

            return table;
        }

        public bool changeUserDetail(String UserId, String Email, String Username, String TitleId, String FullName, String GenderId, String Instituition, String Faculty, String Department, String ResearchField, String Address, String State, String PostalCode, String CountryId, String PhoneNumber, String FaxNumber, String encryptedPassword)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = String.Format("UPDATE [User] SET " +
                " Email = '{0}', " +
                " Username = '{1}', " +
                " encryptedPassword = '{2}', " +
                " TitleId = '{3}', " +
                " FullName = '{4}', " +
                " GenderId = '{5}', " +
                " Instituition = '{6}', " +
                " Faculty = '{7}', " +
                " Department = '{8}', " +
                " ResearchField = '{9}', " +
                " Address = '{10}', " +
                " State = '{11}', " +
                " PostalCode = '{12}', " +
                " CountryId = '{13}', " +
                " PhoneNumber = '{14}', " +
                " FaxNumber = '{15}' " +
                " WHERE UserId = '{16}';",
                Email, Username, encryptedPassword, TitleId, FullName, GenderId, Instituition, Faculty, Department, ResearchField, Address, State, PostalCode, CountryId, PhoneNumber, FaxNumber, UserId);

            SqlCommand command = new SqlCommand(query, dbConnection);
            int result = command.ExecuteNonQuery();

            dbConnection.Close();

            return result > 0;
        }


        private void setConferencesDataTable(DataTable table)
        {
            table.Columns.Add("ConferenceId", typeof(long));
            table.Columns.Add("Username", typeof(String));
            table.Columns.Add("encryptedPassword", typeof(String));
            table.Columns.Add("Website", typeof(String));
            table.Columns.Add("Date", typeof(String));
            table.Columns.Add("ContactName", typeof(String));
            table.Columns.Add("Contact", typeof(String));
            table.Columns.Add("PaperPrefix", typeof(String));
            table.Columns.Add("LoggedIn", typeof(String));
            table.Columns.Add("Logo", typeof(String));
            table.Columns.Add("Short_Name", typeof(String));
            table.Columns.Add("ChairmanName", typeof(String));
            table.Columns.Add("ChairmanEmail", typeof(String));
            table.Columns.Add("ConferencePhone", typeof(String));
            table.Columns.Add("SecretariatAddress", typeof(String));
            table.Columns.Add("ConferenceTime", typeof(String));
            table.Columns.Add("ConferenceVenue", typeof(String));
            table.Columns.Add("Delete", typeof(String));
            table.Columns.Add("ConferenceName", typeof(String));
            table.Columns.Add("WelcomeText", typeof(String));
            table.Columns.Add("Attendee", typeof(Boolean));
            table.Columns.Add("Event", typeof(DataTable));
        }

        private void setConferencesData(DataTable table, SqlDataReader reader)
        {
            table.Rows.Add(
                reader["ConferenceId"],
                reader["Username"],
                reader["encryptedPassword"],
                reader["Website"],
                reader["Date"],
                reader["ContactName"],
                reader["Contact"],
                reader["PaperPrefix"],
                reader["LoggedIn"],
                //null,
                reader["Logo"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["Logo"]) : null,
                reader["Short_Name"],
                reader["ChairmanName"],
                reader["ChairmanEmail"],
                reader["ConferencePhone"],
                reader["SecretariatAddress"],
                reader["ConferenceTime"],
                reader["ConferenceVenue"],
                reader["Delete"],
                reader["ConferenceName"],
                //null,
                reader["WelcomeText"],
                false,
                null
            );
        }

        private void setConferenceEvents(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                String subquery = "SELECT * FROM Event WHERE confId=" + row["ConferenceId"];
                SqlCommand subcommand = new SqlCommand(subquery, dbConnection);
                SqlDataReader subreader = subcommand.ExecuteReader();

                if (subreader.HasRows)
                {
                    DataTable events = new DataTable();
                    setEventsDataTable(events);

                    while (subreader.Read())
                    {
                        setEventsData(events, subreader);
                    }

                    row["Event"] = events;
                }

                subreader.Close();
            }
        }

        private void setEventsDataTable(DataTable table)
        {
            table.Columns.Add("Id", typeof(long));
            table.Columns.Add("text", typeof(String));
            table.Columns.Add("start_date", typeof(String));
            table.Columns.Add("end_date", typeof(String));
            table.Columns.Add("confId", typeof(long));
            table.Columns.Add("pId", typeof(long));
            table.Columns.Add("Venue", typeof(String));
        }

        private void setEventsData(DataTable table, SqlDataReader reader)
        {
            table.Rows.Add(
                reader["Id"],
                reader["text"],
                reader["start_date"],
                reader["end_date"],
                reader["confId"],
                reader["pId"],
                reader["Venue"]
            );
        }


        public DataTable getConferences(String UserId)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            //String query = "SELECT Conference.*, c.WelcomeText AS WelcomeText, a.AttendeeId AS AttendeeId  FROM [Conference] LEFT JOIN [Content] AS c on c.ConferenceId = Conference.ConferenceId LEFT JOIN [Attendee] AS a on a.ConferenceId = Conference.ConferenceId AND a.UserId = " + UserId;
            String query = "SELECT Conference.*, c.WelcomeText AS WelcomeText FROM [Conference] LEFT JOIN [Content] AS c on c.ConferenceId = Conference.ConferenceId;";
            //String query = "SELECT * FROM [Conference]";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();

            if (reader.HasRows)
            {
                setConferencesDataTable(table);

                while (reader.Read())
                {
                    setConferencesData(table, reader);
                }
            }

            reader.Close();

            setConferenceEvents(table);

            if (!String.IsNullOrEmpty(UserId))
            {
                foreach (DataRow row in table.Rows)
                {
                    String subquery = "SELECT * FROM Attendee WHERE ConferenceID=" + row["ConferenceId"] + " AND UserId=" + UserId;
                    SqlCommand subcommand = new SqlCommand(subquery, dbConnection);
                    SqlDataReader subreader = subcommand.ExecuteReader();

                    Console.WriteLine(subquery);
                    System.Diagnostics.Debug.WriteLine(subquery);
                    row["Attendee"] = subreader.HasRows;

                    subreader.Close();
                }
            }

            dbConnection.Close();

            return table;
        }


        public DataTable getRegisterEventOption(String ConferenceId)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            DataTable table = new DataTable();
            table.Columns.Add("Fee", typeof(DataTable));
            table.Columns.Add("UserType", typeof(DataTable));

            String query = "SELECT * FROM [Fee] WHERE ConferenceId = " + ConferenceId + ";";
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

        public bool registerEvent(String ConferenceId, String FeeId, String UserId, String UserTypeId)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            String query = "INSERT INTO [Attendee] (ConferenceId, FeeId, UserId, UserTypeId) VALUES (" + ConferenceId + "," + FeeId + "," + UserId + "," + UserTypeId + ");";

            SqlCommand command = new SqlCommand(query, dbConnection);
            int result = command.ExecuteNonQuery();

            dbConnection.Close();

            return result > 0;
        }
    }
}