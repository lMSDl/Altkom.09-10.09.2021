using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    class OrderSummaryConfiguration : IEntityTypeConfiguration<OrderSummary>
    {
        public void Configure(EntityTypeBuilder<OrderSummary> builder)
        {
            //Obiekt nie posiada klucza głownego
            builder.HasNoKey();

            //Nie chcemy tworzyć tabeli dla obiektu
            //builder.ToTable(null);

            builder.ToView("View_OrderSummary");
        }
    }
}
