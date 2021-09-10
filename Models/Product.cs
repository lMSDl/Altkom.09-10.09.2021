using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Models
{
    public class Product : Entity, IEditedDateTime
    {
        private ILazyLoader _lazyLoader;

        public Product()
        {
        }
        public Product(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

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

        //LazyLoading (proxy) - wymagane są właściwości wirtualne
        //public virtual Order Order { get; set; }

        //LazyLoading (ILazyLoader) - wykrozystujemy wstrzyknięty serwis do ładowania właściwości
        public Order Order { get => _lazyLoader.Load(this, ref order); set => order = value; }

        //Prywatne pole, do którego dostęp będziemy uzyskiwali tylko dzięli EFCore.
        private string _secret;
        private Order order;


        public DateTime LastEdited { get; set; }

        public byte[] Timestamp { get; set; }
        public DateTime EditedDateTime { get; set; }
    }
}
