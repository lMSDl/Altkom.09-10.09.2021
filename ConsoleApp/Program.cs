using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using NetTopologySuite.Geometries;
using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = GetContext())
            {
                //Wyczyszczenie bazy danych
                context.Database.EnsureDeleted();

                //Migracja bazy do najnowszej wersji
                context.Database.Migrate();

                for (int ii = 0; ii < 2; ii++)
                {
                    var order = new Order();
                    order.DateTime = DateTime.Now.AddDays(-15);
                    context.Add(order);
                    for (int i = 0; i < 5; i++)
                    {
                        var product = new Product();
                        if (i % 2 == 0)
                            product.Name = $"Produkt {i}";
                        product.Order = order;
                        product.ExpirationDate = DateTime.Now.AddDays(365);
                        context.Add(product);
                    }
                    order.Type = (OrderType)ii;
                }

                context.SaveChanges();

                //Mośliwość odczytu prywatnych pól za pośrednictwem EF.Property
                context.Set<Product>().Select(x => new { x.FullName, secret = EF.Property<string>(x, "_secret") }).ToList().ForEach(x => Console.WriteLine($"{x.FullName}: {x.secret}"));

                var products = context.Set<Product>().Where(x => x.Name == null).ToList();
                products.ForEach(x => x.Name = "new name");
                context.SaveChanges();

                //EF.Property może być używane tylko w zapytaniach EF LINQ. Poniższy zapis rzuci wyjątek.
                //Console.WriteLine(  EF.Property<string>(products.First(), "_secret") );

                //Dostęp RW dla prywatnego pola przez Entry
                Console.WriteLine(context.Entry(products.First()).Property("_secret").CurrentValue);

                products = context.Set<Product>().Take(1).ToList();
                context.Entry(products.First()).Property("_secret").CurrentValue = "My secret";

                context.SaveChanges();
            }

            using (var context = GetContext())
            {
                context.Entry(context.Set<Order>().Find(1)).Property("IsDeleted").CurrentValue = true;
                context.SaveChanges();
            }


            using (var context = GetContext())
            {
                context.Set<Order>()
                    //Filtrowanie usuniętych zamówień zastąpione przez filtr globalny
                    //.Where(x => !EF.Property<bool>(x, "IsDeleted"))

                    //W celu zignorowania filtrów globalnych stosujemy:
                    //.IgnoreQueryFilters()
                    .Select(x => x.Id).ToList().ForEach(x => Console.WriteLine($"OrderId: {x}"));

                //Zapytania mogące zwracać różne wyniki przy stosowaniu obowiązkowych pól z relacjami i filtrów globalnych
                context.Set<Product>().ToList().ForEach(x => Console.WriteLine($"1. ProductId: {x.Id}"));
                context.Set<Product>().Include(x => x.Order).ToList().ForEach(x => Console.WriteLine($"2. ProductId: {x.Id}"));

            }

            using (var context = GetContext())
            {
                var orders = context.Set<Order>().ToList();

                //Eager loading
                //var ordersEager = context.Set<Order>().Include(x => x.Products).ToList();

                //Explicit loading
                //context.Set<Order>().Load();
                //context.Set<Product>().Load();
                //var ordersExplicit = context.Set<Order>().ToList();

                //Lazy loading - jeśli poprawnie skonfigurowany
                Console.WriteLine(orders.First().Products.LastOrDefault()?.FullName ?? "Brak");
            }

            using (var context = GetContext())
            {
                //Wywołanie procedury składowej bez SELECT
                context.Database.ExecuteSqlRaw("ChangePrice @p0", 5);

                //Wywołanie procedusy z SELECT i materializacja do obiektu
                var orders = context.Set<OrderSummary>().FromSqlInterpolated($"GetOrderSummary {1}").ToList();

                orders = context.Set<OrderSummary>().ToList();

                var product = context.Set<Product>().Find(10);
                product.Name = product.Name + "_";

                //Pętla w celu ponownej opróby zapisu przy wystąpieniu  konfliktu współbieżności
                bool saved = false;
                while (!saved)
                    try
                    {
                        //Porównanie tokenów między OryginalValus a DatabaseValues
                        context.SaveChanges();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        foreach (var entry in e.Entries)
                        {
                            if (entry.Entity is Product)
                            {
                                var databaseValues = entry.GetDatabaseValues();

                                {
                                    //Opcjonalny blok, gdy chcemy zmienić currentValues. Jeśli mają być zachowane dane z currentValues, to możemy go pominąć.
                                    var currentValues = entry.CurrentValues;
                                    foreach (var property in currentValues.Properties)
                                    {
                                        var currentValue = currentValues[property];
                                        var databaseValue = databaseValues[property];

                                        currentValues[property] = currentValue;
                                    }
                                }

                                //Zastąpienie originalValues tymi z bazy danych w celu aktualizacji tokena współbieżności, aby ponownie nie wystąpił wyjątek
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                        }
                    }
            }


            using (var context = GetContext())
            {
                var person = new Person();
                context.Add(person);
                person = new Student();
                context.Add(person);
                person = new Teacher();
                context.Add(person);
                context.SaveChanges();

                person = context.Set<Person>().Find(2);
                person = context.Set<Teacher>().Find(2);
                person = context.Set<Student>().Find(1);

                Address address = new AddressInCity() { Location = new Point(53.2f, 25.8f) { SRID = 4326 }  };
                context.Add(address);
                address = new AddressInTown() { Location = new Point(52.2f, 27.8f) { SRID = 4326 } };
                context.Add(address);
                context.SaveChanges();

                var points = context.Set<Address>().Select(x => x.Location).ToList();

                var distance = points.First().Distance(points.Last());

                //Wywołanie prekompilowanego zapytania
                var products = Context.GetProductsForOrder(context, 2);
            }
        }

        private static Context GetContext()
        {
            return new Context("Server=(local);Database=EFCA;Integrated Security=true;");
        }
    }
}
