using System;
using System.Collections.Generic;

namespace TopTenBites.Web.ApplicationCore.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public string YelpBusinessId { get; set; }
        public string YelpBusinessAlias { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
