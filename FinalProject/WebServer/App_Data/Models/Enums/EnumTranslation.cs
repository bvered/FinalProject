using System.Collections.Generic;

namespace WebServer.App_Data.Models.Enums
{
    public static class EnumTranslation
    {
        static EnumTranslation()
        {
            IntendedYears = new Dictionary<IntendedYear, string>
            {
                {IntendedYear.Any, "הכל"},
                {IntendedYear.First, "שנה ראשונה"},
                {IntendedYear.Second, "שנה שניה"},
                {IntendedYear.Third, "שנה שלישית"},
                {IntendedYear.Forth, "שנה רביעית"},
            };

            AcademicDegrees = new Dictionary<AcademicDegree, string>
            {
                {AcademicDegree.Bachelor, "תואר ראשון"},
                {AcademicDegree.Bachelor, "תואר שני"}
            };

            Semesters = new Dictionary<Semester, string>
            {
                {Semester.A, "א"},
                {Semester.B, "ב"},
                {Semester.Summer, "קיץ"},
            };
        }

        public static Dictionary<IntendedYear, string> IntendedYears { get; private set; }
        public static Dictionary<AcademicDegree, string> AcademicDegrees { get; private set; }
        public static Dictionary<Semester, string> Semesters { get; private set; }
    }
}