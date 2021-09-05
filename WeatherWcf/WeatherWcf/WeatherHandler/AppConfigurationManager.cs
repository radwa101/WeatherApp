using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WeatherWcf.WeatherHandler
{
    public class AppConfigurationManager : IConfigurationManager
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}