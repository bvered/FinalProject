using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NHibernate.Dialect.Function;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

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

        [HttpGet]
        [ActionName("GetCourseByFilter")]
        public IHttpActionResult GetCourseByFilter([FromBody] CourseSearchFilter filter)
        {
            using (var session = DBHelper.OpenSession())
            {
                var query = session.Query<Course>();

                var searchPreferences = filter.SearchPreferences ?? new SearchPreferences();

                if (!string.IsNullOrWhiteSpace(filter.SearchText))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(filter.SearchText.ToLower()));
                }

                Guid facultyId;
                if (!string.IsNullOrWhiteSpace(filter.FacultyId) && Guid.TryParse(filter.FacultyId, out facultyId))
                {
                    searchPreferences.SearchedFaculties[facultyId] += 1;

                    query = query.Where(x => x.Faculty.Id == facultyId);
                }

                int intendedYearValue;
                if (!string.IsNullOrWhiteSpace(filter.IntendedYear) &&
                    int.TryParse(filter.IntendedYear, out intendedYearValue) &&
                    Enum.IsDefined(typeof (IntendedYear), intendedYearValue))
                {
                    var intendedYear = (IntendedYear) intendedYearValue;
                    searchPreferences.SearchedIntendedYears[intendedYear] += 1;
                    query = query.Where(x => x.IntendedYear == intendedYear);
                }

                bool isMandatoryValue;
                if (!string.IsNullOrWhiteSpace(filter.IsMandatory) &&
                    bool.TryParse(filter.IsMandatory, out isMandatoryValue))
                {
                    searchPreferences.SearchedIsMandatory[isMandatoryValue] += 1;
                    query = query.Where(x => x.IsMandatory == isMandatoryValue);
                }

                int academicDegreeValue;
                if (!string.IsNullOrWhiteSpace(filter.AcademicDegree) &&
                    int.TryParse(filter.AcademicDegree, out academicDegreeValue) &&
                    Enum.IsDefined(typeof (AcademicDegree), academicDegreeValue))
                {
                    var academicDegree = (AcademicDegree) academicDegreeValue;
                    searchPreferences.SearchedAcademicDegrees[academicDegree] += 1;
                    query = query.Where(x => x.AcademicDegree == academicDegree);
                }

                query = query.OrderByDescending(x => GetUsageValue(x, searchPreferences));

                return Ok(new CourseSearchResult
                {
                    AllResults = query.Select(ConvertToResult).ToList(),
                    Preferences = searchPreferences
                });
            }
        }

        private double GetUsageValue(Course course, SearchPreferences searchPreferences)
        {
            double result = 0;

            if (searchPreferences.SearchedFaculties.ContainsKey(course.Faculty.Id))
            {
                result += searchPreferences.SearchedFaculties[course.Faculty.Id];
            }

            if (searchPreferences.SearchedAcademicDegrees.ContainsKey(course.AcademicDegree))
            {
                result += searchPreferences.SearchedAcademicDegrees[course.AcademicDegree];
            }

            if (searchPreferences.SearchedIntendedYears.ContainsKey(course.IntendedYear))
            {
                result += searchPreferences.SearchedIntendedYears[course.IntendedYear];
            }

            if (searchPreferences.SearchedIsMandatory.ContainsKey(course.IsMandatory))
            {
                result += searchPreferences.SearchedIsMandatory[course.IsMandatory];
            }

            return result;
        }

        private CourseResult ConvertToResult(Course arg)
        {
            return new CourseResult
            {
                Id = arg.Id,
                Name = arg.Name,
                CourseId = arg.CourseId,
                FacultyName = arg.Faculty.Name,
                AcademicDegree = EnumTranslation.AcademicDegrees[arg.AcademicDegree],
                IntendedYear = EnumTranslation.IntendedYears[arg.IntendedYear],
                IsMandatory = arg.IsMandatory,
                Score = arg.Score,
            };
        }

        public class SearchPreferences
        {
            public Dictionary<Guid, double> SearchedFaculties { get; set; }
            public Dictionary<AcademicDegree, double> SearchedAcademicDegrees { get; set; }
            public Dictionary<IntendedYear, double> SearchedIntendedYears { get; set; }
            public Dictionary<bool, double> SearchedIsMandatory { get; set; }
        }

        public class CourseResult
        {
            public Guid Id { get; set; }
            public int CourseId { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
            public string FacultyName { get; set; }
            public bool IsMandatory { get; set; }
            public string AcademicDegree { get; set; }
            public string IntendedYear { get; set; }
        }

        public class CourseSearchFilter
        {
            public string SearchText { get; set; }
            public string FacultyId { get; set; }
            public string IsMandatory { get; set; }
            public string AcademicDegree { get; set; }
            public string IntendedYear { get; set; }

            public SearchPreferences SearchPreferences { get; set; }
        }

        public class CourseSearchResult
        {
            public IList<CourseResult> AllResults { get; set; }
            public SearchPreferences Preferences { get; set; }
        }
    }
}



