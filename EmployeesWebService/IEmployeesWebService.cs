using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EmployeesWebService
{
    [ServiceContract]
    public interface IEmployeesWebService
    {

        [OperationContract]
        DataTable getEmployeesActive();
    }
}