using System;
using System.Linq;
using System.Web;

namespace MyCore.Helper {
    public static class WebHelper {
        public static string CombineUrl(params string[] urls) {
            if (urls != null && urls.Length > 0) {
                if (urls[0].StartsWith("~")) {
                    urls[0] = GetRootUrl() + urls[0].TrimStart('~');
                }
                return string.Join("/", urls.Select(url => url.Replace(@"\", "/").Trim('/')));
            }
            return string.Empty;
        }
        public static string GetRootUrl() {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }
    }
}