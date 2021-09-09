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
                .HasComputedColumnSql($"DATEDIFF(DAY,[{nameof(DateTime)}],GETDATE())");

            //Generowanie wartości z innych tabel nie jest możliwe
            //builder.Property(x => x.Price)
            //    .HasComputedColumnSql($"SUM(Price) FROM dbo.Product WHERE OrderId = {nameof(Order.Id)}");

            //ShadowProperty - kolumna w bazie danych, która nie ma odpowiednika w modelu
            builder.Property<bool>("IsDeleted");

            //Filtr globalny - zapytanie LINQ WHERE, które dołączane jest do każdego zapytania na tabeli
            builder.HasQueryFilter(x => !EF.Property<bool>(x, "IsDeleted") && x.DaysFromOrder <= 15);
            //Jeśli chcemy użyć wielu warunków na jednej tabeli, musimy je zawrzeć w filtrze. Poniższa instrukcja połączona z powyższą będzie powodowała błąd.
            //builder.HasQueryFilter(x => x.DaysFromOrder < 15);
        }
    }
}
