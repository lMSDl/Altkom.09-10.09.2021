using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Category { get; set; }

        public string FullName { get; }

        public DateTime ExpirationDate { get; set; }

        public int Price { get; set; }

        public Order Order { get; set; }
    }
}
