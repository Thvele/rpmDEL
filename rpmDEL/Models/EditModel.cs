using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class EditModel
    {
        public IEnumerable<user> users { get; set; }

        public IEnumerable<user> files { get; set; }
    }
}
