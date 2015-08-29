namespace WebServer.App_Data.Models.Enums
{
    public enum Semester
    {
        A,
        B, 
        Summer,
    }

    public class SemesterMethod
    {
        public static Semester SemesterFromString(string semester)
        {
            if (semester == "1")
            {
                return Semester.A;
            }
            else if (semester == "2")
            {
                return Semester.B;
            }
            else if (semester == "3")
            {
                return Semester.Summer;
            }

            return Semester.A;
//להוסיף אולי לסמסטר משהו שהוא כללי, נלקח בכל הסמסטרים?
        }
    }
}