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
    public class BusinessesController : Controller
    {
        private IBusinessService _businessService;
        private IMapper _mapper;

        public BusinessesController(IBusinessService businessService, IMapper mapper)
        {
            _businessService = businessService;
            _mapper = mapper;
        }

        // GET: api/v1/businesses
        [HttpGet]
        public IActionResult Get()
        {
            var businesses = _businessService.GetAll();
            var businessesVM = _mapper.Map<IEnumerable<Business>, IEnumerable<BusinessApiModel>>(businesses);

            return Ok(businessesVM);
        }

        // GET api/businesses/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var business = _businessService.GetByBusinessId(id);
            if (business == null) return NotFound();

            var businessVM = _mapper.Map<Business, BusinessApiModel>(business);

            return Ok(businessVM);
        }

        // POST api/businesses
        [HttpPost()]
        public IActionResult Post(/*[CustomizeValidator(RuleSet = "Insert")]*/ [FromBody] BusinessApiModel businessVM)
        {
            var validator = new BusinessViewModelValidator(_businessService);
            var results = validator.Validate(businessVM, ruleSet: "Insert");
            results.AddToModelState(ModelState, null);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var business = _mapper.Map<BusinessApiModel, Business>(businessVM);
            var businessOutput = _businessService.Add(business);
            var businessVMOutput = _mapper.Map<Business, BusinessApiModel>(businessOutput);

            return Ok(businessVMOutput);
        }

        // PUT api/<controller>/5
        [HttpPut()]
        public IActionResult Put(/*[CustomizeValidator(RuleSet = "Update")]*/ [FromBody] BusinessApiModel businessVM)
        {
            var validator = new BusinessViewModelValidator(_businessService);
            var results = validator.Validate(businessVM, ruleSet: "Update");
            results.AddToModelState(ModelState, null);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var business = _businessService.GetByBusinessId((int)businessVM.Id);
            if (business == null) return NotFound();
            
            var businessMapped = _mapper.Map<BusinessApiModel, Business>(businessVM, business);
            _businessService.Update(businessMapped);

            return Ok();
        }

        // DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
