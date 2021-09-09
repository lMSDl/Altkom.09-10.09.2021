using System;

namespace Models
{
    public class Product : Entity
    {
        //Domyślne konwencjie, po których EFCore dopasowuje pola zapasowe (bez potrzeby konfiguracji)
        //private DateTime expirationDate;
        //private DateTime _expirationDate;
        //private DateTime m_expirationDate;

        private DateTime n_expirationDate;

        public string Name { get; set; }
        public string Category { get; set; }

        public string FullName { get; }

        //Konfiguracja pola zapasowego za pomocą adnotacji
        //[BackingField(nameof(n_expirationDate))]
        public DateTime ExpirationDate
        {
            get => new DateTime(2016, 11, 2);
            set => n_expirationDate = value;
        }

        public int Price { get; set; }

        public Order Order { get; set; }
    }
}
