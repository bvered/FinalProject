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
                {AcademicDegree.Master, "תואר שני"}
            };

            Semesters = new Dictionary<Semester, string>
            {
                {Semester.A, "א"},
                {Semester.B, "ב"},
                {Semester.Summer, "קיץ"},
            };

            Faculties = new Dictionary<Faculty, string>
            {
                {Faculty.None, "ללא"},
                {Faculty.ComputerScience, "מדעי המחשב"},
                {Faculty.BehavioralSciences, "מדעי ההתנהגות"},
                {Faculty.InformationSystems, "מערכות מידע"},
                {Faculty.Nursing, "סיעוד"},
                {Faculty.ManagementEconomics, "ניהול וכלכלה"},
                {Faculty.SocietyPolitics, "חברה ופוליטיקה"},
                {Faculty.ConsultingOrganizationalDevelopment, "ייעוץ ופיתוח ארגוני"},
                {Faculty.BusinessAdministration, "מנהל עסקים"},
                {Faculty.Psychology, "פסיכולוגיה"},
            };
        }

        public static Dictionary<IntendedYear, string> IntendedYears { get; private set; }
        public static Dictionary<AcademicDegree, string> AcademicDegrees { get; private set; }
        public static Dictionary<Semester, string> Semesters { get; private set; }
        public static Dictionary<Faculty, string> Faculties { get; private set; }
    }
}