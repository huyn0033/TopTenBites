using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Enums;

namespace TopTenBites.Web.Api.v1.ApiModels
{
    public class CommentApiModel
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public Sentiment Sentiment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int MenuItemId { get; set; }
    }
}
