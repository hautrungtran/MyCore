using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyCore.Extension {
    public static class StringExtension {
        public static string ToString(this decimal number, FormatOptions options) {
            if (options == null) {
                options = new FormatOptions {
                    Format = CultureInfo.InvariantCulture,
                    Unit = 1,
                    Scale = 2,
                    DisplayZero = true
                };
            }
            if (!options.DisplayZero && number == 0) {
                return string.Empty;
            }
            if (options.Unit.HasValue && options.Unit > 1) {
                number = number / options.Unit.Value;
            }
            var formatString = "#,0.";
            if (options.Scale.HasValue && options.Scale > 0) {
                options.Scale = 2;
            }
            for (var i = 0; i < options.Scale; i++) {
                formatString += "#";
            }
            if (options.Format == null) {
                options.Format = CultureInfo.InvariantCulture;
            }
            return number.ToString(formatString, options.Format);
        }
        public static string ToString(this decimal? number, FormatOptions options) {
            return number.HasValue ? number.Value.ToString(options) : string.Empty;
        }
        public static string ToString(this int number, FormatOptions options) {
            return Convert.ToDecimal(number).ToString(options);
        }
        public static string ToString(this int? number, FormatOptions options) {
            return number.HasValue ? Convert.ToDecimal(number.Value).ToString(options) : string.Empty;
        }
        public static string ToString(this DateTime dateTime, FormatOptions options) {
            if (options.Format == null) {
                options.Format = CultureInfo.InvariantCulture;
            }
            return dateTime.ToString("d", options.Format);
        }
        public static string ToString(this DateTime? dateTime, FormatOptions options) {
            return dateTime.HasValue ? dateTime.Value.ToString(options.Format) : string.Empty;
        }
        public static string RemoveUnicode(this string s) {
            if (string.IsNullOrEmpty(s)) {
                return string.Empty;
            }
            const string s1 = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string s2 = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            var result = s;
            foreach (var c in s) {
                var i = s1.IndexOf(c);
                if (i > -1) {
                    result = result.Replace(c, s2[i]);
                }
            }
            return result;
        }
        public static T To<T>(this string s) {
            return (T)TypeDescriptor.GetConverter(typeof(string)).ConvertFromString(s);
        }
        public static bool IsEmail(this string email) {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }
        public static bool IsColor(this string color) {
            return Regex.IsMatch(color, @"^#(?:[0-9a-fA-F]{3}){1,2}$");
        }
    }
    public class FormatOptions {
        public IFormatProvider Format { get; set; }
        public int? Unit { get; set; }
        public int? Scale { get; set; }
        public bool DisplayZero { get; set; }
    }
}