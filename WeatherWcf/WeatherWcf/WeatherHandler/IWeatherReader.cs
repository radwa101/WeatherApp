using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWcf.WeatherHandler
{
    public interface IWeatherReader
    {
        string Download(string file);
    }
}
