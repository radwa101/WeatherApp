using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WeatherWcf.Interfaces.DataTransferObjects;
using WeatherWcf.Models;

namespace WeatherWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DummyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DummyService.svc or DummyService.svc.cs at the Solution Explorer and start debugging.
    public class DummyService : IDummyService
    {
        public Dummy_Response GetData(Dummy_Request request)
        {
            return new Dummy_Response() { Names = new List<string>() { "One", "Two", "Three" } };
        }
    }
}
