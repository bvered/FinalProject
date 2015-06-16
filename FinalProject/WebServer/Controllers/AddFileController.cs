using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace WebServer.Controllers
{
    public class AddFileController : ApiController
    {
        [HttpPost]
        [ActionName("AddSyllabus")]
        public async Task<IHttpActionResult> AddSyllabus()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                Guid courseId;
                Semester result;
                int year;

                var hasCourseId = Guid.TryParse(Request.Headers.GetValues("courseId").First(), out courseId);
                var hasSemster = Enum.TryParse(Request.Headers.GetValues("Semster").First(), out result);
                var hasYear = int.TryParse(Request.Headers.GetValues("Year").First(), out year);


                var syllabus = new Syllabus()
                {
                    File = buffer,
                    FileName = filename,
                    Semster = result,
                    Year = year
                };

                using (var session = DBHelper.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var course = session.Load<Course>(courseId);

                  //  course.AddSyllabus(syllabus);
                    session.Save(syllabus);
                    session.Save(course);

                    transaction.Commit();
                }
            }

            return Ok();
        }
    }

    public class CreateSyllabusFile
    {
        public Semester Semster { get; set; }
        public int Year { get; set; }
        public byte[] File { get; set; }
        public String FileName { get; set; }
    }
}
