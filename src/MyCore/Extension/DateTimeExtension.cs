using System;

namespace MyCore.Extension {
    public static class DateTimeExtension {
        public static DateTime BeginDate(this DateTime date) { return date.Date; }
        public static DateTime EndDate(this DateTime date) { return date.Date == DateTime.MaxValue.Date ? DateTime.MaxValue : date.Date.AddDays(1).AddTicks(-1); }
        public static DateTime BeginDate(this DateTime? date) { return date.HasValue ? date.Value.BeginDate() : DateTime.MinValue; }
        public static DateTime EndDate(this DateTime? date) { return date.HasValue ? date.Value.EndDate() : DateTime.Now; }
    }
}