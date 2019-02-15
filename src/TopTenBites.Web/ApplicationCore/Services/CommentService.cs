using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class CommentService :ICommentService
    {
        private IGenericRepository<Comment> _commentRepository;

        public CommentService(IGenericRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Comment Get(int commentId)
        {
            return _commentRepository.GetById(commentId);
        }

        public IEnumerable<Comment> GetAll(string commentIds = "")
        {
            IEnumerable<Comment> comments = Enumerable.Empty<Comment>();

            int[] aryCommentIds = commentIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            if (aryCommentIds?.Length > 0)
            {
                comments = _commentRepository.GetAll(filter: x => aryCommentIds.Contains((int)x.CommentId));
            }
            else
            {
                comments = _commentRepository.GetAll();
            }

            return comments;
        }

        public Comment Add(Comment comment)
        {
            return _commentRepository.Add(comment);
        }

        public void Update(Comment comment)
        {
            _commentRepository.Update(comment);
        }
    }
}
