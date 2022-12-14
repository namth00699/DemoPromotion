using System.Web.Optimization;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace MyAlloySite.Business.Initialization
{
    [InitializableModule]
    public class BundleConfig : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if (context.HostType == HostType.WebApplication)
            {
                RegisterBundles(BundleTable.Bundles);
            }
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Static/js/jquery.js", //jquery.js can be removed and linked from CDN instead, we use a local one for demo purposes without internet connectionzz
                        "~/Static/js/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Static/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/bootstrap-responsive.css")
                .Include("~/Static/css/media.css")
                .Include("~/Static/css/style.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/editmode.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Static/js/jquery.js",
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/promotion").Include(
                   "~/Static/js/Promotion/promotion1.js"
                  ));
            bundles.Add(new StyleBundle("~/Content/ribbon").Include(
                   "~/Static/css/ribbon.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/site.css"));
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
