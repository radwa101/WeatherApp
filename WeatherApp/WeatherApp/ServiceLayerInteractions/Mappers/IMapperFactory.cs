using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.ServiceLayerInteractions.Mappers
{
    public interface IMapperFactory
    {
        IMapper<TViewModel, TRequest, TResponse> CreateMapper<TViewModel, TRequest, TResponse>(string methodName)
                    where TRequest : class
                    where TResponse : class;

        IMapper<TViewModel, TRequest, TResponse> CreateMapper<TViewModel, TRequest, TResponse>()
                    where TRequest : class
                    where TResponse : class;
        
    }
}
