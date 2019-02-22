using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Dtos;
using TopTenBites.Web.ApplicationCore.Models;
using Microsoft.Extensions.Options;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private IBusinessService _businessService;
        private IHostingEnvironment _hostingEnvironment;
        private AppSettingsOptions _appSettingsOptions;

        public ImageProcessingService(IBusinessService businessService, IHostingEnvironment hostingEnvironment, IOptions<AppSettingsOptions> appSettingsOptions)
        {
            _businessService = businessService;
            _hostingEnvironment = hostingEnvironment;
            _appSettingsOptions = appSettingsOptions.Value;
        }

        public async Task<ImageUploadResultDto> UploadImage(IFormFile file, int menuItemId, string yelpBusinessId)
        {
            var filenameWithoutExtHash = await GetFilenameWithoutExtHashFromFileAsync(file);
            var filenameWithExt = filenameWithoutExtHash + ".jpg";

            var business = _businessService.Get(yelpBusinessId);
            var menuItem = business.MenuItems.Where(x => x.MenuItemId == menuItemId).FirstOrDefault();
            if (menuItem == null) {
                return new ImageUploadResultDto() { IsSuccess = false, ErrorMessage = "Server error" };
            }
            if (menuItem.Images.Any(y => y.Name == filenameWithExt)) {
                return new ImageUploadResultDto() { IsSuccess = false, ErrorMessage = "Duplicate image" };
            }
            
            var imageUploadResultDto = processImageAsync(file, filenameWithoutExtHash, (int)business.BusinessId);
            if (imageUploadResultDto.IsSuccess)
            {
                _businessService.AddImage(filenameWithExt, menuItemId, yelpBusinessId);
            }

            return imageUploadResultDto;
        }

        private ImageUploadResultDto processImageAsync(IFormFile file, string filenameWithoutExtHash, int businessId)
        {
            const int IMAGE_WIDTH = 600;
            const int IMAGE_HEIGHT = 450;
            const int IMAGE_WIDTH_THUMB = 100;
            const int IMAGE_HEIGHT_THUMB = 75;
            
            string fileUploadPath = GetFileUploadPath(businessId);
            string fileNameWithExt = filenameWithoutExtHash + ".jpg";
            string fileNameWithExtThumb = filenameWithoutExtHash + "_thumb.jpg";

            string filePath = fileUploadPath + "\\" + fileNameWithExt;
            string filePathThumb = fileUploadPath + "\\" + fileNameWithExtThumb;

            Directory.CreateDirectory(fileUploadPath);

            using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
            {
                ResizeOptions options = new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Crop,
                    Size = new SixLabors.Primitives.Size() { Width = IMAGE_WIDTH, Height = IMAGE_HEIGHT }
                };

                ResizeOptions optionsThumb = new ResizeOptions() {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Crop,
                    Size = new SixLabors.Primitives.Size() { Width = IMAGE_WIDTH_THUMB, Height = IMAGE_HEIGHT_THUMB }
                };

                image.Mutate((IImageProcessingContext<Rgba32> x) => x
                    .Resize(options));
                image.Save(filePath);       // Automatic encoder selected based on extension.

                image.Mutate((IImageProcessingContext<Rgba32> x) => x
                    .Resize(optionsThumb));
                image.Save(filePathThumb);   // Automatic encoder selected based on extension.
            }

            var fileUploadVirtualDir = GetFileUploadVirtualDir(businessId);
            return new ImageUploadResultDto()
            {
                IsSuccess = true,
                FileRelativeUrl = fileUploadVirtualDir + "/" + fileNameWithExt,
                FileRelativeUrlThumb = fileUploadVirtualDir + "/" + fileNameWithExtThumb
            };
        }

        public string GetFileUploadVirtualDir(int businessId)
        {
            return _appSettingsOptions.UploadsVirtualDirectory + "/" + businessId.ToString();
        }

        public string GetFileUploadPath(int businessId)
        {
            return _appSettingsOptions.UploadsPath + "\\" + businessId.ToString();
        }

        private async Task<string> GetFilenameWithoutExtHashFromFileAsync(IFormFile file)
        {
            var hash = string.Empty;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var bytes = ms.ToArray();
                var md5 = MD5.Create();
                hash = BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "");
            }
            
            return hash;
        }
    }
}
