using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    /// <summary>
    /// Clase que contiene las funciones Code-Behind para la página de administración de la aplicación.
    /// </summary>
    public partial class admin : System.Web.UI.Page
    {
        String mainConnectionString = "SykesVisitorsDB";

        /// <summary>
        /// Contiene el resultado en formato JSON para el dibujado de la gráfica de Tipos de Documentos
        /// </summary>
        public String morris_doctype_data = "";
        /// <summary>
        /// Contiene el resultado en formato JSON para el dibujado de la gráfica de Departamentos Visitados
        /// </summary>
        public String morris_department_data = "";
        /// <summary>
        /// Contiene el resultado en formato JSON para el dibujado de la gráfica de visitantes por dia de la semana
        /// </summary>
        public String morris_weekday_data = "";
        
        /// <summary>
        /// Función que se ejcuta al iniciar la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_newCommentsCount.Text = getNewComCount();
            lbl_visitorsCount.Text = getVisitorsCount();
            lbl30.Text = getFromDays("30");
            lbl7.Text = getFromDays("7");
            lblToday.Text = getFromDays("1");
            GetGraphData();
            GraphTimer.Tick += GraphTimer_Tick;
        }

        /// <summary>
        /// Función cíclica a utilizar para la actualización de gráficas en la pantalla de administración del sistema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GraphTimer_Tick(object sender, EventArgs e)
        {
            if (!lbl_visitorsCount.Text.Equals("0"))
            {
                //read data from SQL or file
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), "GraphDT", "GraphDT([" + morris_doctype_data + "]);", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), "GraphDP", "GraphDP([" + morris_department_data + "]);", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), "GraphWD", "GraphWD([" + morris_weekday_data + "]);", true);
            }
        }

        /// <summary>
        /// Actualiza las variables que conciernen al dibujado de las gráficas en la pantalla de administración del sistema.
        /// </summary>
        public void GetGraphData()
        {
            try
            {
                morris_doctype_data = "{label:\"Sin Visitas\", value: 0}";
                morris_department_data = "{label:\"Sin Visitas\", value: 0}";
                morris_weekday_data = "{y:\"Sin Visitas\", a: 0, b: 0}";
                if (!getVisitorsCount().Equals("0"))
                {
                    morris_doctype_data = "";
                    morris_department_data = "";
                    morris_weekday_data = "";
                    string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT doc_id, doc_name FROM [tbl_doc_documents] where doc_status <> 0";
                        SqlDataReader dreader = cmd.ExecuteReader();
                        int count = 0;
                        while (dreader.Read())
                        {
                            using (var conn2 = new SqlConnection(connStr))
                            using (var cmd2 = conn2.CreateCommand())
                            {
                                conn2.Open();
                                cmd2.CommandText = "SELECT isnull(count(vis_doctype), 0) AS com_total FROM [tbl_vis_visitors] where vis_doctype = @vis_doctype";
                                cmd2.Parameters.AddWithValue("vis_doctype", dreader["doc_id"].ToString());
                                SqlDataReader dreader2 = cmd2.ExecuteReader();
                                if (dreader2.Read())
                                {
                                    if (count > 0) morris_doctype_data += ", ";
                                    morris_doctype_data += "{label: \"" + dreader["doc_name"].ToString().Trim() + "\", value:" + dreader2["com_total"].ToString().Trim() + "}";
                                    count++;
                                }
                                dreader2.Close();
                                conn2.Close();
                            }
                        }
                        dreader.Close();
                        conn.Close();
                    }

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT dep_id, dep_name FROM [tbl_dep_departments] where dep_status <> 0";
                        SqlDataReader dreader = cmd.ExecuteReader();
                        int count = 0;
                        while (dreader.Read())
                        {
                            using (var conn2 = new SqlConnection(connStr))
                            using (var cmd2 = conn2.CreateCommand())
                            {
                                conn2.Open();
                                cmd2.CommandText = "SELECT isnull(count(vis_department), 0) AS com_total FROM [tbl_vis_visitors] where vis_department = @vis_department";
                                cmd2.Parameters.AddWithValue("vis_department", dreader["dep_id"].ToString());
                                SqlDataReader dreader2 = cmd2.ExecuteReader();
                                if (dreader2.Read())
                                {
                                    if (count > 0) morris_department_data += ", ";
                                    morris_department_data += "{label: \"" + dreader["dep_name"].ToString().Trim() + "\", value:" + dreader2["com_total"].ToString().Trim() + "}";
                                    count++;
                                }
                                dreader2.Close();
                                conn2.Close();
                            }
                        }
                        dreader.Close();
                        conn.Close();
                    }

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SET LANGUAGE Spanish;";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "select distinct DATEPART(dw, vis_date), DATENAME(dw, vis_date) as 'day', " +
                            "(select count (b.vis_date) from tbl_vis_visitors b where DATEPART(dw, b.vis_date) = DATEPART(dw, a.vis_date) and convert(time, b.vis_date, 108) < convert(time, '12:00:00', 108)) as 'am', " +
                            "(select count (b.vis_date) from tbl_vis_visitors b where DATEPART(dw, b.vis_date) = DATEPART(dw, a.vis_date) and convert(time, b.vis_date, 108) >= convert(time, '12:00:00', 108)) as 'pm' " +
                            "from tbl_vis_visitors a order by DATEPART(dw, vis_date) asc";
                        SqlDataReader dreader = cmd.ExecuteReader();
                        int count = 0;
                        while (dreader.Read())
                        {
                            using (var conn2 = new SqlConnection(connStr))
                            using (var cmd2 = conn2.CreateCommand())
                            {
                                if (count > 0) morris_weekday_data += ", ";
                                morris_weekday_data += "{y: \"" + dreader["day"].ToString().Trim() + "\", a:\"" + dreader["am"].ToString().Trim() + "\", b:\"" + dreader["pm"].ToString().Trim() + "\"}";
                                count++;
                            }
                        }
                        dreader.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }
        }

        /// <summary>
        /// Obtiene el número total de visitantes registrado en los ultimos 30 dias.
        /// </summary>
        /// <returns></returns>
        public String getFromDays(String days)
        {
            String _result = "";
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(count(*), 0) AS com_total FROM [tbl_vis_visitors] WHERE vis_date >= DATEADD(day,-" + days + ",GETDATE())";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        _result = dreader["com_total"].ToString();
                    }
                    dreader.Close();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }

            return _result;
        }

        /// <summary>
        /// Obtiene el número total de visitantes registrado en el sistema.
        /// </summary>
        /// <returns></returns>
        public String getVisitorsCount()
        {
            String _result = "";
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(count(*), 0) AS com_total FROM [tbl_vis_visitors]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        _result = dreader["com_total"].ToString();
                    }
                    dreader.Close();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }

            return _result;
        }

        /// <summary>
        /// Obtiene una lista de los últimos comentarios publicados en la aplicación.
        /// </summary>
        /// <returns></returns>
        public String getNewComCount()
        {
            String _result = "";
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(count(*), 0) AS com_total FROM [tbl_com_comments] where com_hidden = 0";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        _result = dreader["com_total"].ToString();
                    }
                    dreader.Close();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }

            return _result;
        }

        /// <summary>
        /// Actualiza el estado de los comentarios ya leídos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void hideNewComments(object sender, EventArgs e)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE [tbl_com_comments] set com_hidden = 1 where com_hidden = @hidden";
                    cmd.Parameters.AddWithValue("hidden", 0);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    lbl_newCommentsCount.Text = getNewComCount();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }
        }
    }
}