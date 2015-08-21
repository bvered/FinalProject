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
            if (facultyName.Equals("מד' המחשב תואר ראשון")) {
                return Faculty.ComputerScience;
            } else if (facultyName.Equals("מד' התנהגות תואר ראשון")) {
                return Faculty.BehavioralSciences;
            } else if (facultyName.Equals("ניהול מערכות מידע תואר ראשון")) {
                return Faculty.InformationSystems;
            } else if (facultyName.Equals("מדעי הסיעוד תואר ראשון")) {
                return Faculty.Nursing;
            } else if (facultyName.Equals("כלכלה וניהול תואר ראשון")) {
                return Faculty.ManagementEconomics;
            } else if (facultyName.Equals("ממשל וחברה תואר ראשון")) {
                return Faculty.SocietyPolitics;
            } else if (facultyName.Equals("יעוץ ופיתוח ארגוני תואר שני")) {
                return Faculty.ConsultingOrganizationalDevelopment;
            } else if (facultyName.Equals("מנהל עסקים תואר שני")) {
                return Faculty.BusinessAdministration;
            } else if (facultyName.Equals("פסיכולוגיה תואר שני")) {
                return Faculty.Psychology;
            }
            return Faculty.None;
        }
    }
}
