using System.Collections.Generic;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface ILikeService
    {
        Like Get(int likeId);
        IEnumerable<Like> GetAll(string likeIds = "");
    }
}
