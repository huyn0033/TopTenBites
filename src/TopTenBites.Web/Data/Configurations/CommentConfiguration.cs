using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Data.Configurations
{   
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments")
                   .HasOne(x => x.MenuItem)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.MenuItemId);
            
            builder.HasKey(x => x.CommentId);

            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GetDate()");
        }
    }
}
