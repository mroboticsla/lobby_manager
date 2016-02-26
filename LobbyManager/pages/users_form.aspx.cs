using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bpac;
using System.Security.Cryptography;
using System.Text;

namespace LobbyManager.pages
{
    /// <summary>
    /// Clase principal para el formulario de Ingreso de Equipo
    /// </summary>
    public partial class users_form : System.Web.UI.Page
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
            SqlDataSourceList.SelectCommand = "SELECT usr_id, usr_role, usr_username, usr_name, role_name FROM tbl_usr_users, tbl_roles where role_id = usr_role";
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
                    cmd.CommandText = "DELETE FROM tbl_usr_users where dev_id = @reg_id";
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
                int reg_id = -1;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(usr_id), 0) + 1 AS com_total FROM [tbl_usr_users]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        reg_id = int.Parse(dreader["com_total"].ToString().Trim());
                    }
                    dreader.Close();
                    conn.Close();
                }

                var sha1 = new SHA1CryptoServiceProvider();
                var data = Encoding.ASCII.GetBytes(txt_pass.Value);
                var sha1data = sha1.ComputeHash(data);

                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO tbl_usr_users (usr_id, usr_role, usr_username, usr_password, usr_status, usr_name) \n" +
                                      "values (@usr_id, @usr_role, @usr_username, @usr_password, @usr_status, @usr_name)";
                    cmd.Parameters.AddWithValue("usr_id", reg_id);
                    cmd.Parameters.AddWithValue("usr_role", roleSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("usr_username", txt_usr.Value);
                    cmd.Parameters.AddWithValue("usr_password", sha1data);
                    cmd.Parameters.AddWithValue("usr_name", txt_name.Value);
                    cmd.Parameters.AddWithValue("usr_status", (chk_active.Checked) ? "1" : "0");
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
            txt_usr.Value = "";
            txt_pass.Value = "";
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