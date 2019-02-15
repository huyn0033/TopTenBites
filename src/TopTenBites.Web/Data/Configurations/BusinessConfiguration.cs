using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Data.Configurations
{   
    public class BusinessConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.ToTable("Businesses");

            builder.HasKey(x => x.BusinessId);

            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GetDate()");
                   

        }
    }
}
