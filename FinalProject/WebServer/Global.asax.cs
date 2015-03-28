using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebServer.App_Data;

namespace WebServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DBHelper.Load();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
