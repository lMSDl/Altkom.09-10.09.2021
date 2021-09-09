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
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Category).IsRequired()
            //Ustawianie sztywnej wartości jako domyślna dla właściwości
                .HasDefaultValue("N/A");

            //Ustawienie wartości domyślnej na podstawie sekwencji
            builder.Property(x => x.Price)
                .HasDefaultValueSql("NEXT VALUE FOR sequences.ProductPrice");

            //Generowanie wartości, która będzie przechowywana w bazie danych i aktualizowana przy zmianie właściwości zależnych
            builder.Property(x => x.FullName)
                .HasComputedColumnSql($"[{nameof(Product.Category)}] + ': ' + [{nameof(Product.Name)}]", stored: true);
        }
    }
}
