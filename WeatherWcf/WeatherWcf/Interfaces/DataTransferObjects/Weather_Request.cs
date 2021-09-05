using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherWcf.Interfaces.DataTransferObjects
{
    [DataContract]
    public class Weather_Request
    {
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public int Celcius { get; set; }
        [DataMember]
        public int Fahrenheit { get; set; }
    }
}