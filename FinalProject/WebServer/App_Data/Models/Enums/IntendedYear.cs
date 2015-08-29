namespace WebServer.App_Data.Models.Enums
{
    public enum IntendedYear
    {
        Any,
        First,
        Second,
        Third,
        Forth
    }

    public class IntendedYearMethod
    {
        public static IntendedYear IntendedYearFromInt(int intendedYear)
        {
            if (intendedYear == 0)
            {
                return IntendedYear.Any;
            }
            else if (intendedYear == 1)
            {
                return IntendedYear.First;
            }
            else if (intendedYear == 2)
            {
                return IntendedYear.Second;
            }
            else if (intendedYear == 3)
            {
                return IntendedYear.Third;
            }
            else if (intendedYear == 4)
            {
                return IntendedYear.Forth;
            }

            return IntendedYear.Any;
        }
    }
}