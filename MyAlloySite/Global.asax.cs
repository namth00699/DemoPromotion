using Autofac;
using Autofac.Integration.Mvc;
using EPiServer.Find;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyAlloySite
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            var builder = new ContainerBuilder();
            builder.Register(x => CreateSearchClient()).As<IClient>().SingleInstance();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configure(WebApiRouteConfig.Register);
            GlobalConfiguration.Configuration.EnsureInitialized();
            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }
        private static IClient CreateSearchClient()
        {
            var client = Client.CreateFromConfig();
            //Any modifications required goes here    
            return client;
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = true;
        }
    }

    public class WebApiRouteConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            //Attribute routing.
            //configuration.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            configuration.Routes.MapHttpRoute(
            name: "ContentListing",
            routeTemplate: "api/content/{action}",
            defaults: new { Controller = "ProductApi" }
            );

            //configuration.Routes.MapHttpRoute(
            //name: "ContentListing",
            //routeTemplate: "api/content",
            //defaults: new { controller = "ProductApi" }
            // );

            //This is the default api route
            configuration.Routes.MapHttpRoute(
                name: "DefaultWebApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            configuration.EnsureInitialized();
        }
    }
}