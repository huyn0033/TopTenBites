using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Data.Configurations
{   
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes")
                   .HasOne(x => x.MenuItem)
                   .WithMany(x => x.Likes)
                   .HasForeignKey(x => x.MenuItemId);
            
            builder.HasKey(x => x.LikeId);

            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GetDate()");
        }
    }
}
