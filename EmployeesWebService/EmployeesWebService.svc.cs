using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EmployeesWebService
{
    public class EmployeesWebService : IEmployeesWebService
    {
        static String mainConnectionString = "SykesVisitorsDB";
        
        public DataTable getEmployeesActive()
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = "EMPLOYEES";
            string connString = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            string query = "'11184' as 'SALID','Luis' as 'EMP_FIRST_NAME', 'Ernesto' as EMP_SECOND_NAME, 'Mendoza' as EMP_FIRST_SURNAME,'Sanchez' as EMP_SECOND_SURNAME from tbl_emp_employees";

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
