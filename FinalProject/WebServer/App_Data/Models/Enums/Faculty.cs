namespace WebServer.App_Data.Models.Enums
{
    public enum Faculty
    {
        None = -1,
        ComputerScience,
        BehavioralSciences,
        InformationSystems,
        Nursing,
        ManagementEconomics,
        SocietyPolitics,
        ConsultingOrganizationalDevelopment,
        BusinessAdministration,
        Psychology
    }

    public class FacultyMethod
    {
        public static Faculty FacultyFromString(string facultyName)
        {
            if (facultyName.Equals("מדעי המחשב")) {
                return Faculty.ComputerScience;
            } else if (facultyName.Equals("מדעי התנהגות")) {
                return Faculty.BehavioralSciences;
            } else if (facultyName.Equals("ניהול מערכות מידע")) {
                return Faculty.InformationSystems;
            } else if (facultyName.Equals("מדעי הסיעוד")) {
                return Faculty.Nursing;
            } else if (facultyName.Equals("כלכלה וניהול")) {
                return Faculty.ManagementEconomics;
            } else if (facultyName.Equals("ממשל וחברה")) {
                return Faculty.SocietyPolitics;
            } else if (facultyName.Equals("יעוץ ופיתוח ארגוני")) {
                return Faculty.ConsultingOrganizationalDevelopment;
            } else if (facultyName.Equals("מנהל עסקים")) {
                return Faculty.BusinessAdministration;
            } else if (facultyName.Equals("פסיכולוגיה")) {
                return Faculty.Psychology;
            }
            return Faculty.None;
        }
    }
}
