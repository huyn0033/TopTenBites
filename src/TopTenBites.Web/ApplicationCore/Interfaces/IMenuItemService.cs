using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IMenuItemService
    {
        MenuItem Get(int commentId);
        IEnumerable<MenuItem> GetAll(string menuItemIds = "");
        MenuItem Add(MenuItem menuItem);
        void Update(MenuItem menuItem);
    }
}
