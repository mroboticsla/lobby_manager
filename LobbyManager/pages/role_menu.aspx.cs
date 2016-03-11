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
    public partial class role_menu : System.Web.UI.Page
    {
        static String mainConnectionString = "SykesVisitorsDB";
        /// <summary>
        /// Guarda el ID de visitante al que se ha de asociar el equipo ingresado.
        /// </summary>
        public String roleID = "";

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
            if (Request.QueryString["role"] != null)
            {
                roleID = Request.QueryString["role"].ToString();
                if (!roleID.Equals(""))
                {
                    SqlDataSourceList.SelectCommand = "select isnull((select a.role_access from tbl_role_menu a where a.role_menu = " + roleID + "), 2) role_access, " +
                                                    "b.menu_id, b.menu_label, b.menu_file, b.menu_icon, b.menu_root_level, b.menu_root " +
                                                    "from tbl_menu b " +
                                                    "order by b.menu_id, b.menu_root, b.menu_root_level asc";
                    
                    try
                    {
                        string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                        using (var conn = new SqlConnection(connStr))
                        using (var cmd = conn.CreateCommand())
                        {
                            conn.Open();
                            cmd.CommandText = "Select role_name from tbl_roles where role_id = @reg_id";
                            cmd.Parameters.AddWithValue("reg_id", roleID);
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                lblRoleTitle.Text = dr["role_name"].ToString().Trim();
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception a)
                    {
                        Response.Write(a.Message);
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
    }
}