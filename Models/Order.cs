﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace Models
{
    public class Order : Entity, IEditedDateTime
    {
        private ILazyLoader _lazyLoader;
        private List<Product> products = new List<Product>();

        public Order()
        {
        }
        public Order(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public OrderType Type { get; set; }
        public DateTime DateTime { get; set; }
        public int DaysFromOrder { get; }
        public int Price { get; set; }

        //LazyLoading (proxy) - wymagane są właściwości wirtualne
        //public virtual List<Product> Products { get; set; } = new List<Product>();

        //LazyLoading (ILazyLoader) - wykrozystujemy wstrzyknięty serwis do ładowania właściwości
        public List<Product> Products { get => _lazyLoader.Load(this, ref products); set => products = value; }


        public DateTime EditedDateTime { get; set; }
    }
}
