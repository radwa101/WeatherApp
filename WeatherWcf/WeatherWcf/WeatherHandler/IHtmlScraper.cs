using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWcf.Models;

namespace WeatherWcf.WeatherHandler
{
    public interface IScraper
    {
        List<Location> GetCities(string htmlContent);
        string GetForecastData(string htmlContent, string filter);
    }
}
