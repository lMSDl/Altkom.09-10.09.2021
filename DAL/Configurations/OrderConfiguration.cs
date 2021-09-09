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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.DateTime)
            //Ustawianie zapytania SQL, którego wynik ma być wartością domyślną dla właściwości (SELECT niepotrzebny)
                .HasDefaultValueSql("GETDATE()");

            //Generowanie "dynamiczne" wartości, które nie są przechowywane w bazie, a obliczane przy próbie pobrania
            //Watoście zmienne w czasie nie mogą być przechowywane w bazie (stred = false)
            builder.Property(x => x.DaysFromOrder)
                .HasComputedColumnSql($"DATEDIFF(SECOND,[{nameof(DateTime)}],GETDATE())");
        }
    }
}
