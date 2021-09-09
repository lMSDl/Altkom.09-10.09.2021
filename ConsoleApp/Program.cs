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

                for (int i = 0; i < 10; i++)
                {
                    var product = new Product();
                    if (i % 2 == 0)
                        product.Name = $"Produkt {i}";
                    context.Add(product);
                }

                context.SaveChanges();

                context.Set<Product>().ToList().ForEach(x => Console.WriteLine(x.FullName));

                context.Set<Product>().Where(x => x.Name == null).ToList().ForEach(x => x.Name = "new name");
                context.SaveChanges();
            }
        }
    }
}
