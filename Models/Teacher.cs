using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Teacher : Person
    {
        public string Specialization { get; set; }
        public DateTime TeacherFrom { get; set; }

    }
}
