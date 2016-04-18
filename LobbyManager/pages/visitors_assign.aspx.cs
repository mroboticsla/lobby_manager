using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    /// <summary>
    /// Clase principal para el formulario de asignación de gafetes, impresión de viñetas y finalización de visitas.
    /// </summary>
    public partial class visitors_assign : System.Web.UI.Page
    {
        static String html = "";
        static String mainConnectionString = "SykesVisitorsDB";

        /// <summary>
        /// Se ejecuta al iniciar la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            mainTitle.InnerText = "Visitantes en Espera - " + Session["usr_device"].ToString();
            SqlDataSourceVisitors.SelectCommand = "SELECT vis_id, vis_date, vis_department, upper(vis_name) vis_name, upper(vis_lastname) vis_lastname, upper(vis_internal_contact) vis_internal_contact, upper(dep_name) dep_name, img_profile, vis_with_equipment, " + 
                            "isnull((select vis_alert_level from tbl_vis_blacklist b where b.vis_document = a.vis_docnumber or (UPPER(RTRIM(a.vis_name)) like '%' + UPPER(RTRIM(b.vis_name)) + '%' and UPPER(RTRIM(a.vis_lastname)) like '%' + UPPER(RTRIM(b.vis_lastname)) + '%')), '') alert " +
                            "FROM [tbl_vis_visitors] a, tbl_dep_departments, tbl_img_images, tbl_log_events " +
                            "where dep_id = vis_department and vis_status = 1 and img_visitor = vis_id and log_visitor_record = vis_id and log_user = '" + Session["usr_device"].ToString() + "' " +
                            "order by vis_id desc";
        }

        /// <summary>
        /// Establece el contenido html a exportar.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static String setHTML(String str)
        {
            html = str;
            return str;
        }

        /// <summary>
        /// Exporta a XLS
        /// </summary>
        /// <param name="dt"></param>
        public void ExportToExcel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string filename = "Historico de Visitantes " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }

        /// <summary>
        /// Finaliza la visita.
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        [System.Web.Services.WebMethod]
        public static void finishVisit(int visitor)
        {
            try
            {
                int vis_id = visitor;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE [tbl_vis_visitors] SET vis_status = @vis_status, vis_checkout = GETDATE() \n" +
                                    "WHERE vis_id = @vis_id";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    cmd.Parameters.AddWithValue("vis_status", 0);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// Ejecuta la exportación a XLS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportTable(object sender, EventArgs e)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var hnodes = doc.DocumentNode.SelectNodes("//table/thead/tr");
            var nodes = doc.DocumentNode.SelectNodes("//table/tbody/tr");
            var table = new DataTable("MyTable");

            var headers = hnodes[0]
                .Elements("th")
                .Select(th => th.InnerText.Trim());
            foreach (var header in headers)
            {
                table.Columns.Add(header);
            }

            var rows = nodes.Skip(1).Select(tr => tr
                .Elements("td")
                .Select(td => td.InnerText.Trim())
                .ToArray());
            foreach (var row in rows)
            {
                table.Rows.Add(row);
            }

            ExportToExcel(table);
        }
    }
}