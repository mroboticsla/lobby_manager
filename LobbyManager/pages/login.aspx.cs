using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    /// <summary>
    /// Clase principal para la pantalla de Histórico de Visitantes
    /// </summary>
    public partial class login : System.Web.UI.Page
    {
        static String html = "";

        /// <summary>
        /// Función que se ejecuta al inicar la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            String strHostName = "";
            strHostName = Dns.GetHostName();
            //MessageBox.Show(strHostName.ToString());
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 0; i < addr.Length; i++)
            {
                //MessageBox.Show(addr[i].ToString());
            }

            machineLabel.Text = strHostName;
        }
    }
}