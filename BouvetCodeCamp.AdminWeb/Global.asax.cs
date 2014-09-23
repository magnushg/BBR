﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BouvetCodeCamp.AdminWeb
{
    using BouvetCodeCamp.AdminWeb.App_Start;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IoCConfig.RegisterDependencies();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}