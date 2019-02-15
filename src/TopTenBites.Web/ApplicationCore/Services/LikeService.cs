using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class LikeService : ILikeService
    {
        private IGenericRepository<Like> _likeRepository;

        public LikeService(IGenericRepository<Like> likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public Like Get(int likeId)
        {
            return _likeRepository.GetById(likeId);
        }

        public IEnumerable<Like> GetAll(string likeIds = "")
        {
            IEnumerable<Like> likes = Enumerable.Empty<Like>();

            int[] aryLikeIds = likeIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            if (aryLikeIds?.Length > 0)
            {
                likes = _likeRepository.GetAll(filter: x => aryLikeIds.Contains((int)x.LikeId));
            }
            else
            {
                likes = _likeRepository.GetAll();
            }

            return likes;
        }

    }
}
