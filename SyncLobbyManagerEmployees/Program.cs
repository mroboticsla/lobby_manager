using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncLobbyManagerEmployees.SykesWS;
using SyncLobbyManagerEmployees.LobbyManagerWS;
using System.Data;

namespace SyncLobbyManagerEmployees
{
    /// <summary>
    /// Cliente de sincronización de empleados para LobbyManager
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Conectando a WS...");
                Service1SoapClient externalWS = new Service1SoapClient();
                EmployeesWSClient LM_Employees = new EmployeesWSClient();

                Console.WriteLine("Consultando base de datos...");
                DataTable employees = externalWS.getEmployeesActive();
                Console.WriteLine("Consulta realizada con exito!");
                if (employees.Rows.Count > 0)
                {
                    Console.WriteLine("Se encontraron: " + employees.Rows.Count + " Empleados activos.");
                    Console.WriteLine("");
                    Console.WriteLine("******************************************************************");
                    for (int i = 0; i < employees.Rows.Count; i++)
                    {
                        string id = employees.Rows[i].ItemArray[0].ToString();
                        string name = employees.Rows[i].ItemArray[1].ToString() + " " + employees.Rows[i].ItemArray[2].ToString();
                        string lastname = employees.Rows[i].ItemArray[3].ToString() + " " + employees.Rows[i].ItemArray[4].ToString();

                        if (i < 3)
                        {
                            Console.WriteLine("Datos de ejemplo:");
                            Console.WriteLine("ID: " + id + " | NAME: " + name + " | LASTNAME: " + lastname);
                        }
                    }
                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("");
                    Console.WriteLine("Sincronizando base actual... " + LM_Employees.DeleteCurrentData());
                    Console.WriteLine("Guardando en la base de datos " + employees.Rows.Count + " registros...");
                    Console.WriteLine(LM_Employees.InsertDataSet(employees));
                }
                else
                {
                    Console.WriteLine("La consulta no generó resultados..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error: ");
                Console.WriteLine("Fuente: " + ex.Source);
                Console.WriteLine("Mensaje: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                Console.WriteLine("");
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }
    }
}