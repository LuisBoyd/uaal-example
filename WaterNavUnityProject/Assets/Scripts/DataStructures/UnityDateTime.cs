using System;
using RCR.Utilities;

namespace DataStructures
{
    [Serializable]
    public struct UnityDateTime: IComparable<UnityDateTime>
    {
        public int Years;
        public int Months;
        public int Days;
        public int Hours;
        public double Minutes;
        public double Seconds;
        
        
        public int CompareTo(UnityDateTime other)
        {
            if (other.Years > this.Years)
                return 1;
            if (other.Months > this.Months)
                return 1;
            if (other.Days > this.Days)
                return 1;
            if (other.Hours > this.Hours)
                return 1;
            if (other.Minutes > this.Minutes)
                return 1;
            if (other.Seconds > this.Seconds)
                return 1;

            if (other.Years == this.Years || other.Months == this.Months || other.Days == this.Days
                || other.Hours == this.Hours || MathUtils.FloatingPointComparision(other.Minutes, this.Minutes) ||
                MathUtils.FloatingPointComparision(other.Seconds, this.Seconds))
                return 0;

            return -1;
        }

        public static explicit operator UnityDateTime(double f)
        {
            //e = 10
            //So 1.0790280948756975e+31 is (1.0790280948756975 * 10)^31
            UnityDateTime newTime = new UnityDateTime();
            TimeSpan ts = TimeSpan.FromSeconds(f);
            newTime.Years = ts.Days / 365;
            newTime.Months = (ts.Days - (ts.Days / 365) * 365) / 30;
            newTime.Days = (ts.Days - (ts.Days / 365) * 365) - ((ts.Days - (ts.Days / 365) * 365) / 30) * 30;
            newTime.Hours = ts.Hours;
            newTime.Minutes = ts.Minutes;
            newTime.Seconds = ts.Seconds;
            return newTime;
        }
        public static explicit operator UnityDateTime(float f)
        {
            //e = 10
            //So 1.0790280948756975e+31 is (1.0790280948756975 * 10)^31
            UnityDateTime newTime = new UnityDateTime();
            TimeSpan ts = TimeSpan.FromSeconds(f);
            newTime.Years = ts.Days / 365;
            newTime.Months = (ts.Days - (ts.Days / 365) * 365) / 30;
            newTime.Days = (ts.Days - (ts.Days / 365) * 365) - ((ts.Days - (ts.Days / 365) * 365) / 30) * 30;
            newTime.Hours = ts.Hours;
            newTime.Minutes = ts.Minutes;
            newTime.Seconds = ts.Seconds;
            return newTime;
        }
    }
}