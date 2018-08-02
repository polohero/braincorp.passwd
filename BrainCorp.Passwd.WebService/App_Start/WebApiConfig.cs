using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using BrainCorp.Passwd.Common.Exceptions;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Replace(typeof(IExceptionHandler), new WebApiExceptionHandler());
            config.Services.Add(
                typeof(IExceptionLogger),
               new WebApiExceptionLogger(new NullLogger()));


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Replace(typeof(ModelMetadataProvider), new EmptyStringAllowedModelMetadataProvider());
        }
    }

    public class EmptyStringAllowedModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override CachedDataAnnotationsModelMetadata CreateMetadataFromPrototype(CachedDataAnnotationsModelMetadata prototype, Func<object> modelAccessor)
        {
            var metadata = base.CreateMetadataFromPrototype(prototype, modelAccessor);
            metadata.ConvertEmptyStringToNull = false;
            return metadata;
        }

        protected override CachedDataAnnotationsModelMetadata CreateMetadataPrototype(IEnumerable<Attribute> attributes, Type containerType, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadataPrototype(attributes, containerType, modelType, propertyName);
            metadata.ConvertEmptyStringToNull = false;
            return metadata;
        }
    }
}
