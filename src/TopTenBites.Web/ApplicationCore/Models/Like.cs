using System;

namespace TopTenBites.Web.ApplicationCore.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public bool IsLike { get; set; }
        public string UserFingerPrintHash { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int MenuItemId {get; set;}
        public virtual MenuItem MenuItem { get; set; }
    }
}
