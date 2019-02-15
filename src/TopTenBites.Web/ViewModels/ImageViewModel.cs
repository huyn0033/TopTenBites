using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopTenBites.Web.ViewModels
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FileRelativeUrl { get; set; }
        public string FileRelativeUrlThumb { get; set; }
    }
}
