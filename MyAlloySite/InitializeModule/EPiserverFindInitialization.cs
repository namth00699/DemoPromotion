using EPiServer.Find;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using MyAlloySite.Find;

namespace MyAlloySite.InitializeModule
{
    //[InitializableModule]
    //public class EPiserverFindInitialization : IInitializableModule
    //{
    //    public void Initialize(InitializationEngine context)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public void Uninitialize(InitializationEngine context)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    [InitializableModule]
    public class FindConfigModule : IInitializableModule
    {
        private readonly Injected<EPiServer.Find.IClient> client;
        private static ContentClientConventions _catalogContentClientConventions ;

        public void Initialize(InitializationEngine context)
        {
            _catalogContentClientConventions = context.Locate.Advanced.GetInstance<ContentClientConventions>();
            _catalogContentClientConventions.ApplyConventions(SearchClient.Instance.Conventions);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<CatalogContentClientConventions , ContentClientConventions>();
        }
    }
}