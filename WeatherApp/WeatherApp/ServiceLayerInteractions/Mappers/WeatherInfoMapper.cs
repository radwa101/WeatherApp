using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.ViewModels;
using WeatherApp.WeatherService;

namespace WeatherApp.ServiceLayerInteractions.Mappers
{
    public class WeatherInfoMapper : IMapper<WeatherInfoViewModel, Weather_Request, Weather_Response>
    {
        public WeatherInfoMapper()
        {
        }

        public Weather_Request Map(WeatherInfoViewModel viewModel)
        {
            Weather_Request request = new Weather_Request();
            request.City = viewModel.CityFilter;
            return request;
        }

        public void Map(Weather_Response response, WeatherInfoViewModel viewModel)
        {
            List<Models.Location> locations = new List<Models.Location>();
            foreach (var location in response.locations)
            {
                locations.Add(new Models.Location()
                {
                    City = location.City,
                    Country = location.Country,
                    DateTime = location.DateTime,
                    ForecastDetailsUrl = location.ForecastDetailsUrl,
                    Icon = location.Icon,
                    LocationDisplayed = location.LocationDisplayed,
                    Temperature = location.Temperature,
                    TemperatureDisplay = location.TemperatureDisplay
                });
            }
            viewModel.Locations = locations;
        }
    }
}