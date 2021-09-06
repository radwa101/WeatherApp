using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WeatherApp.ServiceLayerInteractions.Mappers
{
    public class MapperFactory : IMapperFactory
    {
        public IMapper<TViewModel, TRequest, TResponse> CreateMapper<TViewModel, TRequest, TResponse>(string methodName)
                    where TRequest : class
                    where TResponse : class
        {
            Type[] typeList = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "WeatherApp.ServiceLayerInteractions.Mappers");
            var type = typeList.SingleOrDefault(c => c.Name == methodName + "Mapper");
            if (type == null)
            {
                throw new Exception(methodName + "Mapper not found by MapperFactory");
            }
            return CreateMapper<TViewModel, TRequest, TResponse>(type);
        }

        private IMapper<TViewModel, TRequest, TResponse> CreateMapper<TViewModel, TRequest, TResponse>(Type type)
        {
            IMapper<TViewModel, TRequest, TResponse> mapper;
            var instance = Activator.CreateInstance(type);
            mapper = (IMapper<TViewModel, TRequest, TResponse>)instance;
            return mapper;
        }

        IMapper<TViewModel, TRequest, TResponse> IMapperFactory.CreateMapper<TViewModel, TRequest, TResponse>()
        {
            Type[] typeList = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "WeatherApp.ServiceLayerInteractions.Mappers");
            for (int i = 0; i < typeList.Length; i++)
            {
                if (typeof(IMapper<TViewModel, TRequest, TResponse>).IsAssignableFrom(typeList[i]))
                {
                    return CreateMapper<TViewModel, TRequest, TResponse>(typeList[i]);
                }
            }
            TRequest req = Activator.CreateInstance<TRequest>();
            TResponse res = Activator.CreateInstance<TResponse>();
            TViewModel vm = Activator.CreateInstance<TViewModel>();

            throw new Exception("mapper not found by MapperFactory for request type: " + req.GetType());
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
    }
}