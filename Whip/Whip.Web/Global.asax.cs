﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Whip.Services.Interfaces;

namespace Whip.Web
{
    public class MvcApplication : HttpApplication
    {
        private readonly Lazy<IErrorLoggingService> _errorLoggingService = new Lazy<IErrorLoggingService>(
            () => DependencyResolver.Current.GetService<IErrorLoggingService>());

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            _errorLoggingService.Value.Log(exception);
            Response.Clear();
            Server.ClearError();
            Response.Redirect("~/Error/");
        }
    }
}
