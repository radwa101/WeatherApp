using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApi.WeatherHandler
{
    public interface IWeatherReader
    {
        string Download(string url);
    }
}
