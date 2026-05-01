using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Visa;

namespace TN.Shared.Infrastructure.EntityConfiguration
{
    public class VisaApplicationConfig : IEntityTypeConfiguration<VisaApplication>
    {
        public void Configure(EntityTypeBuilder<VisaApplication> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.Property(x => x.CountryId)
            //       .HasMaxLength(100)
            //       .IsRequired();
        }
    }
}
