using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace TopTenBites.Web.Infrastructure
{
    public static class HttpRequestExtensions
    {
        public static bool IsApiCall(this HttpRequest request)
        {
            bool isJson = request.GetTypedHeaders().Accept.Contains(
                new MediaTypeHeaderValue("application/json"));
            if (isJson)
                return true;
            if (request.Path.Value.StartsWith("/api/"))
                return true;
            return false;
        }
    }
}
