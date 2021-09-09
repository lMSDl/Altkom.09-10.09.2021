using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            //Stosowanie konwersji

            //Wykorzystanie konwertera wbudowanego enum -> string
            //builder.Property(x => x.Type).HasConversion<string>();

            //Ręczna konfiguracja konwesji
            /*builder.Property(x => x.Type).HasConversion(
                x => x.ToString(),
                x => (OrderType)Enum.Parse(typeof(OrderType), x));*/

            //Zastosowanie predefiniowanego konwertera
            builder.Property(x => x.Type).HasConversion(EnumToBase64Converter<OrderType>());
            //Dodatkowe obostrzenia tabeli przeniesione zostały do konwertera
                //.HasMaxLength(20)
                //.IsUnicode(false);
        }

        public ValueConverter<T, string> EnumToBase64Converter<T>() where T : Enum =>
            new ValueConverter<T, string>(
            x => Convert.ToBase64String(Encoding.UTF8.GetBytes(x.ToString()).Reverse().ToArray()),
            x => (T)Enum.Parse(typeof(T), Encoding.UTF8.GetString(Convert.FromBase64String(x).Reverse().ToArray())),
            new ConverterMappingHints(size: 20, unicode: false)
            );
    }
}
