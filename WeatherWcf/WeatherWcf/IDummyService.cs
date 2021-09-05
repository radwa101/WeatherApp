using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WeatherWcf.Interfaces.DataTransferObjects;

namespace WeatherWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDummyService" in both code and config file together.
    [ServiceContract]
    public interface IDummyService
    {
        [OperationContract]
        Dummy_Response GetData(Dummy_Request request);
    }
}
