using LobbyManager.ActiveEmployeesWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyManager
{
    /// <summary>
    /// Clase que permite la sincronización de empleados activos con un WebService de Terceros hacia una tabla local del sistema.
    /// </summary>
    public class EmployeesWebService : IEmployeesWebService
    {
        static String mainConnectionString = "SykesVisitorsDB";
        Service1SoapClient externalWS = new Service1SoapClient();

        /// <summary>
        /// Sincroniza listado de empleados activos
        /// </summary>
        public void SyncEmployees()
        {
            try
            {
                DataTable employees = externalWS.getEmployeesActive();

                if (employees.Rows.Count > 0)
                {
                    string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "DELETE FROM tbl_emp_employees";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        for (int i = 0; i < employees.Rows.Count; i++)
                        {
                            string id = employees.Rows[i].ItemArray[0].ToString();
                            string name = employees.Rows[i].ItemArray[1].ToString() + " " + employees.Rows[i].ItemArray[2].ToString();
                            string lastname = employees.Rows[i].ItemArray[3].ToString() + " " + employees.Rows[i].ItemArray[4].ToString();
                            cmd.CommandText = "INSERT INTO tbl_emp_employees (emp_id, emp_name, emp_lastname, emp_status) values (@emp_id, @emp_name, @emp_lastname, 1)";
                            cmd.Parameters.AddWithValue("emp_id", id);
                            cmd.Parameters.AddWithValue("emp_name", name);
                            cmd.Parameters.AddWithValue("emp_lastname", lastname);
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}
