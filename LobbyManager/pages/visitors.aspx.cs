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
    public partial class visitors : System.Web.UI.Page
    {
        String mainConnectionString = "SykesVisitorsDB";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InsertVisitor(object sender, EventArgs e)
        {
            try
            {
                int vis_id = -1;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(vis_id), 0) + 1 AS com_total FROM [tbl_vis_visitors]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        vis_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }
                
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_vis_visitors] (vis_id, vis_name, vis_lastname, vis_doctype, vis_docnumber, vis_company, vis_phone, vis_reason, vis_description, vis_department, vis_internal_contact, vis_date, vis_status, vis_image_record, vis_with_equipment) \n" +
                                      "values (@vis_id, @vis_name, @vis_lastname, @vis_doctype, @vis_docnumber, @vis_company, @vis_phone, @vis_reason, @vis_description, @vis_department, @vis_internal_contact, GETDATE(), @vis_status, @vis_image_record, @vis_with_equipment)";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    cmd.Parameters.AddWithValue("vis_name", txt_name.Value);
                    cmd.Parameters.AddWithValue("vis_lastname", txt_lastname.Value);
                    cmd.Parameters.AddWithValue("vis_doctype", docTypeSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("vis_docnumber", txt_docnumber.Value);
                    cmd.Parameters.AddWithValue("vis_company", txt_company.Value);
                    cmd.Parameters.AddWithValue("vis_phone", txt_phone.Value);
                    cmd.Parameters.AddWithValue("vis_reason", reasonSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("vis_description", txt_description.Value);
                    cmd.Parameters.AddWithValue("vis_department", deptSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("vis_internal_contact", txt_contact.Value);
                    cmd.Parameters.AddWithValue("vis_status", 1);
                    cmd.Parameters.AddWithValue("vis_image_record", 0);
                    cmd.Parameters.AddWithValue("vis_with_equipment", (chk_addEQ.Checked) ? 1 : 0);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    CleanForm();
                }

                int log_id = -1;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(log_id), 0) + 1 AS com_total FROM [tbl_log_events]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        log_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }

                int vrs_notification_type = -1;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT vrs_notification_type FROM [tbl_vrs_visit_reasons] WHERE vrs_id = @vrs_id";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        vrs_notification_type = int.Parse(dreader["vrs_notification_type"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }

                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_log_events] (log_id, log_notification, log_user, log_date) \n" +
                                      "values (@log_id, @log_notification, @log_user, GETDATE())";
                    cmd.Parameters.AddWithValue("log_id", log_id);
                    cmd.Parameters.AddWithValue("log_notification", vrs_notification_type);
                    cmd.Parameters.AddWithValue("log_user", "est01");
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    CleanForm();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }
        }

        public void CleanForm()
        {

        }
    }
}