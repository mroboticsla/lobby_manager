using System;
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
    public partial class Mobile : System.Web.UI.MasterPage
    {
        String mainConnectionString = "SykesVisitorsDB";

        /// <summary>
        /// Funnción ejecutada al iniciar la carga del templete.
        /// </summary>
        /// <param name="sender">Objeto que llama a la accíón</param>
        /// <param name="e">Evento ejecutado</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}