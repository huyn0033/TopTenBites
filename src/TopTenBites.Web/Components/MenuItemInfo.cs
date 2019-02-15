using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ViewModels;

namespace TopTenBites.Web.Components
{
    public class MenuItemInfo : ViewComponent
    {
        private IBusinessService _businessService;
        private IImageProcessingService _imageService;
        private IMapper _mapper;

        public MenuItemInfo(IBusinessService businessService, IImageProcessingService imageService, IMapper mapper)
        {
            _businessService = businessService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke(int? menuItemId, string yelpBusinessId)
        {
            MenuItemViewModel menuItemVM = null;

            if (menuItemId.HasValue)
            {
                Business b = _businessService.Get(yelpBusinessId);
                if (b != null)
                {
                    var mi = b.MenuItems.Where(x => x.MenuItemId == menuItemId).FirstOrDefault();
                    if (mi != null)
                    {
                        menuItemVM = new MenuItemViewModel()
                        {
                            Id = mi.MenuItemId,
                            Name = mi.Name,
                            YelpBusinessId = mi.Business.YelpBusinessId,
                        };

                        var comments = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(mi.Comments);
                        menuItemVM.Comments = comments.OrderByDescending(x => x.CreatedDate).ToList();

                        var images = _mapper.Map<IEnumerable<Image>, IEnumerable<ImageViewModel>>(mi.Images,
                            opts => opts.AfterMap((src, dest) => {
                                foreach (var d in dest)
                                {
                                    d.FileRelativeUrl = _imageService.GetFileUploadVirtualDir(b.BusinessId) + "/" + d.Name;
                                    d.FileRelativeUrlThumb = _imageService.GetFileUploadVirtualDir(b.BusinessId) + "/" + d.Name.Replace(".jpg", "_thumb.jpg");
                                }
                            }));
                        menuItemVM.Images = images.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                }
            }

            return View(nameof(MenuItemInfo), menuItemVM);
        }
    }
}
