using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WeatherApp.ServiceLayerInteractions.Mappers;

namespace WeatherApp.ServiceLayerInteractions
{
    public class ServiceLayerInteractionsBase
    {
        protected TResponse GetResponseFromServiceCall<TRequest, TResponse, TClient>(TRequest request,
                                        Expression<Func<TClient, TRequest, TResponse>> methodFunc)
                            where TRequest : class
                            where TResponse : class
                            where TClient : IDisposable
        {
            TResponse response = null;
            ServiceOperationInfo info = GetServiceOperationInfoFromMethodFuncWithRequestAndResponse(methodFunc);
            using (var c = Create<TClient>())
            {
                Func<TClient, TRequest, TResponse> theFunc = methodFunc.Compile();
                response = theFunc(c, request);
            }
            return response;
        }

        private TClient Create<TClient>()
        {
            return Activator.CreateInstance<TClient>();
        }

        protected ServiceOperationInfo GetServiceOperationInfoFromMethodFuncWithRequestAndResponse<TRequest, TResponse, TClient>(
                                        Expression<Func<TClient, TRequest, TResponse>> methodFunc)
                            where TRequest : class
                            where TResponse : class
                            where TClient : IDisposable
        {
            var info = (MethodCallExpression)methodFunc.Body;
            return BuildServiceOperationInfoObject(info);
        }

        protected virtual void CallService<TViewModel, TRequest, TResponse, TClient>(TViewModel model, Expression<Func<TClient, TRequest, TResponse>> methodFunc)
                            where TRequest : class
                            where TResponse : class
                            where TClient : IDisposable
        {
            ServiceOperationInfo operationInfo = GetServiceOperationInfoFromMethodFuncWithRequestAndResponse(methodFunc);
            MapperFactory mapperFactory = new MapperFactory();
            var mapper = mapperFactory.CreateMapper<TViewModel, TRequest, TResponse>(operationInfo.MethodName);
            TRequest request = mapper.Map(model);
            TResponse response = GetResponseFromServiceCall(request, methodFunc);
            mapper.Map(response, model);
        }

        protected ServiceOperationInfo BuildServiceOperationInfoObject(MethodCallExpression info)
        {
            var methodInfo = new ServiceOperationInfo();
            methodInfo.MethodName = info.Method.Name;
            methodInfo.RequestType = info.Method.GetParameters()[0].ParameterType;
            methodInfo.ResponseType = info.Method.ReturnParameter.ParameterType;
            methodInfo.ClassName = info.Object.Type.FullName;
            return methodInfo;
        }

        protected class ServiceOperationInfo
        {
            public string MethodName { get; set; }
            public Type RequestType { get; set; }
            public Type ResponseType { get; set; }
            public string ClassName { get; set; }
        }
    }
}