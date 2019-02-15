using System;

namespace TopTenBites.Web.Api.v1.ApiModels
{
    public class BusinessApiModel
    {
        public int? Id { get; set; }
        public string YelpBusinessId { get; set; }
        public string YelpBusinessAlias { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
