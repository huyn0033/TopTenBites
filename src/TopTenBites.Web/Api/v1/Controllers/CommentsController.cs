using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.Api.v1.Validation;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Api.v1.Controllers
{
    [Route("api/v1/[controller]")]
    public class CommentsController : Controller
    {
        private IBusinessService _businessService;
        private IMenuItemService _menuItemService;
        private ICommentService _commentService;
        private IMapper _mapper;

        public CommentsController(IBusinessService businessService, IMenuItemService menuItemService, ICommentService commentService, IMapper mapper)
        {
            _businessService = businessService;
            _menuItemService = menuItemService;
            _commentService = commentService;
            _mapper = mapper;
        }

        // GET: api/v1/Comments/GetAllByBusinessId?filter={businessId}
        [HttpGet("GetAllByBusinessId")]
        public IActionResult GetAllByBusinessId(int filter)
        {
            var business = _businessService.GetByBusinessId(filter);
            if (business == null) return NotFound();
             
            var comments = business.MenuItems.SelectMany(x => x.Comments);
            var commentsVM = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentApiModel>>(comments);
            
            return Ok(commentsVM);
        }

        // GET: api/v1/Comments/GetAllByMenuItemId?filter={menuItemId}
        [HttpGet("GetAllByMenuItemId")]
        public IActionResult GetAllByMenuItemId(int filter)
        {
            var menuItem = _menuItemService.Get(filter);
            if (menuItem == null) return NotFound();

            var comments = menuItem.Comments.Select(x => x);
            var commentsVM = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentApiModel>>(comments);

            return Ok(commentsVM);
        }

        // GET: api/v1/Comments/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var comment = _commentService.Get(id);
            if (comment == null) return NotFound();
            
            var commentVM = _mapper.Map<Comment, CommentApiModel>(comment);

            return Ok(commentVM);
        }

        // POST: api/v1/Comments
        [HttpPost()]
        public IActionResult Post(/*[CustomizeValidator(RuleSet = "Insert")]*/ [FromBody] CommentApiModel commentVM)
        {
            var validator = new CommentViewModelValidator();
            var results = validator.Validate(commentVM, ruleSet: "Insert");
            results.AddToModelState(ModelState, null);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var comment = _mapper.Map<CommentApiModel, Comment>(commentVM);
            var commentOutput = _commentService.Add(comment);
            var commentVMOutput = _mapper.Map<Comment, CommentApiModel>(commentOutput);

            return Ok(commentVMOutput);
        }

        // PUT: api/v1/Comments
        [HttpPut()]
        public IActionResult Put(/*[CustomizeValidator(RuleSet = "Update")]*/ [FromBody] CommentApiModel commentVM)
        {
            var validator = new CommentViewModelValidator();
            var results = validator.Validate(commentVM, ruleSet: "Update");
            results.AddToModelState(ModelState, null);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var comment = _commentService.Get((int)commentVM.Id);
            if (comment == null) return NotFound();

            var commentMapped =_mapper.Map<CommentApiModel, Comment>(commentVM, comment);
            _commentService.Update(commentMapped);
            
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
