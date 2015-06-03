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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using WebServer.App_Data;
//using WebServer.App_Data.Models;

//namespace WebServer.Controllers
//{
//    public class SmartSearchController : ApiController
//    {
//        [HttpGet]
//        [ActionName("GetAll")]
//        public IList<SearchResult> GetAll()
//        {
//            using (var session = DBHelper.OpenSession())
//            {
//                var coursesNameList = session.QueryOver<Course>().List();
//                var teachersNameList = session.QueryOver<Teacher>().List();

//                IList<SearchResult> resultList = new List<SearchResult>();

//                foreach (var course in coursesNameList)
//                {
//                    resultList.Add(new SearchResult
//                    {
//                        Id = course.Id,
//                        Name = course.Name,
//                        Type = "course"
//                    });
//                }
//                foreach (var teacher in teachersNameList)
//                {
//                    resultList.Add(new SearchResult
//                    {
//                        Id = teacher.Id,
//                        Name = teacher.Name,
//                        Type = "teacher"
//                    });
//                }

//                return resultList;
//            }
//        }
//    }

//    public class SearchResult
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Type { get; set; }
//    }
//}

