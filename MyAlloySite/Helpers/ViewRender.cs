using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Electrolux.Core.Helpers.MVCHelper
{
    public class ViewRenderer
    {
        protected ControllerContext Context
        {
            get;
            set;
        }
        public ViewRenderer(ControllerContext controllerContext = null)
        {
            if (controllerContext == null)
            {
                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("ViewRenderer must run in the context of an ASP.NET Application and requires HttpContext.Current to be present.");
                }
                controllerContext = CreateController<EmptyController>().ControllerContext;
            }
            Context = controllerContext;
        }
        private string RenderViewToStringInternal(string viewPath, object model, bool partial = false)
        {
            ViewEngineResult viewEngineResult;
            if (partial)
            {
                viewEngineResult = ViewEngines.Engines.FindPartialView(Context, viewPath);
            }
            else
            {
                viewEngineResult = ViewEngines.Engines.FindView(Context, viewPath, null);
            }
            if (viewEngineResult == null || viewEngineResult.View == null)
            {
                throw new FileNotFoundException("View could not be found.");
            }
            IView view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;
            string result = null;
            using (StringWriter stringWriter = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(Context, view, Context.Controller.ViewData, Context.Controller.TempData, stringWriter);
                view.Render(viewContext, stringWriter);
                result = stringWriter.ToString();
            }
            return result;
        }
        public string RenderPartialViewToString(string viewPath, object model = null)
        {
            return RenderViewToStringInternal(viewPath, model, true);
        }

        public T CreateController<T>(RouteData routeData = null, params object[] parameters) where T : Controller, new()
        {
            T t = (T)((object)Activator.CreateInstance(typeof(T), parameters));
            if (HttpContext.Current != null)
            {
                HttpContextBase httpContextBase = new HttpContextWrapper(HttpContext.Current);
                if (routeData == null)
                {
                    routeData = new RouteData();
                }
                if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                {
                    routeData.Values.Add("controller", t.GetType().Name.ToLower().Replace("controller", ""));
                }
                t.ControllerContext = new ControllerContext(httpContextBase, routeData, t);
                return t;
            }
            throw new InvalidOperationException("Can't create Controller Context if no active HttpContext instance is available.");
        }
    }
}
