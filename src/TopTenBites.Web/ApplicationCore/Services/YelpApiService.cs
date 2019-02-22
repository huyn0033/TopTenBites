using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Dtos;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;

namespace TopTenBites.Web.Core.Services
{
    public class YelpApiService : IYelpApiService
    {
        private AppSettingsOptions _appSettingsOptions;
        private IHttpClientFactory _httpClientFactory;

        public YelpApiService(IHttpClientFactory httpClientFactory, IOptions<AppSettingsOptions> appSettingsOptions)
        {
            _httpClientFactory = httpClientFactory;
            _appSettingsOptions = appSettingsOptions.Value;
        }

        public static string GetYelpAutocompleteDescriptionQuery(string text, string location, string lat, string lng)
        {
            var s = string.Empty;
            if (location == "Current Location")
                s = $"lat={lat}&lng={lng}&is_new_loc=true&prefix={text}&is_initial_prefetch=";
            else
                s = $"loc={location}&loc_name_param=loc&is_new_loc=true&prefix={text}&is_initial_prefetch=";

            return s;
        }

        public static string GetYelpBusinessSearchQuery(string text, string location, string lat, string lng)
        {
            var s = string.Empty;
            if (location == "Current Location")
                s = $"term={text}&latitude={lat}&longitude={lng}";
            else
                s = $"term={text}&location={location}";

            return s;
        }

        public static string GetYelpAutocompleteLocationQuery(string text) => $"prefix={text}";

        public async Task<YelpApiResultDto> GetYelpAutocompleteDescriptionAsync(string text, string location, string lat, string lng)
        {
            var query = GetYelpAutocompleteDescriptionQuery(text, location, lat, lng);
            var url = $"https://www.yelp.com/search_suggest/v2/prefetch?{query}";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = new YelpApiResultDto() { Query = query, Url = url };

            HttpResponseMessage response = await client.GetAsync(url);
            result.IsSuccessStatusCode = response.IsSuccessStatusCode;
            if (response.IsSuccessStatusCode)
            {
                result.StatusCode = (int)response.StatusCode;
                result.Result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<YelpApiResultDto> GetYelpAutocompleteLocationAsync(string text)
        {
            string query = GetYelpAutocompleteLocationQuery(text);
            string url = $"https://www.yelp.com/location_suggest/v2?{query}";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = new YelpApiResultDto() { Query = query, Url = url };

            HttpResponseMessage response = await client.GetAsync(url);
            result.IsSuccessStatusCode = response.IsSuccessStatusCode;
            if (response.IsSuccessStatusCode)
            {
                result.StatusCode = (int)response.StatusCode;
                result.Result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<YelpApiResultDto> GetYelpBusinessSearchAsync(string text, string location, string lat, string lng)
        {
            string apiKey = _appSettingsOptions.YelpApiKey;
                
            var query = GetYelpBusinessSearchQuery(text, location, lat, lng);
            var url = $"https://api.yelp.com/v3/businesses/search?{query}";

            var client = _httpClientFactory.CreateClient("yelp");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var result = new YelpApiResultDto() { Query = query, Url = url };
            HttpResponseMessage response = await client.GetAsync(url);
            result.IsSuccessStatusCode = response.IsSuccessStatusCode;
            if (response.IsSuccessStatusCode)
            {
                result.StatusCode = (int)response.StatusCode;
                result.Result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
    }
}
