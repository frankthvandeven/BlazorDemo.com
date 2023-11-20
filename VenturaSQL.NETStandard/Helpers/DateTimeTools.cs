using System;
using System.Text;

namespace VenturaSQL
{

    public static class DateTimeTools
    {

        public static int CalcAgeInYears(DateTime birthDate, DateTime now)
        {
            int result = now.Year - birthDate.Year;
            if (now.DayOfYear >= birthDate.DayOfYear)
                return result;
            else
                return result - 1;
        } // end of method


        public static string TimestampISO(DateTime datetime)
        {
            return datetime.ToString("yyyyMMddHHmmss"); // you can add 1 to 7 f's for second fractions
        }

    } // end of class

} // end of namespace
