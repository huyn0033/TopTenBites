using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Dtos;
using TopTenBites.Web.ApplicationCore.Enums;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.Components;
using TopTenBites.Web.Core.Services;
using TopTenBites.Web.ViewModels;

namespace TopTenBites.Web.Controllers
{
    public class HomeController : Controller
    {
        const string CACHE_AUTOCOMPLETE_PREFIX = "ca_";
        const string CACHE_BUSINESS_SEARCH_PREFIX = "cbs_";
        const string COOKIE_FINGERPRINTHASH = "fingerprintHash";

        private IMemoryCache _memoryCache;
        private IYelpApiService _yelpApiService;
        private IBusinessService _businessService;
        private IImageProcessingService _imageService;
        private IMapper _mapper;
        private ILogger _logger;

        public HomeController(IMemoryCache memoryCache, IYelpApiService yelpApiService, IBusinessService businessService, 
            IImageProcessingService imageService, IMapper mapper, ILogger<HomeController> logger)
        {
            _memoryCache = memoryCache;
            _yelpApiService = yelpApiService;
            _businessService = businessService;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string find_desc, string find_loc, string lat, string lng)
        {
            var text = find_desc?.Trim();
            var location = find_loc?.Trim();
            lat = lat?.Trim();
            lng = lng?.Trim();

            string query = YelpApiService.GetYelpBusinessSearchQuery(text, location, lat, lng);

            var yelpBusinesses = new List<YelpBusiness>();
            YelpApiResultDto yelpApiResultDto = null;
            var cacheKey = CACHE_BUSINESS_SEARCH_PREFIX + query;
            var jsonResult = _memoryCache.Get<string>(cacheKey);
            if (!string.IsNullOrEmpty(jsonResult))
            {
                yelpBusinesses = JsonConvert.DeserializeObject<RootObject>(jsonResult).businesses;
            }
            else
            {
                yelpApiResultDto = await _yelpApiService.GetYelpBusinessSearchAsync(text, location, lat, lng);
                if (yelpApiResultDto.IsSuccessStatusCode)
                {
                    jsonResult = yelpApiResultDto.Result;
                    yelpBusinesses = JsonConvert.DeserializeObject<RootObject>(jsonResult).businesses;
                    
                    _memoryCache.Set<string>(cacheKey, jsonResult,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(24)));
                }
            }

            if (yelpBusinesses.Any())
            {
                IEnumerable<Business> businesses = _businessService.GetAll(string.Join(",", yelpBusinesses.Select(x => x.id)));
                foreach (var yb in yelpBusinesses)
                {
                    Business b = businesses.Where(x => x.YelpBusinessId == yb.id).FirstOrDefault();
                    if (b != null)
                    {
                        var fingerprintHash = Request.Cookies[COOKIE_FINGERPRINTHASH];

                        foreach (var mi in b.MenuItems)
                        {
                            var menuItemVM = _mapper.Map<MenuItem, MenuItemViewModel>(mi,
                                opts =>
                                {
                                    opts.Items["YelpBusinessId"] = mi.Business.YelpBusinessId;
                                    opts.Items["CurrentVisitorHasLiked"] = mi.Likes.Any(x => x.IsLike && x.UserFingerPrintHash == fingerprintHash);
                                    opts.Items["CurrentVisitorHasDisliked"] = mi.Likes.Any(x => !x.IsLike && x.UserFingerPrintHash == fingerprintHash);
                                });

                            yb.menuItems.Add(menuItemVM);
                        }
                    }

                    yb.menuItems = OrderMenuItems(yb.menuItems);
                }
            }

            ViewBag.find_desc = find_desc;
            ViewBag.find_loc = find_loc;
            ViewBag.lat = lat;
            ViewBag.lng = lng;
            return View(yelpBusinesses);
        }

        public async Task<IActionResult> GetYelpAutocompleteDescription(string text, string location, string lat, string lng)
        {
            text = text?.Trim();
            location = location?.Trim();
            lat = lat?.Trim();
            lng = lng?.Trim();

            string query = YelpApiService.GetYelpAutocompleteDescriptionQuery(text, location, lat, lng);

            YelpApiResultDto yelpApiResultDto = null;
            var cacheKey = CACHE_AUTOCOMPLETE_PREFIX + query;
            var jsonResult = _memoryCache.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(jsonResult))
            {
                yelpApiResultDto = await _yelpApiService.GetYelpAutocompleteDescriptionAsync(text, location, lat, lng);
                if (yelpApiResultDto.IsSuccessStatusCode)
                {
                    jsonResult = yelpApiResultDto.Result;

                    _memoryCache.Set<string>(query, jsonResult,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(24)));
                }
            }

            return new ContentResult()
            {
                StatusCode = (int?)yelpApiResultDto?.StatusCode,
                Content = jsonResult,
                ContentType = "application/json"
            };
        }

        public async Task<IActionResult> GetYelpAutocompleteLocation(string text)
        {
            text = text?.Trim();

            string query = YelpApiService.GetYelpAutocompleteLocationQuery(text);

            YelpApiResultDto yelpApiResultDto = null;
            var cacheKey = CACHE_AUTOCOMPLETE_PREFIX + query;
            var jsonResult = _memoryCache.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(jsonResult))
            {
                yelpApiResultDto = await _yelpApiService.GetYelpAutocompleteLocationAsync(text);
                if (yelpApiResultDto.IsSuccessStatusCode)
                {
                    jsonResult = yelpApiResultDto.Result;

                    _memoryCache.Set<string>(query, jsonResult,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(24)));
                }
            }

            return new ContentResult()
            {
                StatusCode = (int?)yelpApiResultDto?.StatusCode,
                Content = jsonResult,
                ContentType = "application/json"
            };
        }

        public IActionResult AddMenuItem(string txtMenuItem, string yelpBusinessId, string yelpBusinessAlias)
        {
            txtMenuItem = txtMenuItem?.Trim();
            if (string.IsNullOrWhiteSpace(txtMenuItem))
            {
                return BadRequest(new { ErrorMessage = "Field is required" });
            }

            if (_businessService.Get(yelpBusinessId)?.MenuItems.Any(x => x.Name == txtMenuItem) == true)
            {
                return BadRequest(new { ErrorMessage = $"'{txtMenuItem}' already exists" });
            }

            Business b = _businessService.AddMenuItem(txtMenuItem, yelpBusinessId, yelpBusinessAlias);

            List<MenuItemViewModel> menuItemsVM = new List<MenuItemViewModel>();
            if (b != null)
            {
                var fingerprintHash = Request.Cookies[COOKIE_FINGERPRINTHASH];
                foreach (var mi in b.MenuItems)
                {
                    var menuItemVM = _mapper.Map<MenuItem, MenuItemViewModel>(mi,
                        opts => 
                        {
                            opts.Items["YelpBusinessId"] = mi.Business.YelpBusinessId;
                            opts.Items["CurrentVisitorHasLiked"] = mi.Likes.Any(x => x.IsLike && x.UserFingerPrintHash == fingerprintHash);
                            opts.Items["CurrentVisitorHasDisliked"] = mi.Likes.Any(x => !x.IsLike && x.UserFingerPrintHash == fingerprintHash);
                        });

                    menuItemsVM.Add(menuItemVM);
                }

                menuItemsVM = OrderMenuItems(menuItemsVM);
            }

            ViewBag.MenuItemId = b.MenuItems.Where(x => x.Name == txtMenuItem).Select(x => x.MenuItemId).FirstOrDefault();
            ViewBag.YelpBusinessId = yelpBusinessId;
            return PartialView("_BusinessMenu", menuItemsVM);
        }

        public IActionResult AddLike(LikeAction likeAction, int menuItemId, string yelpBusinessId)
        {
            var fingerprintHash = Request.Cookies[COOKIE_FINGERPRINTHASH];
            Business b = _businessService.AddLike(likeAction, menuItemId, yelpBusinessId, fingerprintHash);

            List<MenuItemViewModel> menuItemsVM = new List<MenuItemViewModel>();
            if (b != null)
            {
                foreach (var mi in b.MenuItems)
                {
                    var menuItemVM = _mapper.Map<MenuItem, MenuItemViewModel>(mi,
                        opts =>
                        {
                            opts.Items["YelpBusinessId"] = mi.Business.YelpBusinessId;
                            opts.Items["CurrentVisitorHasLiked"] = mi.Likes.Any(x => x.IsLike && x.UserFingerPrintHash == fingerprintHash);
                            opts.Items["CurrentVisitorHasDisliked"] = mi.Likes.Any(x => !x.IsLike && x.UserFingerPrintHash == fingerprintHash);
                        });

                    menuItemsVM.Add(menuItemVM);
                }

                menuItemsVM = OrderMenuItems(menuItemsVM);
            }

            ViewBag.MenuItemId = menuItemId;
            ViewBag.YelpBusinessId = yelpBusinessId;
            return PartialView("_BusinessMenu", menuItemsVM);
        }

        public IActionResult GetMenuItemImagesAndComments(int menuItemId, string yelpBusinessId)
        {
            return ViewComponent(nameof(MenuItemInfo), new { menuItemId = menuItemId, yelpBusinessId = yelpBusinessId });
        }

        public IActionResult AddComment(string text, int commentSentiment, int menuItemId, string yelpBusinessId)
        {
            text = text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest(new { ErrorMessage = "Field is required" });
            }

            Business b = _businessService.AddComment(text, (Sentiment)commentSentiment, menuItemId, yelpBusinessId);

            return ViewComponent(nameof(MenuItemInfo), new { menuItemId = menuItemId, yelpBusinessId = yelpBusinessId });
        }

        public async Task<IActionResult> UploadImage(IFormFile file, int menuItemId, string yelpBusinessId)
        {
            var imageUploadResultDto = await _imageService.UploadImage(file, menuItemId, yelpBusinessId);

            if (imageUploadResultDto.IsSuccess)
                return Ok(imageUploadResultDto);
            else
                return BadRequest(imageUploadResultDto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<MenuItemViewModel> OrderMenuItems(List<MenuItemViewModel> menuItemsVM)
        {
            return menuItemsVM.OrderByDescending(x => CalculateMenuItemScore(x.LikeCount, x.DislikeCount))
                                           .ThenByDescending(x => x.LikeCount)
                                           .ToList();
        }

        private double CalculateMenuItemScore(int likeCount, int dislikeCount)
        {
            return likeCount == 0 && dislikeCount == 0 ? int.MinValue : likeCount * 2 - dislikeCount;
        }
    }
}
