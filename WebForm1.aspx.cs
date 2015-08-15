using ConferenceRESTSystem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ConferenceWebREST
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
            {
                return;
            }

            byte[] buffer = File.ReadAllBytes(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Image") + "/" + Int32.Parse(Request.QueryString["id"]) % 8 + ".jpg");

            SqlConnection dbConnection = DBConnect.getConnection();
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            SqlCommand command = dbConnection.CreateCommand();
            command.CommandText = "UPDATE Conference SET logo = @image WHERE ConferenceId=" + Request.QueryString["id"];
            //command.Text="INSERT INTO YOUR_TABLE_NAME (image) values (@image)";
            command.Parameters.AddWithValue("@image",buffer);
            command.ExecuteNonQuery();
        }
    }
}