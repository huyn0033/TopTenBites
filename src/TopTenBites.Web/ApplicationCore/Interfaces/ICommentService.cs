using System.Collections.Generic;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface ICommentService
    {
        Comment Get(int commentId);
        IEnumerable<Comment> GetAll(string commentIds = "");
        Comment Add(Comment comment);
        void Update(Comment comment);
    }
}
