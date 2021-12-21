using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class FilterViewModel
    {
        public int? SelectID { get; private set; }
        public string SelectLogin { get; private set; }
        public string SelectEmail { get; private set; }

        public FilterViewModel(int? id, string login, string email)
        {
            SelectID = id;
            SelectLogin = login;
            SelectEmail = email;
        }
    }
}
