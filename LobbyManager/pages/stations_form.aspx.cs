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
    public partial class stations_form : System.Web.UI.Page
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
        
        /// <summary>
        /// Se ejcuta al iniciar la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            msgWarn.Visible = false;
            SqlDataSourceList.SelectCommand = "SELECT dev_id, dev_name, dev_status FROM tbl_dev_stations";
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
                    cmd.CommandText = "DELETE FROM tbl_dev_stations where dev_id = @reg_id";
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
                msgWarn.Visible = true;
                return;
            }
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_dev_stations] (dev_id, dev_name, dev_status) \n" +
                                      "values (@dev_id, @dev_name, @dev_status)";
                    cmd.Parameters.AddWithValue("dev_id", txt_serviceID.Value);
                    cmd.Parameters.AddWithValue("dev_name", txt_name.Value);
                    cmd.Parameters.AddWithValue("dev_status", (chk_active.Checked) ? "1" : "0");
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
            txt_serviceID.Value = "";
            txt_name.Value = "";
            chk_active.Checked = false;
            msgWarn.Visible = false;
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