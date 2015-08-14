using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.Http;
using NHibernate;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace WebServer.Controllers
{
    public class SmartSearchController : ApiController
    {
        [HttpPost]
        [ActionName("GetAnyResults")]
        public IHttpActionResult GetAny([FromBody] CourseSearchFilter filter)
        {
            using (var session = DBHelper.OpenSession())
            {
                filter.SearchPreferences = filter.SearchPreferences ?? new SearchPreferences();
                var teachersQuery = GetTeachersQuery(session, filter.SearchText);
                var coursesQuery = GetCoursesQuery(session, filter, filter.SearchPreferences);

                var lowestResult = filter.Counter * 5;

                var teachersCount = teachersQuery.Count();

                IQueryable<Course> courses = null;
                IQueryable<Teacher> teachers = null;

                if (teachersCount > lowestResult + 5)
                {
                    teachers = teachersQuery.Skip(lowestResult).Take(5);
                }
                else if (teachersCount < lowestResult)
                {
                    var coursesToSkip = lowestResult - teachersCount;
                    courses = coursesQuery.Skip(coursesToSkip).Take(5);
                }
                else
                {
                    var coursesToSkip = lowestResult - teachersCount;

                    var coursesCount = lowestResult + 5 - teachersCount;

                    teachers = teachersQuery.Skip(lowestResult);

                    courses = coursesQuery.Skip(coursesToSkip).Take(coursesCount);
                }

                var anySearchResult = new AnySearchResult
                {
                    CourseResults = courses == null ? new List<CourseResult>() : courses.ToList().Select(ConvertToResult).ToList(),
                    TeacherResults = teachers == null ? new List<ResultTeacher>() : teachers.ToList().Select(x => ConvertToResult(session, x)).ToList(),
                    TotalCount = teachersCount + coursesQuery.Count(),
                    SearchPreferences = filter.SearchPreferences
                };

                return Ok(anySearchResult);
            }
        }

        [HttpPost]
        [ActionName("GetAllSearchedTeachers")]
        public TeacherSearchResults GetAllSearchedTeachers([FromBody] SearchTeacher searchTeacher)
        {
            using (var session = DBHelper.OpenSession())
            {
                var query = GetTeachersQuery(session, searchTeacher.Name.ToLower());

                var totalCount = query.Count();
                if (searchTeacher.isTop)
                {
                    query = query.OrderByDescending(x => x.Score);
                }

                var teachers = query.Skip(searchTeacher.counter * 5).Take(5).ToList();

                var result = teachers.Select(teacher => ConvertToResult(session, teacher)).ToList();

                return new TeacherSearchResults
                {
                    TotalCount = totalCount,
                    Results = result
                };
            }
        }

        private static IQueryable<Teacher> GetTeachersQuery(ISession session, string lower)
        {
            return session.Query<Teacher>().Where(x => x.Name.ToLower().Contains(lower));
        }

        private static ResultTeacher ConvertToResult(ISession session, Teacher teacher)
        {
            var courses = session.Query<CourseInSemester>()
                .Where(x => x.Teacher.Id == teacher.Id)
                .Select(x => x.Course.Name).Distinct()
                .ToList();

            var resultTeacher = new ResultTeacher(teacher.Id, teacher.Name, courses, teacher.Score);
            return resultTeacher;
        }

        [HttpGet]
        [ActionName("GetAllNames")]
        public IList<string> GetAll([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var coursesNameList = session.Query<Course>().Where(x => x.University.Acronyms == id).Select(x => x.Name).ToList();
                var teachersNameList = session.Query<Teacher>().Where(x => x.University.Acronyms == id).Select(x => x.Name).ToList();

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

                filter.SearchPreferences = filter.SearchPreferences ?? new SearchPreferences();

                var query = GetCoursesQuery(session, filter, filter.SearchPreferences);

                List<Course> orderedCourses;
                if (filter.isTop)
                {
                    orderedCourses = query.OrderByDescending(x => x.Score).ToList();
                }
                else
                {
                    orderedCourses = query.ToList().OrderByDescending(x => GetUsageValue(x, filter.SearchPreferences)).ToList();
                }

                var total = query.Count();

                var courseResults = orderedCourses.Select(ConvertToResult).Skip(filter.Counter * 5).Take(5).ToList();

                var courseSearchResult = new CourseSearchResult
                {
                    AllResults = courseResults,
                    SearchPreferences = filter.SearchPreferences,
                    TotalCount = total,
                };

                return Ok(courseSearchResult);
            }
        }

        private static IQueryable<Course> GetCoursesQuery(ISession session, CourseSearchFilter filter,
            SearchPreferences searchPreferences)
        {
            var query = session.Query<Course>();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.SearchText.ToLower()));
            }

            int facultyValue;
            if (!string.IsNullOrWhiteSpace(filter.Faculty) &&
                int.TryParse(filter.Faculty, out facultyValue) &&
                Enum.IsDefined(typeof(Faculty), facultyValue))
            {
                var faculty = (Faculty)facultyValue;

                SetEmptyValue(searchPreferences.SearchedFaculties, faculty);
                searchPreferences.SearchedFaculties[faculty] += 1;
                query = query.Where(x => x.Faculty == faculty);
            }

            int intendedYearValue;
            if (!string.IsNullOrWhiteSpace(filter.IntendedYear) &&
                int.TryParse(filter.IntendedYear, out intendedYearValue) &&
                Enum.IsDefined(typeof(IntendedYear), intendedYearValue))
            {
                var intendedYear = (IntendedYear)intendedYearValue;

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
                Enum.IsDefined(typeof(AcademicDegree), academicDegreeValue))
            {
                var academicDegree = (AcademicDegree)academicDegreeValue;

                SetEmptyValue(searchPreferences.SearchedAcademicDegrees, academicDegree);
                searchPreferences.SearchedAcademicDegrees[academicDegree] += 1;
                query = query.Where(x => x.AcademicDegree == academicDegree);
            }
            return query;
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
            public bool isTop { get; set; }
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
            public bool isTop { get; set; }
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

        public class TeacherSearchResults
        {
            public IList<ResultTeacher> Results { get; set; }
            public int TotalCount { get; set; }
        }

        public class AnySearchResult
        {
            public IList<ResultTeacher> TeacherResults { get; set; }
            public IList<CourseResult> CourseResults { get; set; }
            public SearchPreferences SearchPreferences { get; set; }
            public int TotalCount { get; set; }
        }
    }
}