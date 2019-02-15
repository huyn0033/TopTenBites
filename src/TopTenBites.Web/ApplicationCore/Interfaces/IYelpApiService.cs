using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Dtos;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IYelpApiService
    {
        Task<YelpApiResultDto> GetYelpAutocompleteDescriptionAsync(string text, string location, string lat, string lng);
        Task<YelpApiResultDto> GetYelpAutocompleteLocationAsync(string text);
        Task<YelpApiResultDto> GetYelpBusinessSearchAsync(string text, string location, string lat, string lng);
    }
}
