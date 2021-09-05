using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherWcf.Interfaces.DataTransferObjects
{
    [DataContract]
    public class Dummy_Request
    {
        public Dummy_Request()
        {
        }

        [DataMember]
        public string Name { get; set; }
    }
}