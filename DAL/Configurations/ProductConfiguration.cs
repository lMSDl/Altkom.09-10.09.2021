using Laraue.EfCoreTriggers.Common.Extensions;
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
            //FiltryGlobalne - ustawienie powiązania między obiektami na obowiązkowe może powodować różne wyniki przy stosowaniu filrtów globalnych.
            //W celu uniknięcia tego problemu, nalezy "poluzować" relacje (pole opcjonalne) lub zastosować dodatkowy filtr globalny z "drugiej strony" (jak poniżej)
            builder.HasOne(x => x.Order).WithMany(x => x.Products).IsRequired();
            builder.HasQueryFilter(x => !EF.Property<bool>(x.Order, "IsDeleted") && x.Order.DaysFromOrder <= 15);

            builder.Property(x => x.Category).IsRequired()
            //Ustawianie sztywnej wartości jako domyślna dla właściwości
                .HasDefaultValue("N/A");

            //Ustawienie wartości domyślnej na podstawie sekwencji
            builder.Property(x => x.Price)
                .HasDefaultValueSql("NEXT VALUE FOR sequences.ProductPrice");

            //Generowanie wartości, która będzie przechowywana w bazie danych i aktualizowana przy zmianie właściwości zależnych
            builder.Property(x => x.FullName)
                .HasComputedColumnSql($"[{nameof(Product.Category)}] + ': ' + [{nameof(Product.Name)}]", stored: true);

            //Konfiguracja pola zapasowego. Domyślnie EFCore korzysta z pola zapasowego, jeśli jest dostępne.
            builder.Property(x => x.ExpirationDate).HasField("n_expirationDate")
                .UsePropertyAccessMode(PropertyAccessMode.PreferField);

            //Konfiguracja pola zapasowego bez właściwości (Property)
            builder.Property("_secret").HasDefaultValueSql("NEWID()");

            //https://github.com/win7user10/Laraue.EfCoreTriggers
            //builder.AfterUpdate(trigget => trigget.Action(
            //    action => action.Update<Product>(
            //        (old, updated, person) => person.Id == old.Id,
            //        (old, updated, person) => new Product { LastEdited = DateTime.Now  })
            //    )
            //);
        }
    }
}
