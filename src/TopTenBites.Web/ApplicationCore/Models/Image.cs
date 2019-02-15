using System;

namespace TopTenBites.Web.ApplicationCore.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int MenuItemId { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
