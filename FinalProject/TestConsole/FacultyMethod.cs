using System;
using WebServer.App_Data.Models.Enums;

namespace TestConsole
{
    public class FacultyMethod
    {
        public static Faculty FacultyFromString(string facultyName)
        {
            if (string.Equals(facultyName, "מדעי המחשב", StringComparison.CurrentCulture))
            {
                return Faculty.ComputerScience;
            }
            if (string.Equals(facultyName, "מדעי התנהגות", StringComparison.CurrentCulture))
            {
                return Faculty.BehavioralSciences;
            }
            if (string.Equals(facultyName, "ניהול מערכות מידע", StringComparison.CurrentCulture))
            {
                return Faculty.InformationSystems;
            }
            if (string.Equals(facultyName, "מדעי הסיעוד", StringComparison.CurrentCulture))
            {
                return Faculty.Nursing;
            }
            if (string.Equals(facultyName, "כלכלה וניהול", StringComparison.CurrentCulture))
            {
                return Faculty.ManagementEconomics;
            }
            if (string.Equals(facultyName, "ממשל וחברה", StringComparison.CurrentCulture))
            {
                return Faculty.SocietyPolitics;
            }
            if (string.Equals(facultyName, "יעוץ ופיתוח ארגוני", StringComparison.CurrentCulture))
            {
                return Faculty.ConsultingOrganizationalDevelopment;
            }
            if (string.Equals(facultyName, "מנהל עסקים", StringComparison.CurrentCulture))
            {
                return Faculty.BusinessAdministration;
            }
            if (string.Equals(facultyName, "פסיכולוגיה", StringComparison.CurrentCulture))
            {
                return Faculty.Psychology;
            }
            return Faculty.None;
        }
    }
}