using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class Post
    {
        [Key]
        public int id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string img4 { get; set; }
        public string img5 { get; set; }
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public user user_ { get; set; }
    }
}
