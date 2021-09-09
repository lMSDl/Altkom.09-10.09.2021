using System;
using System.Collections.Generic;

namespace Models
{
    public class Order : Entity
    {
        public OrderType Type { get; set; }
        public DateTime DateTime { get; set; }
        public int DaysFromOrder { get; }
        public int Price { get; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
