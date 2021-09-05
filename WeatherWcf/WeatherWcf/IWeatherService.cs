using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WeatherWcf.Interfaces.DataTransferObjects;
using WeatherWcf.Models;

namespace WeatherWcf
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Location> GetWorldTemperatures();
        
        [OperationContract]
        Weather_Response GetWorldTemperaturesSoap(Weather_Request request);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Location> GetEuropeanTemperatures();

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string GetForecastData(string filter, string filterTable);

        [OperationContract]
        Weather_Response WeatherInfo(Weather_Request request);
    }
}
