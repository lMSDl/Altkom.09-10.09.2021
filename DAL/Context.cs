using DAL.Configurations;
using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

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
        public Context()
        {
        }

        public Context([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            //Opcjonalnie, można sprawdzić czy Context został już skonfigurowany
            if(!optionsBuilder.IsConfigured)
            //Określamy z jakiego dostawcy bazy danych będziemu korzystać
                optionsBuilder.UseSqlServer();

            optionsBuilder.UseSqlServerTriggers();
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
        }
    }
}
