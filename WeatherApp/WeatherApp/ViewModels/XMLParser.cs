using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.ViewModels
{
    public class XMLParser
    {
        public Dictionary<string, List<string>> NestedParameters
        {
            get;
            set;
        }

        public dynamic JsonData
        {
            get;
            set;
        }
    }
}