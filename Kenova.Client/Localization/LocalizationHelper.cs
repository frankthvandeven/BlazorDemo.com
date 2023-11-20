using System.Globalization;

namespace Kenova.Client.Localization
{
    public static class LocalizationHelper
    {

        public static DateOrderEnum DateOrder()
        {
            //string short_pattern = CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern;
            string short_pattern = CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.ShortDatePattern;

            string first = short_pattern.Substring(0, 1).ToUpper();

            if (first == "M")
                return DateOrderEnum.MDY;
            else if (first == "Y")
                return DateOrderEnum.YMD;

            return DateOrderEnum.DMY;
        }

        /// <summary>
        /// Converts the specified year to a four-digit year by using the System.Globalization.Calendar.TwoDigitYearMax
        ///  property to determine the appropriate century.
        /// </summary>
        public static int ToFourDigitYear(int year)
        {
            //return CultureInfo.CurrentCulture.DateTimeFormat.Calendar.ToFourDigitYear(year);
            //return CultureInfo.InstalledUICulture.DateTimeFormat.Calendar.ToFourDigitYear(year);
            return CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.Calendar.ToFourDigitYear(year);
        }

    }

    public enum DateOrderEnum
    {
        MDY = 0,
        DMY = 1,
        YMD = 2
    }

}



//[DllImport("kernel32.dll")]
//private static extern int GetLocaleInfo(int Locale, int LCType, [Out] StringBuilder lpLCData, int cchData);
//private const int LOCALE_SYSTEM_DEFAULT = 0x400;
//private const int LOCALE_IDATE = 0x21;

//private static string GetInfo(int lInfo)
//{
//    StringBuilder lpLCData = new StringBuilder(256);
//    int ret = GetLocaleInfo(LOCALE_SYSTEM_DEFAULT, lInfo, lpLCData, lpLCData.Capacity);
//    if (ret > 0)
//    {
//        return lpLCData.ToString().Substring(0, ret - 1);
//    }
//    return string.Empty;
//}
//
//
//   return (DateOrder)Convert.ToInt32(GetInfo(LOCALE_IDATE));
