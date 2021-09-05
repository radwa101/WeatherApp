using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WeatherWcf.Models;

namespace WeatherWcf.Interfaces.DataTransferObjects
{
    [DataContract]
    public class Weather_Response
    {
        public Weather_Response()
        {
        }

        [DataMember]
        public List<Location> locations { get; set; }
    }
}