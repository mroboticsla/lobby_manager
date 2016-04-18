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
    public partial class documents_form : System.Web.UI.Page
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
            SqlDataSourceList.SelectCommand = "SELECT doc_id, doc_name, doc_abreviature, doc_is_national_idcard FROM tbl_doc_documents";

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
                    cmd.CommandText = "DELETE FROM tbl_doc_documents where doc_id = @reg_id";
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
        /// Guarda los datos de equipo a ingresar
        /// </summary>
        /// <param name="sender">Objeto que ejecuta la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        protected void saveItem(object sender, EventArgs e)
        {
            if (txt_name.Value.Trim().Length == 0)
            {
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
                    cmd.CommandText = "SELECT isnull(MAX(doc_id), 0) + 1 AS com_total FROM [tbl_doc_documents]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        reg_id = int.Parse(dreader["com_total"].ToString().Trim());
                    }
                    dreader.Close();
                    conn.Close();
                }

                if (!recordOption.Value.Equals("E"))
                {
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "INSERT INTO [tbl_doc_documents] (doc_id, doc_name, doc_abreviature, doc_is_national_idcard, doc_status) \n" +
                                          "values (@doc_id, @doc_name, @doc_abreviature, @doc_is_national_idcard, @doc_status)";
                        cmd.Parameters.AddWithValue("doc_id", reg_id);
                        cmd.Parameters.AddWithValue("doc_name", txt_name.Value);
                        cmd.Parameters.AddWithValue("doc_abreviature", txt_abr.Value);
                        cmd.Parameters.AddWithValue("doc_is_national_idcard", (chk_oficial.Checked) ? "1" : "0");
                        cmd.Parameters.AddWithValue("doc_status", 1);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        CleanForm();
                    }
                }
                else
                {
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "UPDATE [tbl_doc_documents] SET doc_name = @doc_name, doc_abreviature = @doc_abreviature, doc_is_national_idcard = @doc_is_national_idcard \n" +
                                          "WHERE doc_id = @doc_id";
                        cmd.Parameters.AddWithValue("doc_id", selectedID.Value);
                        cmd.Parameters.AddWithValue("doc_name", txt_name.Value);
                        cmd.Parameters.AddWithValue("doc_abreviature", txt_abr.Value);
                        cmd.Parameters.AddWithValue("doc_is_national_idcard", (chk_oficial.Checked) ? "1" : "0");
                        cmd.Parameters.AddWithValue("doc_status", 1);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        CleanForm();
                    }
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
            txt_name.Value = "";
            txt_abr.Value = "";
            chk_oficial.Checked = false;
            Response.Redirect(Request.Url.ToString()); 
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