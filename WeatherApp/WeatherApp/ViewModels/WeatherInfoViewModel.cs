using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Models;

namespace WeatherApp.ViewModels
{
    public class WeatherInfoViewModel
    {
        public string CityFilter { get; set; }
        public List<Location> Locations { get; set; }
    }
}