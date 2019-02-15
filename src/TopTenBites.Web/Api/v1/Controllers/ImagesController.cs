using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TopTenBites.Web.Api.v1.Controllers
{
    [Route("api/v1/[controller]")]
    public class ImagesController : Controller
    {
        private IBusinessService _businessService;
        private IMenuItemService _menuItemService;
        private IImageService _imageService;
        private IMapper _mapper;

        public ImagesController(IBusinessService businessService, IMenuItemService menuItemService, IImageService imageService, IMapper mapper)
        {
            _businessService = businessService;
            _menuItemService = menuItemService;
            _imageService = imageService;
            _mapper = mapper;
        }

        // GET: api/v1/Images/GetAllByBusinessId?filter={businessId}
        [HttpGet("GetAllByBusinessId")]
        public IActionResult GetAllByBusinessId(int filter)
        {
            var business = _businessService.GetByBusinessId(filter);
            if (business == null) return NotFound();

            var images = business.MenuItems.SelectMany(x => x.Images);
            var imagesVM = _mapper.Map<IEnumerable<Image>, IEnumerable<ImageApiModel>>(images);

            return Ok(imagesVM);
        }

        // GET: api/v1/Images/GetAllByMenuItemId?filter={menuItemId}
        [HttpGet("GetAllByMenuItemId")]
        public IActionResult GetAllByMenuItemId(int filter)
        {
            var menuItem = _menuItemService.Get(filter);
            if (menuItem == null) return NotFound();

            var images = menuItem.Images.Select(x => x);
            var imagesVM = _mapper.Map<IEnumerable<Image>, IEnumerable<ImageApiModel>>(images);

            return Ok(imagesVM);
        }

        // GET: api/v1/Images/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var image = _imageService.Get(id);
            if (image == null) return NotFound();

            var imageVM = _mapper.Map<Image, ImageApiModel>(image);

            return Ok(imageVM);
        }

    }
}
