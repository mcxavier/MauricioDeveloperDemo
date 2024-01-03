using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Extensions
{
    public static class DateTimeExtensions
    {

        public static string ToDateTimeSqliteFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        //public static long ToJavascriptTimestamp(this DateTime input)
        //{
        //    TimeSpan span = new TimeSpan(new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        //    DateTime time = DateTime(input).Value.Subtract(span);

        //    return time.Ticks / 10000;
        //}

        public static DateTime ToLocalTime(this DateTime input, int? utcOffset)
        {
            return utcOffset == null ? DateTime.UtcNow : input.AddSeconds(utcOffset.Value);
        }

        public static string ToDateTimeUTCFormat(this DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        public static int CalculateAge(this DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

    }
    
}
