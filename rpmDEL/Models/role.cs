using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<user> Users { get; set; }
        public role()
        {
            Users = new List<user>();
        }
    }
}
