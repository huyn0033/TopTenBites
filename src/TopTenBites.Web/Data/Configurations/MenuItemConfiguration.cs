using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Data.Configurations
{   
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems")
                   .HasOne(x => x.Business)
                   .WithMany(x => x.MenuItems)
                   .HasForeignKey(x => x.BusinessId);
                   
            builder.HasKey(x => x.MenuItemId);

            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GetDate()");

        }
    }
}
