using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Enums;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ViewModels;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IBusinessService
    {
        Business Get(string yelpBusinessId);
        Business GetByBusinessId(int businessId);
        IEnumerable<Business> GetAll(string yelpBusinessIds = "");
        Business Add(Business business);
        void Update(Business business);
        Business AddMenuItem(string menuItemName, string yelpBusinessId, string yelpBusinessAlias);
        Business AddLike(LikeAction likeAction, int menuItemId, string yelpBusinessId, string fingerprintHash);
        Business AddComment(string text, Sentiment commentSentiment, int menuItemId, string yelpBusinessId);
        Business AddImage(string filename, int menuItemId, string yelpBusinessId);
    }
}
