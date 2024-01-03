using System;

namespace Utils
{
    public static class DateTimeBrazil
    {
        public static DateTime Now
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")); }
        }

        public static DateTime Today
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }
        public static DateTime AllStart
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1900, 1, 1), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime WeekStart
        {
            get { return DateTime.UtcNow.AddDays(-(DateTime.UtcNow.Date.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek)).Date; }
        }

        public static DateTime WeekEnd
        {
            get { return DateTimeBrazil.WeekStart.AddDays(6); }
        }

        public static DateTime YearStart
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Today.Year, 1, 1), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime YearEnd
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Today.Year, 12, 31), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime MonthStart
        {
            // get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(1 - DateTime.Today.Day), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
            get { return DateTimeBrazil.Now.AddDays(1 - DateTimeBrazil.Today.Day).Date; }
        }

        public static DateTime MonthEnd
        {
            // get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(1 - (DateTime.UtcNow.Day)).AddMonths(1).AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
            get { return DateTimeBrazil.Now.AddDays(1 - (DateTimeBrazil.Now.Day)).AddMonths(1).AddDays(-1).Date; }
        }

        public static DateTime ThreemesterStart
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Today.Year, (DateTime.Today.Month <= 3 ? 1 : DateTime.Today.Month <= 4 ? 4 : DateTime.Today.Month <= 7 ? 7 : 12), 1), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime ThreemesterEnd
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Today.Year, (DateTime.Today.Month <= 3 ? 3 : DateTime.Today.Month <= 4 ? 6 : DateTime.Today.Month <= 7 ? 9 : 10), (DateTime.Today.Month <= 3 ? 31 : DateTime.Today.Month <= 4 ? 30 : DateTime.Today.Month <= 7 ? 30 : 31)), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime SemesterStart
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Today.Year, DateTime.Today.Month <= 6 ? 1 : 7, 1), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime SemesterEnd
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Today.Year, DateTime.Today.Month <= 6 ? 6 : 12, DateTime.Today.Month <= 6 ? 30 : 31), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date; }
        }

        public static DateTime? ToUTC(DateTime? source)
        {
            if (source == null)
                return null;

            if (source.Value.Year < 1940)
                return new DateTime(source.Value.Year, source.Value.Month, source.Value.Day, 4, 0, 0, DateTimeKind.Utc);

            if (source != null && TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time").IsInvalidTime(source.Value))
                source = source.Value.AddHours(2);
            return source == null ? (DateTime?)null : TimeZoneInfo.ConvertTimeToUtc(source ?? DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public static DateTime? FromUTC(DateTime? source)
        {
            return source == null ? (DateTime?)null : TimeZoneInfo.ConvertTimeFromUtc(source ?? DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public static int GetAge(DateTime birthday)
        {
            int age = DateTimeBrazil.Today.Year - birthday.Year;

            if (DateTimeBrazil.Today < birthday.AddYears(age))
                age--;

            return age;
        }

        public static DateTime AddWorkingDays(this DateTime date, int addDays)
        {
            while (addDays != 0)
            {
                date = date.AddDays(Math.Sign(addDays));
                if (IsBusinessDay(date))
                    addDays = addDays - Math.Sign(addDays);
            }

            return date;
        }

        private static bool IsBusinessDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return true;
                default:
                    return false;
            }
        }
    }
}
