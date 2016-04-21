using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncLobbyManagerEmployees.EmployeesWS;

namespace SyncLobbyManagerEmployees
{
    /// <summary>
    /// Cliente de sincronización de empleados para LobbyManager
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            EmployeesWebServiceClient ws = new EmployeesWebServiceClient();
            ws.SyncEmployees();
        }
    }
}