using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApi
{
    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string LocationDisplayed { get; set; }
        public string DateTime { get; set; }
        public int Temperature { get; set; }
        public string TemperatureDisplay { get; set; }
        public string Icon { get; set; }
        public string WeatherDescription { get; set; }
        public string WindSpeed { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }
    }
}
