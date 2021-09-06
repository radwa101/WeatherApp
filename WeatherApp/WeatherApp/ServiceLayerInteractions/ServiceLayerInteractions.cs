using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WeatherApp.ServiceLayerInteractions.Mappers;
using WeatherApp.ViewModels;
using WeatherApp.WeatherService;
using WeatherApp.DummyService;
using WeatherApp.ServiceLayerInteractions;

namespace WeatherApp.Infrastructure
{
    public class ServiceLayerInteractions : ServiceLayerInteractionsBase
    {
        public WeatherInfoViewModel GetWorldTemperatures(WeatherInfoViewModel viewModel)
        {
            var weatherInfoViewModel = new WeatherInfoViewModel();
            var request = new Weather_Request() { City = viewModel.CityFilter };
            var worldTemperaturesResponse = GetResponseFromServiceCall<Weather_Request, Weather_Response, WeatherServiceClient>(request, (c, req) => c.GetWorldTemperaturesSoap(req));

            var mapper = new WeatherInfoMapper();
            mapper.Map(worldTemperaturesResponse, weatherInfoViewModel);

            //var dummyRequest = new Dummy_Request();
            //var dummyResponse = GetResponseFromServiceCall<Dummy_Request, Dummy_Response, DummyServiceClient>(dummyRequest, (c, req) => c.GetData(dummyRequest));

            return weatherInfoViewModel;
        }


        public void GetWorldTemperaturesWithMapper(WeatherInfoViewModel viewModel)
        {
            Weather_Request request = new Weather_Request() { City = viewModel.CityFilter };
            CallService<WeatherInfoViewModel, Weather_Request, Weather_Response, WeatherServiceClient>(viewModel, (c, req) => c.WeatherInfo(req));
        }
    }
}