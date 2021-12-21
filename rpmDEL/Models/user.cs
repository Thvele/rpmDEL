using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class user
    {
        [Key]
        public int _ID { get; set; }
        public string _imgpath { get; set; }
        public string _Fam { get; set; }
        public string _Name { get; set; }
        public string _Surname { get; set; }
        public DateTime _regday { get; set; }
        public string _Description { get; set; }
        public string _login { get; set; }
        public string _pass { get; set; }

        [EmailAddress]
        public string _EMail { get; set; }

        public int? RoleId { get; set; }
        public role Role { get; set; }
    }

    public enum SortState
    {
        IdAsc,
        IdDesc,
        EmailAsc,
        EmailDesc,
        LoginAsc,
        LoginDesc
    }
}
