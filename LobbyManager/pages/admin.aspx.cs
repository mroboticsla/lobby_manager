using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    public partial class admin : System.Web.UI.Page
    {
        String mainConnectionString = "SykesVisitorsDB";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_newCommentsCount.Text = getNewComCount();
        }

        public String getNewComCount()
        {
            String _result = "";
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(count(*), 0) AS com_total FROM [tbl_com_comments] where com_hidden = 0";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        _result = dreader["com_total"].ToString();
                    }
                    dreader.Close();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }

            return _result;
        }

        public void hideNewComments(object sender, EventArgs e)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE [tbl_com_comments] set com_hidden = 1 where com_hidden = @hidden";
                    cmd.Parameters.AddWithValue("hidden", 0);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    lbl_newCommentsCount.Text = getNewComCount();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }
        }
    }
}