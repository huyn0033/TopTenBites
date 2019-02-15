using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Dtos;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IImageProcessingService
    {
        Task<ImageUploadResultDto> UploadImage(IFormFile file, int menuItemId, string yelpBusinessId);
        string GetFileUploadVirtualDir(int businessId);
        string GetFileUploadPath(int businessId);
    }
}
