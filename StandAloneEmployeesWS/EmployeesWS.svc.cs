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

namespace StandAloneEmployeesWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EmployeesWS : IEmployeesWS
    {
        static String mainConnectionString = "SykesVisitorsDB";
        
        public string DeleteCurrentData()
        {
            String result = "PROCESO INCOMPLETO";

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "DELETE FROM tbl_emp_employees";
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    result = "OK";
                }
            }
            catch (Exception ex)
            {
                result = "ERROR: " + ex.Message;
            }
            
            return result;
        }
        
        public String InsertDataSet(DataTable data)
        {
            String result = "PROCESO INCOMPLETO";

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string id = data.Rows[i].ItemArray[0].ToString();
                        string name = data.Rows[i].ItemArray[1].ToString() + " " + data.Rows[i].ItemArray[2].ToString();
                        string lastname = data.Rows[i].ItemArray[3].ToString() + " " + data.Rows[i].ItemArray[4].ToString();
                        cmd.CommandText = "INSERT INTO tbl_emp_employees (emp_id, emp_name, emp_lastname, emp_status) values (@emp_id, @emp_name, @emp_lastname, 1)";
                        cmd.Parameters.AddWithValue("emp_id", id);
                        cmd.Parameters.AddWithValue("emp_name", name);
                        cmd.Parameters.AddWithValue("emp_lastname", lastname);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();

                    result = "OK";
                }
            }
            catch (Exception ex)
            {
                result = "ERROR: " + ex.Message;
            }

            return result;
        }
        
        public String InsertDataRow(string id, string name, string lastname)
        {
            String result = "PROCESO INCOMPLETO";

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO tbl_emp_employees (emp_id, emp_name, emp_lastname, emp_status) values (@emp_id, @emp_name, @emp_lastname, 1)";
                    cmd.Parameters.AddWithValue("emp_id", id);
                    cmd.Parameters.AddWithValue("emp_name", name);
                    cmd.Parameters.AddWithValue("emp_lastname", lastname);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    result = "OK";
                }
            }
            catch (Exception ex)
            {
                result = "ERROR: " + ex.Message;
            }

            return result;
        }
    }
}
