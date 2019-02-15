using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class ImageService : IImageService
    {
        private IGenericRepository<Image> _imageRepository;

        public ImageService(IGenericRepository<Image> imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public Image Get(int imageId)
        {
            return _imageRepository.GetById(imageId);
        }

        public IEnumerable<Image> GetAll(string imageIds = "")
        {
            IEnumerable<Image> images = Enumerable.Empty<Image>();

            int[] aryImageIds = imageIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            if (aryImageIds?.Length > 0)
            {
                images = _imageRepository.GetAll(filter: x => aryImageIds.Contains((int)x.ImageId));
            }
            else
            {
                images = _imageRepository.GetAll();
            }

            return images;
        }

    }
}
