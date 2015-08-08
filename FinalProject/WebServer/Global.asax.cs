using System.Web.Http;
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
