using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class SmartSearchController : ApiController
    {
        [HttpGet]
        [ActionName("GetAll")]
        public IList<string> GetAll()
        {
            using (var session = DBHelper.OpenSession())
            {
                IList<string> coursesNameList = session.QueryOver<Course>().Select(x => x.Name).List<string>();
                IList<string> teachersNameList = session.QueryOver<Teacher>().Select(x => x.Name).List<string>();
                IList<string> resultList = new List<string>();
                
                foreach (var name in coursesNameList)
                {
                    resultList.Add(name);
                }
                foreach (var name in teachersNameList)
                {
                    resultList.Add(name);
                }
                
                return resultList;
            }
        }
    }
}
