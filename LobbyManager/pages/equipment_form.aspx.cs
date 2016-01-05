using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    public partial class equipment_form : System.Web.UI.Page
    {
        String mainConnectionString = "SykesVisitorsDB";
        public String visitorID = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            msgWarn.Visible = false;
            visitorID = Request.QueryString["visitor"].ToString();
            lblTitle.Text = getVisitorName(visitorID);
            SqlDataSourceList.SelectCommand = "SELECT reg_id, reg_type, type_name, reg_quantity, reg_serial, reg_desc FROM tbl_reg_equipment, tbl_type_equipment where type_id = reg_type and reg_visitor = @reg_visitor";
            SqlDataSourceList.SelectParameters.Add("reg_visitor", visitorID);
        }

        private String getVisitorName(String visitor)
        {
            String _result = "";

            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT vis_name, vis_lastname FROM tbl_vis_visitors where vis_id = @vis_id";
                cmd.Parameters.AddWithValue("vis_id", visitor);
                SqlDataReader dreader = cmd.ExecuteReader();
                if (dreader.Read())
                {
                    _result = dreader["vis_name"].ToString() + " " + dreader["vis_lastname"].ToString();
                }
                dreader.Close();
                conn.Close();
            }

            return _result;
        }

        protected void saveItem(object sender, EventArgs e)
        {
            if (txt_desc.Value.Trim().Length == 0)
            {
                msgWarn.Visible = true;
                return;
            }
            try
            {
                int reg_id = -1;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(reg_id), 0) + 1 AS com_total FROM [tbl_reg_equipment]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        reg_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }

                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_reg_equipment] (reg_id, reg_type, reg_quantity, reg_serial, reg_desc, reg_visitor, reg_status, reg_last_update) \n" +
                                      "values (@reg_id, @reg_type, @reg_quantity, @reg_serial, @reg_desc, @reg_visitor, @reg_status, GETDATE())";
                    cmd.Parameters.AddWithValue("reg_id", reg_id);
                    cmd.Parameters.AddWithValue("reg_type", eqTypeSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("reg_quantity", (txt_quantity.Value == "") ? "1" : txt_quantity.Value);
                    cmd.Parameters.AddWithValue("reg_serial", txt_serial.Value);
                    cmd.Parameters.AddWithValue("reg_desc", txt_desc.Value);
                    cmd.Parameters.AddWithValue("reg_visitor", visitorID);
                    cmd.Parameters.AddWithValue("reg_status", 1);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    CleanForm();
                }
            }
            catch (Exception a)
            {
                //Response.Write(a.Message);
            }
        }

        public void CleanForm()
        {
            txt_serial.Value = "";
            txt_desc.Value = "";
            txt_quantity.Value = "";
            msgWarn.Visible = false;
            Response.Redirect(Request.Url.ToString()); 
        }
    }
}