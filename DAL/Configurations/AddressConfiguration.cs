using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            //builder.OwnsOne(x => x.Location);
            /*builder.Property(x => x.Location).HasConversion(
                x => new Point(x.Longitude, x.Latitude) { SRID = 4326 },
                x => new Models.Location() { Longitude = (float)x.X, Latitude = (float)x.Y }
                );*/
        }
    }

    public class AddressInTownConfiguration : IEntityTypeConfiguration<AddressInTown>
    {
        public void Configure(EntityTypeBuilder<AddressInTown> builder)
        {
            builder.ToTable(nameof(AddressInTown));
            builder.Property(x => x.FundedIn).HasColumnName("In");
        }
    }
    public class AddressInCityConfiguration : IEntityTypeConfiguration<AddressInCity>
    {
        public void Configure(EntityTypeBuilder<AddressInCity> builder)
        {
            builder.ToTable(nameof(AddressInCity));
            builder.Property(x => x.BuildedIn).HasColumnName("In");
        }
    }
}
