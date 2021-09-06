using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WeatherWcf.Interfaces.DataTransferObjects;
using WeatherWcf.Models;
using WeatherWcf.WeatherHandler;

namespace WeatherWcf
{
    public class WeatherService : IWeatherService
    {
        private static readonly IConfigurationManager configManager = new AppConfigurationManager();
        private readonly IWeatherReader weatherReader = new WeatherReader(configManager);
        private readonly IScraper htmlScraper = new HtmlScraper();

        public List<Location> GetWorldTemperatures()
        {
            string htmlContents = weatherReader.Download(configManager.GetAppSetting("WorldWeatherUrl") + "weather/");
            return htmlScraper.GetCities(htmlContents);
        }

        public Weather_Response GetWorldTemperaturesSoap(Weather_Request request)
        {
            Weather_Response response = new Weather_Response();
            string htmlContents = weatherReader.Download(configManager.GetAppSetting("WorldWeatherUrl") + "weather/");
            response.locations = htmlScraper.GetCities(htmlContents);
            request.City = request.City ?? string.Empty;
            response.locations = response.locations.Where(l => l.City.Contains(request.City)).ToList();
            return response;
        }

        public Weather_Response WeatherInfo(Weather_Request request)
        {
            Weather_Response response = new Weather_Response();
            string htmlContents = weatherReader.Download(configManager.GetAppSetting("WorldWeatherUrl") + "weather/");
            response.locations = htmlScraper.GetCities(htmlContents);
            request.City = request.City ?? string.Empty;
            response.locations = response.locations.Where(l => l.City.Contains(request.City)).ToList();
            return response;
        }

        public List<Location> GetEuropeanTemperatures()
        {
            string htmlContents = weatherReader.Download(configManager.GetAppSetting("WorldWeatherUrl") + "weather/?continent=europe");
            return htmlScraper.GetCities(htmlContents);
        }

        public string GetForecastData(string filter, string filterTable)
        {
            string htmlContents = weatherReader.Download(configManager.GetAppSetting("WorldWeatherUrl") + filter);
            return htmlScraper.GetForecastData(htmlContents, filterTable);
        }
    }
}