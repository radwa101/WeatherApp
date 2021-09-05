using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherWcf.Interfaces.DataTransferObjects
{
    [DataContract]
    public class Dummy_Response
    {
        public Dummy_Response()
        {
        }

        [DataMember]
        public List<string> Names { get; set; }
    }
}