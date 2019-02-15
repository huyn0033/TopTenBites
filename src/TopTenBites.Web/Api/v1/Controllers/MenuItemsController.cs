using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.Api.v1.Validation;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Api.v1.Controllers
{
    [Route("api/v1/[controller]")]
    public class MenuItemsController : Controller
    {
        private IBusinessService _businessService;
        private IMenuItemService _menuItemService;
        private IMapper _mapper;

        public MenuItemsController(IBusinessService businessService, IMenuItemService menuItemService, IMapper mapper)
        {
            _businessService = businessService;
            _menuItemService = menuItemService;
            _mapper = mapper;
        }

        // GET: api/v1/MenuItems/GetAllByBusinessId?filter={businessId}
        [HttpGet("GetAllByBusinessId")]
        public IActionResult GetAllByBusinessId(int filter)
        {
            var business = _businessService.GetByBusinessId(filter);
            if (business == null) return NotFound();
             
            var menuItems = business.MenuItems;
            var menuItemsVM = _mapper.Map<IEnumerable<MenuItem>, IEnumerable<MenuItemApiModel>>(menuItems);
            
            return Ok(menuItemsVM);
        }

        // GET: api/v1/MenuItems/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var menuItem = _menuItemService.Get(id);
            if (menuItem == null) return NotFound();
            
            var menuItemVM = _mapper.Map<MenuItem, MenuItemApiModel>(menuItem);

            return Ok(menuItemVM);
        }

        // POST: api/v1/MenuItems
        [HttpPost()]
        public IActionResult Post(/*[CustomizeValidator(RuleSet = "Insert")]*/ [FromBody] MenuItemApiModel menuItemVM)
        {
            var validator = new MenuItemViewModelValidator(_businessService);
            var results = validator.Validate(menuItemVM, ruleSet: "Insert");
            results.AddToModelState(ModelState, null);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var menuItem = _mapper.Map<MenuItemApiModel, MenuItem>(menuItemVM);
            var menuItemOutput = _menuItemService.Add(menuItem);
            var menuItemVMOutput = _mapper.Map<MenuItem, MenuItemApiModel>(menuItemOutput);

            return Ok(menuItemVMOutput);
        }

        // PUT: api/v1/MenuItems
        [HttpPut()]
        public IActionResult Put(/*[CustomizeValidator(RuleSet = "Update")]*/ [FromBody] MenuItemApiModel menuItemVM)
        {
            var validator = new MenuItemViewModelValidator(_businessService);
            var results = validator.Validate(menuItemVM, ruleSet: "Update");
            results.AddToModelState(ModelState, null);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var menuItem = _menuItemService.Get((int)menuItemVM.Id);
            if (menuItem == null) return NotFound();

            var menuItemMapped = _mapper.Map<MenuItemApiModel, MenuItem>(menuItemVM, menuItem);
            _menuItemService.Update(menuItemMapped);
            
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
