using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class LoginViewModel
    {
        public IEnumerable<user> users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
