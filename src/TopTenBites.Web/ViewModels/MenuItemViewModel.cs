using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ViewModels
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string YelpBusinessId { get; set; }
        public bool CurrentVisitorHasLiked { get; set; }
        public bool CurrentVisitorHasDisliked { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    }
}
