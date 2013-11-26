using System;
using System.IO;
using System.Web;

namespace MyCore.Helper {
    public static class WebHelper {
        public static string CombineUrl(params string[] urls) {
            if (urls != null && urls.Length > 0) {
                return Path.Combine(urls).Replace(@"\\", "/").Replace(@"\", "/");
            }
            return string.Empty;
        }
        public static string GetRootUrl() {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }
    }
}