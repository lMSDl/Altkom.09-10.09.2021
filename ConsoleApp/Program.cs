using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using System;

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
                context.Add(order);

                var product = new Product();
                context.Add(product);

                context.SaveChanges();
            }
        }
    }
}
