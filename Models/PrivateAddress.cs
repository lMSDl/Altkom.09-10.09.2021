using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AddressInTown : Address
    {
        public string Town { get; set; }
        public DateTime FundedIn { get; set; }
    }
}
