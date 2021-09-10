using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //[Keyless]
    public class OrderSummary
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
