using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class FileModel
    {
        [Key]
        public int id { get; set; }
        public string _name { get; set; }
        public string _path { get; set; }
    }
}
