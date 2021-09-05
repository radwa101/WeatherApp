using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApi.WeatherHandler
{
    public interface IScraper
    {
        List<Location> GetCities(string htmlContent);
        List<Location> GetExtremes(string htmlContent, string filter, bool isTemp);
    }
}
