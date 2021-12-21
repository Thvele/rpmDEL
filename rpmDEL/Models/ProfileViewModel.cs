using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class ProfileViewModel
    {
        public IEnumerable<user> users { get; set; }
        public IEnumerable<Post> posts { get; set; }
    }
}
