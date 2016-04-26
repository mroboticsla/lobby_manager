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
using System.Web.Services;

namespace WSEmpleados
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]


    public class Service1 : System.Web.Services.WebService
    {
        private static string sqlConnectionString = ConfigurationManager.ConnectionStrings["DBSalDev"].ConnectionString;
        private static string sqlConnectionStringSQL = ConfigurationManager.ConnectionStrings["DBSQL003"].ConnectionString;
        private static string sqlConnectionStringSQLD = ConfigurationManager.ConnectionStrings["DBSalDevSQL"].ConnectionString;

        [WebMethod]
        public DataTable
            getEmployeesActive()
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = "EMPLOYEES";
            string connString = sqlConnectionStringSQL;
            string query = "select '11184' as 'SALID','Luis' as 'EMP_FIRST_NAME', 'Ernesto' as EMP_SECOND_NAME, 'Mendoza' as EMP_FIRST_SURNAME,'Sanchez' as EMP_SECOND_SURNAME";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            return dataTable;
        }

        
    }
}