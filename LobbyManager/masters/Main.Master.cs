﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LobbyManager
{
    /// <summary>
    /// Archivo "Master" a utilizar por las páginas de contenido y formularios en la aplicación. Ésta página contiene la definición de los contenedores, administración de notificaciones, mensajería, publicación de comentarios,detalles y opciones de usuario entre otros.
    /// </summary>
    public partial class Main : System.Web.UI.MasterPage
    {
        String mainConnectionString = "SykesVisitorsDB";

        /// <summary>
        /// Funnción ejecutada al iniciar la carga del templete.
        /// </summary>
        /// <param name="sender">Objeto que llama a la accíón</param>
        /// <param name="e">Evento ejecutado</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usr_id"] == null)
            {
                Response.Redirect("login.aspx", true);
            }
        }

        /// <summary>
        /// Ejecuta la acción para guardar comentarios o reportes de error en la aplicación.
        /// </summary>
        /// <param name="sender">Objeto que llama a la accíón</param>
        /// <param name="e">Evento ejecutado</param>
        void btn_sendComment_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int com_id = -1;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(com_id), 0) + 1 AS com_total FROM [tbl_com_comments]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        com_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }
                
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_com_comments] (com_id, com_date, com_user, com_description, com_hidden, com_type) \n" +
                                      "values (@com_id, GETDATE(), @com_user, @com_description, @com_hidden, @com_type)";
                    cmd.Parameters.AddWithValue("com_id", com_id);
                    cmd.Parameters.AddWithValue("com_user", "est01");
                    cmd.Parameters.AddWithValue("com_description", txt_commentDescription);
                    cmd.Parameters.AddWithValue("com_hidden", 0);
                    cmd.Parameters.AddWithValue("com_type", cb_commentType.Value);
                    cmd.ExecuteNonQuery();
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