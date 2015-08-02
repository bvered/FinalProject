using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace WebServer.Controllers
{
    public class SmartSearchController : ApiController
    {
        [HttpPost]
        [ActionName("GetAllSearchedTeachers")]
        public AllResults GetAllSearchedTeachers([FromBody]SearchTeacher searchTeacher)
        {
            using (var session = DBHelper.OpenSession())
            {
                var teachers = session.Query<Teacher>().Where(x => x.Name.ToLower().Contains(searchTeacher.Name.ToLower()))
                    .Skip(searchTeacher.counter * 5).Take(5).ToList();
                var totalCount = session.Query<Teacher>().Count(x => x.Name.ToLower().Contains(searchTeacher.Name.ToLower()));

                IList<ResultTeacher> result = new List<ResultTeacher>();

                foreach (var teacher in teachers)
                {
                    IList<string> courses = session.Query<CourseInSemester>()
                        .Where(x => x.Teacher.Id == teacher.Id)
                        .Select(x => x.Course.Name).Distinct()
                        .ToList();

                    result.Add(new ResultTeacher(teacher.Id, teacher.Name, courses, teacher.Score));
                }

                return new AllResults
                {
                    TotalCount = totalCount,
                    Results = result
                };
            }
        }
        
        [HttpGet]
        [ActionName("GetAll")]
        public IList<string> GetAll()
        {
            using (var session = DBHelper.OpenSession())
            {
                var coursesNameList = session.QueryOver<Course>().Select(x => x.Name).List<string>();
                var teachersNameList = session.QueryOver<Teacher>().Select(x => x.Name).List<string>();

                var resultList = coursesNameList.ToList();

                resultList.AddRange(teachersNameList);

                return resultList;
            }
        }

        [HttpPost]
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

                int facultyValue;
                if (!string.IsNullOrWhiteSpace(filter.Faculty) && 
                    int.TryParse(filter.Faculty, out facultyValue) &&
                    Enum.IsDefined(typeof (Faculty), facultyValue))
                {
                    var faculty = (Faculty)facultyValue;

                    SetEmptyValue(searchPreferences.SearchedFaculties, faculty);
                    searchPreferences.SearchedFaculties[faculty] += 1;
                    query = query.Where(x => x.Faculty == faculty);
                }

                int intendedYearValue;
                if (!string.IsNullOrWhiteSpace(filter.IntendedYear) &&
                    int.TryParse(filter.IntendedYear, out intendedYearValue) &&
                    Enum.IsDefined(typeof (IntendedYear), intendedYearValue))
                {
                    var intendedYear = (IntendedYear) intendedYearValue;

                    SetEmptyValue(searchPreferences.SearchedIntendedYears, intendedYear);
                    searchPreferences.SearchedIntendedYears[intendedYear] += 1;
                    query = query.Where(x => x.IntendedYear == intendedYear);
                }

                bool isMandatoryValue;
                if (!string.IsNullOrWhiteSpace(filter.IsMandatory) &&
                    bool.TryParse(filter.IsMandatory, out isMandatoryValue))
                {
                    SetEmptyValue(searchPreferences.SearchedIsMandatory, isMandatoryValue);
                    searchPreferences.SearchedIsMandatory[isMandatoryValue] += 1;
                    query = query.Where(x => x.IsMandatory == isMandatoryValue);
                }

                int academicDegreeValue;
                if (!string.IsNullOrWhiteSpace(filter.AcademicDegree) &&
                    int.TryParse(filter.AcademicDegree, out academicDegreeValue) &&
                    Enum.IsDefined(typeof (AcademicDegree), academicDegreeValue))
                {
                    var academicDegree = (AcademicDegree) academicDegreeValue;

                    SetEmptyValue(searchPreferences.SearchedAcademicDegrees, academicDegree);
                    searchPreferences.SearchedAcademicDegrees[academicDegree] += 1;
                    query = query.Where(x => x.AcademicDegree == academicDegree);
                }

                var courseResults = query.ToList().OrderByDescending(x => GetUsageValue(x, searchPreferences)).Select(ConvertToResult).Skip(filter.Counter * 5).Take(5).ToList();
                var total = query.Count();


                return Ok(new CourseSearchResult
                {
                    AllResults = courseResults,
                    SearchPreferences = searchPreferences,
                    TotalCount = total,
                });

            }
        }

        private static void SetEmptyValue<T>(Dictionary<T, int> dictionary, T key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, 0);
            }
        }

        private int GetUsageValue(Course course, SearchPreferences searchPreferences)
        {
            int result = 0;

            if (searchPreferences.SearchedFaculties.ContainsKey(course.Faculty))
            {
                result += searchPreferences.SearchedFaculties[course.Faculty];
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
                Faculty = EnumTranslation.Faculties[arg.Faculty],
                AcademicDegree = EnumTranslation.AcademicDegrees[arg.AcademicDegree],
                Year = EnumTranslation.IntendedYears[arg.IntendedYear],
                IsMandatory = arg.IsMandatory,
                Score = arg.Score,
            };
        }

        public class SearchPreferences
        {
            public SearchPreferences()
            {
                SearchedFaculties = new Dictionary<Faculty, int>();
                SearchedAcademicDegrees = new Dictionary<AcademicDegree, int>();
                SearchedIntendedYears = new Dictionary<IntendedYear, int>();
                SearchedIsMandatory = new Dictionary<bool, int>();
            }

            public Dictionary<Faculty, int> SearchedFaculties { get; set; }
            public Dictionary<AcademicDegree, int> SearchedAcademicDegrees { get; set; }
            public Dictionary<IntendedYear, int> SearchedIntendedYears { get; set; }
            public Dictionary<bool, int> SearchedIsMandatory { get; set; }
        }

        public class CourseResult
        {
            public Guid Id { get; set; }
            public int CourseId { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
            public string Faculty { get; set; }
            public bool IsMandatory { get; set; }
            public string AcademicDegree { get; set; }
            public string Year { get; set; }
        }

        public class CourseSearchFilter
        {
            public string SearchText { get; set; }
            public string Faculty { get; set; }
            public string IsMandatory { get; set; }
            public string AcademicDegree { get; set; }
            public string IntendedYear { get; set; }

            public SearchPreferences SearchPreferences { get; set; }
            public int Counter { get; set; }
        }

        public class CourseSearchResult
        {
            public IList<CourseResult> AllResults { get; set; }
            public SearchPreferences SearchPreferences { get; set; }
            public int TotalCount { get; set; }
        }

        public class SearchTeacher
        {
            public string Name { get; set; }
            public int counter { get; set; }
        }

        public class ResultTeacher
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public IList<string> Courses { get; set; }
            public int Score { get; set; }

            public ResultTeacher(Guid id, string name, IList<string> courses, int score)
            {
                Id = id;
                Name = name;
                Courses = courses;
                Score = score;
            }
        }

        public class AllResults
        {
            public IList<ResultTeacher> Results { get; set; }
            public int TotalCount { get; set; }
        }
    }
}



