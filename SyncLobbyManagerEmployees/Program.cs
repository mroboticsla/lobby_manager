using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncLobbyManagerEmployees.EmployeesWS;

namespace SyncLobbyManagerEmployees
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeesWebServiceClient ws = new EmployeesWebServiceClient();
            ws.SyncEmployees();
        }
    }
}
