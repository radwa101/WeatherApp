using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WeatherWcf.WeatherHandler
{
    public class WeatherReader : IWeatherReader
    {
        private IConfigurationManager _configurationManager;

        public WeatherReader(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string Download(Uri url)
        {
            return new WebClient().DownloadString(url);
        }
    }
}