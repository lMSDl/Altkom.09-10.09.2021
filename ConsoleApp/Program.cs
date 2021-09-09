using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(new DbContextOptionsBuilder().UseSqlServer("Server=(local);Database=EFCA;Integrated Security=true;").Options))
            {
                //Wyczyszczenie bazy danych
                context.Database.EnsureDeleted();

                //Migracja bazy do najnowszej wersji
                context.Database.Migrate();

                var order = new Order();
                order.DateTime = DateTime.Now.AddDays(-15);
                context.Add(order);
                context.Add(new Order());

                for (int i = 0; i < 10; i++)
                {
                    var product = new Product();
                    if (i % 2 == 0)
                        product.Name = $"Produkt {i}";
                    product.Order = order;
                    product.ExpirationDate = DateTime.Now.AddDays(365);
                    context.Add(product);
                }

                context.SaveChanges();

                //Mośliwość odczytu prywatnych pól za pośrednictwem EF.Property
                context.Set<Product>().Select(x => new {x.FullName, secret = EF.Property<string>(x, "_secret") }).ToList().ForEach(x => Console.WriteLine($"{x.FullName}: {x.secret}"));

                var products = context.Set<Product>().Where(x => x.Name == null).ToList();
                    products.ForEach(x => x.Name = "new name");
                context.SaveChanges();

                //EF.Property może być używane tylko w zapytaniach EF LINQ. Poniższy zapis rzuci wyjątek.
                //Console.WriteLine(  EF.Property<string>(products.First(), "_secret") );

                //Dostęp RW dla prywatnego pola przez Entry
                Console.WriteLine(context.Entry(products.First()).Property("_secret").CurrentValue);
                context.Entry(products.First()).Property("_secret").CurrentValue = "My secret";
                context.SaveChanges();
            }

            using (var context = new Context(new DbContextOptionsBuilder().UseSqlServer("Server=(local);Database=EFCA;Integrated Security=true;").Options))
            {
                context.Entry(context.Set<Order>().Find(1)).Property("IsDeleted").CurrentValue = true;
                context.SaveChanges();
            }
         }
    }
}
