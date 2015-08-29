namespace WebServer.App_Data.Models.Enums
{
    public enum AcademicDegree
    {
        Bachelor,
        Master
    }

    public class AcademicDegreeMethod
    {
        public static AcademicDegree AcademicDegreeFromString(string academicDegree)
        {
            if (academicDegree == "שני")
            {
                return AcademicDegree.Master;
            }
            else
            {
                return AcademicDegree.Bachelor;
            }
        }
    }
}