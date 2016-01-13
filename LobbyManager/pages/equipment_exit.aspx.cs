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
    /// <summary>
    /// Clase principal para el formulario de Ingreso de Equipo
    /// </summary>
    public partial class equipment_exit : System.Web.UI.Page
    {
        static String mainConnectionString = "SykesVisitorsDB";
        /// <summary>
        /// Guarda el ID de visitante al que se ha de asociar el equipo ingresado.
        /// </summary>
        public String visitorID = "";

        /// <summary>
        /// Contiene la variable de aprobacion
        /// </summary>
        public String app = "";

        /// <summary>
        /// Controla el flujo de aprobacion en la pantalla.
        /// </summary>
        public bool approved = false;

        private const string TEMPLATE_DIRECTORY = @"C:\Program Files\Brother bPAC3 SDK\Templates\";	// Template file path
        private const string TEMPLATE_SIMPLE = "BcdItem.lbx";	// Template file name
        private const string TEMPLATE_FRAME = "NamePlate2.LBX";		// Template file name
        
        /// <summary>
        /// Se ejcuta al iniciar la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            msgWarn.Visible = false;
            visitorID = Request.QueryString["visitor"].ToString();
            lblTitle.Text = getVisitorName(visitorID);

            SqlDataSourceList.SelectCommand = "SELECT reg_id, reg_type, type_name, reg_quantity, reg_serial, reg_desc FROM tbl_reg_equipment, tbl_type_equipment where type_id = reg_type and reg_visitor = @reg_visitor and reg_status = 1";
            SqlDataSourceList.SelectParameters.Add("reg_visitor", visitorID);

            SqlDataSourceListEq.SelectCommand = "SELECT reg_id, reg_type, type_name, reg_quantity, reg_serial, reg_desc FROM tbl_reg_equipment, tbl_type_equipment where type_id = reg_type and reg_visitor = @reg_visitor and reg_status = 0";
            SqlDataSourceListEq.SelectParameters.Add("reg_visitor", visitorID);

            if (Request.QueryString["approve"] != null)
            {
                app = Request.QueryString["approve"].ToString();
                if (!app.Equals(""))
                {
                    try
                    {
                        approved = app.Equals("true");
                    }
                    catch
                    {
                        approved = false;
                    }
                }
            }

            btnExecutePrint.Visible = approved;
        }

        /// <summary>
        /// Método para eliminar un registro de equipo ingresado utilizando el ID interno del registro.
        /// </summary>
        /// <param name="reg_id">ID interno del Equipo ingresado.</param>
        /// <returns>Respuesta de la ejecución de la función</returns>
        [System.Web.Services.WebMethod]
        public static String deleteRecord(String reg_id)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "DELETE FROM tbl_reg_equipment where reg_id = @reg_id";
                    cmd.Parameters.AddWithValue("reg_id", reg_id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch
            { }
            return "ok";
        }

        /// <summary>
        /// Método para actualizar un registro de equipo ingresado utilizando el ID interno del registro.
        /// </summary>
        /// <param name="reg_id">ID interno del Equipo ingresado.</param>
        /// <returns>Respuesta de la ejecución de la función</returns>
        [System.Web.Services.WebMethod]
        public static String checkRecord(String reg_id)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE tbl_reg_equipment SET reg_status = @reg_status where reg_id = @reg_id";
                    cmd.Parameters.AddWithValue("reg_id", reg_id);
                    cmd.Parameters.AddWithValue("reg_status", 0);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch
            { }
            return "ok";
        }

        /// <summary>
        /// Obtiene el nombre del visitante a partir de su correlativo de visita.
        /// </summary>
        /// <param name="visitor">ID de visitante</param>
        /// <returns></returns>
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
                    _result = dreader["vis_name"].ToString().Trim() + " " + dreader["vis_lastname"].ToString().Trim();
                }
                dreader.Close();
                conn.Close();
            }

            return _result;
        }

        /// <summary>
        /// Guarda los datos de equipo a ingresar
        /// </summary>
        /// <param name="sender">Objeto que ejecuta la acción</param>
        /// <param name="e">Evento Ejecutado</param>
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
                        reg_id = int.Parse(dreader["com_total"].ToString().Trim());
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
                Response.Write(a.Message);
            }
        }
        
        /// <summary>
        /// Limpia el formulario de ingreso de equipo.
        /// </summary>
        public void CleanForm()
        {
            txt_serial.Value = "";
            txt_desc.Value = "";
            txt_quantity.Value = "";
            msgWarn.Visible = false;
            Response.Redirect(Request.Url.ToString()); 
        }

        /// <summary>
        /// Controla el evento del botón para la impresión de viñetas en el impresor Brother QL 700
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
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
                        String name = dreader["type_name"].ToString().Trim();
                        doc.GetObject("objName").Text = name;
                        doc.GetObject("objSerial").Text = dreader["reg_serial"].ToString().Trim();
                        doc.GetObject("objBarcode").Text = dreader["reg_id"].ToString().Trim();
                        doc.GetObject("objDesc").Text = dreader["reg_desc"].ToString().Trim();
                        doc.GetObject("objOwner").Text = lblTitle.Text.Trim();

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

        /// <summary>
        /// Controla el evento de cancelación en el ingreso de equipo.
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        protected void btnCancelForm_Click(object sender, EventArgs e)
        {
            CleanForm();
        }
    }
}