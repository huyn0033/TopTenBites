using System;
using TopTenBites.Web.ApplicationCore.Enums;

namespace TopTenBites.Web.ApplicationCore.Models
{
    public class Comment
    {
        public int? CommentId { get; set; }
        public string Text { get; set; }
        public Sentiment Sentiment { get; set; } = Sentiment.Unknown;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int MenuItemId { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
