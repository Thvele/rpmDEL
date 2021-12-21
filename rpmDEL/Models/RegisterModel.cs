using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string _login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string _pass { get; set; }

        [DataType(DataType.Password)]
        [Compare("_pass", ErrorMessage = "Пароль введен неверно")]
        public string _Rpass { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Не указан Email")]
        public string _EMail { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string _Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string _Fam { get; set; }

        public string _Surname { get; set; }

    }
}
