using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WeatherService.dto;

namespace WeatherService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WeatherService" in both code and config file together.
    public class WeatherService : IWeatherService
    {
        public double ConvertCelciusToFahrenheit(double? celcius)
        {
            if (celcius != null) return celcius.Value * 9 / 5 + 32;

            return 0;
        }


        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public string GetResults()
        {
            List<Person> results = new List<Person>();
            results.Add(new Person("Peyton", "Manning", 35));
            results.Add(new Person("Drew", "Brees", 31));
            results.Add(new Person("Tony", "Romo", 29));

            // Serialize the results as JSON
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(results.GetType());
            MemoryStream memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, results);

            // Return the results serialized as JSON
            string json = Encoding.Default.GetString(memoryStream.ToArray());
            return json;
        }
    }
}
