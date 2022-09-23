using EPiServer.Find;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using EPiServer.Logging;
using MyAlloySite.Commerce.Products;
using System;

namespace MyAlloySite.Find
{
    public class ContentClientConventions : CatalogContentClientConventions
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(ContentClientConventions));
        public override void ApplyConventions(IClientConventions clientConventions)
        {
            try
            {
                SearchClient.Instance.Conventions.ForInstancesOf<CommonProducts>().ApplyFieldConventions();
                base.ApplyConventions(clientConventions);
            }
            catch(Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}