﻿using HNBAUserIdentityAPI.Controllers.User;
using HNBAUserIdentityAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HNBAUserIdentityAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //To return the result as JSON 
            config.Formatters.JsonFormatter.
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));


            //To register the authentication
            GlobalConfiguration.Configuration.Filters.Add(new TDABasicAuthenticationFilter());


            //To register model validation filter
            config.Filters.Add(new ValidateModelAttribute());
        }
    }
}
