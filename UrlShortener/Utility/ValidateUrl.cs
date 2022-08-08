using System.Net;
using System.Text.RegularExpressions;

namespace UrlShortener.Utility
{
    public class ValidateUrl
    {
        public static bool IsValidUrl(string url)
        {
            string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(url);
        }
    }
}
