using DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;

namespace DAL
{
    /*PowerShell:
     * dotnet tool install --global dotnet-ef (instalacja narzędzi dla EFCore)
     * dotnet ef (potwierdzenie, że narzędzia zostały zainstalowane)
     * 
     * dotnet ef migrations add <nazwa migracji> (dodanie nowej migracji)
     * dotnet ef migrations remove [-f] (usunięcie ostatniej migracji, opcjonalny parametr -f jako wymuszenie przy braku połączenia z bazą danych)
     * dotnet ef database update (aktualizacja bazy danych)
    */

    public sealed class Context : DbContext
    {
        private string _connectionString;
        public Context()
        {
        }

        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //Opcjonalnie, można sprawdzić czy Context został już skonfigurowany
            //if(!optionsBuilder.IsConfigured)
            //Określamy z jakiego dostawcy bazy danych będziemu korzystać
            if (_connectionString == null)
                optionsBuilder.UseSqlServer(x => x.UseNetTopologySuite());
            else
                optionsBuilder.UseSqlServer(_connectionString, x => x.UseNetTopologySuite());

            //LazyLoading (proxy) - włączenie proxy
            //.UseLazyLoadingProxies()

            //.UseSqlServerTriggers();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Dodawanie konfiguracji encji pojedynczo
            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            //Automatyczne dodawanie wszystkich konfiguracji ze wskazanego assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);

            //Zadeklarowanie nowej sekwencji (schema opcjonalna)
            modelBuilder.HasSequence<int>("ProductPrice", "sequences")
                //Opcjonalna konfiguracja zachowania sekwencji
                .StartsAt(5)
                .HasMax(10)
                .HasMin(1)
                .IncrementsBy(2)
                .IsCyclic();

            //Własne konwencje
            //https://github.com/dotnet/efcore/issues/214
            modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(Person).IsAssignableFrom(x.ClrType))
                .SelectMany(x => x.GetProperties())
                .Where(x => x.PropertyInfo?.PropertyType == typeof(DateTime))
                .ToList()
                .ForEach(x => x.SetColumnType("datetime2(4)"));

        }

        public override int SaveChanges()
        {
            var now = DateTime.Now;
            ChangeTracker.Entries<IEditedDateTime>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ToList()
                .ForEach(x => x.EditedDateTime = now);

            return base.SaveChanges();
        }
    }
}
