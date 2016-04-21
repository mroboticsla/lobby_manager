using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeesWebService" in both code and config file together.
    /// <summary>
    /// Clase que permite la sincronización de empleados activos con un WebService de Terceros hacia una tabla local del sistema.
    /// </summary>
    [ServiceContract]
    public interface IEmployeesWebService
    {
        /// <summary>
        /// Sincroniza listado de empleados activos
        /// </summary>
        [OperationContract]
        void SyncEmployees();
    }
}
