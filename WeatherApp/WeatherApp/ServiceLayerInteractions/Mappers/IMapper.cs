using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.ServiceLayerInteractions.Mappers
{
    public interface IMapper<TViewModel, TRequest, TResponse>
    {
        TRequest Map(TViewModel viewModel);
        void Map(TResponse response, TViewModel viewModel);
    }
}
