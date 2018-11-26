using System;

namespace RedmineBot.Helpers
{
    public class DateHelpers
    {
        /// <summary>
        /// Return date with last day of current month
        /// </summary>
        public static DateTime GetLastDay()
        {
            var today = DateTime.Today;
            var lastDay = DateTime.DaysInMonth(today.Year, today.Month);
            return new DateTime(today.Year, today.Month, lastDay);
        }

        /// <summary>
        /// Return date with first day of current month
        /// </summary>
        public static DateTime GetFirstDay =>
             new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        
        public static string DateFormat = "yyyy-MM-dd";
    }
}
