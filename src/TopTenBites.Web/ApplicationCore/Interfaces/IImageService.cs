using System.Collections.Generic;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IImageService
    {
        Image Get(int imageId);
        IEnumerable<Image> GetAll(string imageIds = "");
    }
}
