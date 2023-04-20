using System;
using System.Globalization;

namespace Utility
{
    public static class MathHelper
    {
        /// <summary>
        ///  converts a integer value to a string that represents it to x amount of decimal places
        /// </summary>
        public static string ToDecimalPlace(int value, int decimalPlacePoint) =>
            (value / (decimal) Math.Pow(10.00, decimalPlacePoint)).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///  converts a integer value into a decimal category so for example 10 would return tenths
        /// *Supports a standard int32 so the max value would be 2147483647 which is billions*
        /// </summary>
        public static string DeciamlCatergory(int value)
        {
            switch (value)
            {
                case < 1000:
                    return string.Empty;
                    break;
                case < 1000000:
                    return "Thousands";
                    break;
                case < 1000000000:
                    return "Millions";
                    break;
                // case < 1000000000:
                //     return "Trillions"; not supported by int32 unfortunately
                //     break;
                case 2147483647:
                    return "Maxed";
                break;
            }
            return string.Empty;
        }
        
        /// <summary>
        ///  converts a integer value into a decimal category shorthand so for example 1000 would return K or 1 million would return M
        /// *Supports a standard int32 so the max value would be 2147483647 which is billions*
        /// </summary>
        public static string DeciamlCatergoryShortHand(int value)
        {
            switch (value)
            {
                case < 1000:
                    return string.Empty;
                    break;
                case < 1000000:
                    return "K";
                    break;
                case < 1000000000:
                    return "M";
                    break;
                // case < 1000000000:
                //     return "Trillions"; not supported by int32 unfortunately
                //     break;
                case 2147483647:
                    return "Max";
                    break;
            }
            return string.Empty;
        }

    }
}