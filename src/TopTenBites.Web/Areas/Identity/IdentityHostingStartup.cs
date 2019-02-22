using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TopTenBites.Web.Data;

[assembly: HostingStartup(typeof(TopTenBites.Web.Areas.Identity.IdentityHostingStartup))]
namespace TopTenBites.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<ApplicationDBContext>(options =>
            //        options.UseSqlServer(context.Configuration["Data:TopTenBites:ConnectionString"]));

            //    services.AddDefaultIdentity<ApplicationUser>()
            //        .AddEntityFrameworkStores<ApplicationDBContext>();
            //});
        }
    }
}