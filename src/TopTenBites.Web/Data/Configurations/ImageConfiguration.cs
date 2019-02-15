using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Data.Configurations
{   
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(x => x.ImageId);

            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GetDate()");
                   

        }
    }
}
