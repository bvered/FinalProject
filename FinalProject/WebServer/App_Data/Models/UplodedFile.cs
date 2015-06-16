using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebServer.App_Data.Models.Enums;

namespace WebServer.App_Data.Models
{
    public class UplodedFile : IPersistent
    {
        [DataMember]
        public Semester Semster { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public byte[] File { get; set; }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
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