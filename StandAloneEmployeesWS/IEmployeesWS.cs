using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace StandAloneEmployeesWS
{
    [ServiceContract]
    public interface IEmployeesWS
    {

        [OperationContract]
        string DeleteCurrentData();

        [OperationContract]
        string CountCurrentData();

        [OperationContract]
        string InsertDataSet(DataTable data);

        [OperationContract]
        string InsertDataRow(string id, string name, string lastname);
    }
}
