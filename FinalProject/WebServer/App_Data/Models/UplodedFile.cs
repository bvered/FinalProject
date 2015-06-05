using System;

namespace WebServer.App_Data.Models
{
    public class UplodedFile : IPersistent
    {
        public Semester Semster { get; set; }
        public int Year { get; set; }
        public byte[] File { get; set; }
        public String FileName { get; set; }
        public Guid Id { get; set; }
        
    }

    public class Syllabus : UplodedFile
    {
         
    }

    public class GradesDestribution : UplodedFile
    {
        public int AverageGrade { get; set; }
    }
}