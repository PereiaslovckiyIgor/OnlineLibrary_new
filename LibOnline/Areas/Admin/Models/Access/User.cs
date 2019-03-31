using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Access
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int IdRole { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("Roles", Schema = "access")]
    public class Role {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
