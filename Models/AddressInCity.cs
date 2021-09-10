using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AddressInCity : Address
    {
        public string City { get; set; }

        public DateTime BuildedIn { get; set; }
    }
}
