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
using System.Security.Cryptography;
using System.Text;
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
        String mainConnectionString = "SykesVisitorsDB";
        
        /// <summary>
        /// Función que se ejecuta al inicar la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            String strHostName = "";
            try
            {
                strHostName = Dns.GetHostName();
                //MessageBox.Show(strHostName.ToString());
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;

                for (int i = 0; i < addr.Length; i++)
                {
                    //MessageBox.Show(addr[i].ToString());
                }

                txt_device.Text = "Device: " + strHostName;
                Session["usr_device"] = strHostName;
            }
            catch
            {
                txt_device.Text = "Device: N/A";
            }
            Boolean isClient = false;
            Boolean isStation = false;
            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "select dev_id, dev_name, dev_station, dev_status \n" +
                    "from tbl_dev_clients where dev_name = @dev_name and dev_status = 1";
                cmd.Parameters.AddWithValue("dev_name", strHostName);
                SqlDataReader dreader = cmd.ExecuteReader();
                if (dreader.Read())
                {
                    isClient = true;
                    strHostName = dreader["dev_station"].ToString();
                }
                dreader.Close();
                conn.Close();
            }

            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "select dev_id, dev_name, dev_status \n" +
                    "from tbl_dev_stations where dev_name = @dev_name and dev_status = 1";
                cmd.Parameters.AddWithValue("dev_name", strHostName);
                SqlDataReader dreader = cmd.ExecuteReader();
                if (dreader.Read())
                {
                    isStation = true;
                    strHostName = dreader["dev_id"].ToString();
                }
                dreader.Close();
                conn.Close();
            }

            Session["usr_device"] = strHostName;

            if (isClient)
            {
                Response.Redirect("visitors_mobile.aspx", true);
            }
            else if (Session["usr_id"] != null)
            {
                Response.Redirect("admin.aspx", true);
            }
            else
            {
                Session["usr_id"] = null;
                Session["usr_role"] = null;
                Session["usr_name"] = null;
            }
        }

        /// <summary>
        /// Función que inicia el proceso de verificación de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DoLogin(object sender, EventArgs e)
        {
            Boolean valid = false;
            var sha1 = new SHA1CryptoServiceProvider();
            var data = Encoding.ASCII.GetBytes(txt_pwd.Value);
            var sha1data = sha1.ComputeHash(data);
            byte[] stored = null;

            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            /*
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "update tbl_usr_users set usr_username = @usr_username, usr_password = @usr_password";
                cmd.Parameters.AddWithValue("usr_username", txt_usr.Text);
                cmd.Parameters.AddWithValue("usr_password", sha1data);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            */
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "select usr_id, usr_role, usr_password, usr_status, usr_name \n" +
                    "from tbl_usr_users where usr_username = @usr_username and usr_status = 1";
                cmd.Parameters.AddWithValue("usr_username", txt_usr.Text);
                SqlDataReader dreader = cmd.ExecuteReader();
                if (dreader.Read())
                {
                    stored = (byte[])dreader["usr_password"];

                    if (stored != null)
                    {
                        if (sha1data.SequenceEqual(stored))
                        {
                            valid = true;
                            Session["usr_id"] = dreader["usr_id"].ToString();
                            Session["usr_role"] = dreader["usr_role"].ToString();
                            Session["usr_name"] = dreader["usr_name"].ToString();
                        }
                        else
                        {
                            msgError.Visible = true;
                        }
                    }
                }
                dreader.Close();
                conn.Close();
            }

            if (valid) Response.Redirect("admin.aspx", true);
        }
    }
}