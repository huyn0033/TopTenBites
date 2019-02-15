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
    public class LikesController : Controller
    {
        private IBusinessService _businessService;
        private IMenuItemService _menuItemService;
        private ILikeService _likeService;
        private IMapper _mapper;

        public LikesController(IBusinessService businessService, IMenuItemService menuItemService, ILikeService likeService, IMapper mapper)
        {
            _businessService = businessService;
            _menuItemService = menuItemService;
            _likeService = likeService;
            _mapper = mapper;
        }

        // GET: api/v1/Likes/GetAllByBusinessId?filter={businessId}
        [HttpGet("GetAllByBusinessId")]
        public IActionResult GetAllByBusinessId(int filter)
        {
            var business = _businessService.GetByBusinessId(filter);
            if (business == null) return NotFound();

            var likes = business.MenuItems.SelectMany(x => x.Likes);
            var likesVM = _mapper.Map<IEnumerable<Like>, IEnumerable<LikeApiModel>>(likes);

            return Ok(likesVM);
        }

        // GET: api/v1/Likes/GetAllByMenuItemId?filter={menuItemId}
        [HttpGet("GetAllByMenuItemId")]
        public IActionResult GetAllByMenuItemId(int filter)
        {
            var menuItem = _menuItemService.Get(filter);
            if (menuItem == null) return NotFound();

            var likes = menuItem.Likes.Select(x => x);
            var likesVM = _mapper.Map<IEnumerable<Like>, IEnumerable<LikeApiModel>>(likes);

            return Ok(likesVM);
        }

        // GET: api/v1/Likes/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var like = _likeService.Get(id);
            if (like == null) return NotFound();

            var likeVM = _mapper.Map<Like, LikeApiModel>(like);

            return Ok(likeVM);
        }

    }
}
