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
    public partial class visitors_assign : System.Web.UI.Page
    {
        static String mainConnectionString = "SykesVisitorsDB";
        static String html = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        [System.Web.Services.WebMethod]
        public static String setHTML(String str)
        {
            html = str;
            return str;
        }

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