using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bpac;

namespace LobbyManager.pages
{
    public partial class equipment_form : System.Web.UI.Page
    {
        static String mainConnectionString = "SykesVisitorsDB";
        public String visitorID = "";

        private const string TEMPLATE_DIRECTORY = @"C:\Program Files\Brother bPAC3 SDK\Templates\";	// Template file path
        private const string TEMPLATE_SIMPLE = "BcdItem.lbx";	// Template file name
        private const string TEMPLATE_FRAME = "NamePlate2.LBX";		// Template file name
        
        protected void Page_Load(object sender, EventArgs e)
        {
            msgWarn.Visible = false;
            visitorID = Request.QueryString["visitor"].ToString();
            lblTitle.Text = getVisitorName(visitorID);
            SqlDataSourceList.SelectCommand = "SELECT reg_id, reg_type, type_name, reg_quantity, reg_serial, reg_desc FROM tbl_reg_equipment, tbl_type_equipment where type_id = reg_type and reg_visitor = @reg_visitor";
            SqlDataSourceList.SelectParameters.Add("reg_visitor", visitorID);
        }

        [System.Web.Services.WebMethod]
        public static String deleteRecord(String str)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "DELETE FROM tbl_reg_equipment where reg_id = @reg_id";
                    cmd.Parameters.AddWithValue("reg_id", str);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                //Response.Write(a.Message);
            }
            return "ok";
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

        protected void btnExecutePrint_Click(object sender, EventArgs e)
        {
            string templatePath = TEMPLATE_DIRECTORY;
            templatePath += TEMPLATE_SIMPLE;

            bpac.DocumentClass doc = new DocumentClass();
            if (doc.Open(templatePath) != false)
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT reg_id, reg_type, type_name, reg_quantity, reg_serial, reg_desc FROM tbl_reg_equipment, tbl_type_equipment where type_id = reg_type and reg_visitor = @reg_visitor";
                    cmd.Parameters.AddWithValue("reg_visitor", visitorID);
                    SqlDataReader dreader = cmd.ExecuteReader();
                    while (dreader.Read())
                    {
                        String name = dreader["type_name"].ToString();
                        doc.GetObject("objName").Text = name;
                        doc.GetObject("objSerial").Text = dreader["reg_serial"].ToString();
                        doc.GetObject("objBarcode").Text = dreader["reg_id"].ToString();
                        doc.GetObject("objDesc").Text = dreader["reg_desc"].ToString();
                        doc.GetObject("objOwner").Text = lblTitle.Text;

                        // doc.SetMediaById(doc.Printer.GetMediaId(), true);
                        doc.StartPrint("", PrintOptionConstants.bpoDefault);
                        doc.PrintOut(1, PrintOptionConstants.bpoDefault);
                        doc.EndPrint();
                    }
                    doc.Close();
                    dreader.Close();
                    conn.Close();
                }
                
                        
            }
            else
            {
                //MessageBox.Show("Open() Error: " + doc.ErrorCode);
            }
        }
    }
}