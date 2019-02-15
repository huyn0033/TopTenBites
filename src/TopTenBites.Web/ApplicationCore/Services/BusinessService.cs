using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ViewModels;
using TopTenBites.Web.ApplicationCore.Enums;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class BusinessService : IBusinessService
    {
        private IGenericRepository<Business> _businessRepository;
        public BusinessService(IGenericRepository<Business> businessRepository)
        {
            _businessRepository = businessRepository;
        }

        public Business Get(string yelpBusinessId)
        {
            return _businessRepository.Get(filter: x => x.YelpBusinessId == yelpBusinessId,
                                   includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
        }

        public Business GetByBusinessId(int businessId)
        {
            return _businessRepository.Get(filter: x => x.BusinessId == businessId,
                                   includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
        }

        public IEnumerable<Business> GetAll(string yelpBusinessIds = "")
        {
            IEnumerable<Business> businesses = Enumerable.Empty<Business>();

            string[] aryYelpBusinessIds = yelpBusinessIds?.Split(",")
                                                          .Select(x => x.Trim())
                                                          .Where(x => !string.IsNullOrWhiteSpace(x))
                                                          .ToArray(); ;
            if (aryYelpBusinessIds?.Length > 0)
            {
                businesses = _businessRepository.GetAll(filter: x => aryYelpBusinessIds.Contains(x.YelpBusinessId),
                                                includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
            }
            else
            {
                businesses = _businessRepository.GetAll(includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
            }

            return businesses;
        }

        public Business Add(Business business)
        {
            return _businessRepository.Add(business);
        }

        public void Update(Business business)
        {
            _businessRepository.Update(business);
        }

        public Business AddMenuItem(string menuItemName, string yelpBusinessId, string yelpBusinessAlias)
        {
            Business b = _businessRepository.Get(filter: x => x.YelpBusinessId == yelpBusinessId, 
                                        includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");

            if (b == null)
            {
                b = new Business()
                {
                    YelpBusinessId = yelpBusinessId,
                    YelpBusinessAlias = yelpBusinessAlias
                };
            }

            if (b != null)
            {
                if (!b.MenuItems.Any(x => x.Name == menuItemName))
                {
                    b.MenuItems.Add(
                        new MenuItem()
                        {
                            BusinessId = b.BusinessId,
                            Name = menuItemName
                        }
                    );
                    if (b.BusinessId == 0)
                        _businessRepository.Add(b);
                    else
                        _businessRepository.Update(b);
                }
            }

            return b;
        }

        public Business AddLike(LikeAction likeAction, int menuItemId, string yelpBusinessId, string fingerprintHash)
        {
            Business b = _businessRepository.Get(filter: x => x.YelpBusinessId == yelpBusinessId,
                                        includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
            if (b != null)
            {
                //don't allow user to like/dislike MenuItem multiple times
                MenuItem mi = b.MenuItems.Where(x => x.MenuItemId == menuItemId 
                                            && !x.Likes.Any(y => y.UserFingerPrintHash == fingerprintHash)).FirstOrDefault();
                if (mi != null)
                {
                    mi.Likes.Add(
                        new Like
                        {
                            IsLike = likeAction == LikeAction.Like ? true : false,
                            UserFingerPrintHash = fingerprintHash
                        }
                    );
                    _businessRepository.Update(b);
                }
            }

            return b;
        }

        public Business AddComment(string text, Sentiment commentSentiment, int menuItemId, string yelpBusinessId)
        {
            Business b = _businessRepository.Get(filter: x => x.YelpBusinessId == yelpBusinessId,
                                        includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
            if (b != null)
            {
                MenuItem mi = b.MenuItems.Where(x => x.MenuItemId == menuItemId).FirstOrDefault();
                if (mi != null)
                {
                    mi.Comments.Add(
                        new Comment()
                        {
                            Text = text,
                            Sentiment = commentSentiment
                        }
                    );
                    _businessRepository.Update(b);
                }
            }

            return b;
        }

        public Business AddImage(string filename, int menuItemId, string yelpBusinessId)
        {
            Business b = _businessRepository.Get(filter: x => x.YelpBusinessId == yelpBusinessId,
                                        includeProperties: "MenuItems,MenuItems.Likes,MenuItems.Comments,MenuItems.Images");
            if (b != null)
            {
                MenuItem mi = b.MenuItems.Where(x => x.MenuItemId == menuItemId).FirstOrDefault();
                if (mi != null)
                {
                    mi.Images.Add(
                        new Image()
                        {
                            Name = filename
                        }
                    );
                    _businessRepository.Update(b);
                }
            }

            return b;
        }

    }
}
