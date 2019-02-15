using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopTenBites.Web.ApplicationCore.Dtos
{
    public class ImageUploadResultDto
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string FileRelativeUrl { get; set; }
        public string FileRelativeUrlThumb { get; set; }
    }
}
