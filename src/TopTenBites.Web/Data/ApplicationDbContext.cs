using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.Data.Configurations;

namespace TopTenBites.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Business> Businesses { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Like> Likes { get; set; }

        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BusinessConfiguration())
                        .ApplyConfiguration(new MenuItemConfiguration())
                        .ApplyConfiguration(new LikeConfiguration())
                        .ApplyConfiguration(new CommentConfiguration())
                        .ApplyConfiguration(new ImageConfiguration());

        }
    }
}
