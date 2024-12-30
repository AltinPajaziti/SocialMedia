using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class Roles
    {
        [Key]
        public int ID { get; set; }
        public string RoleName { get; set; }
        public ICollection<User> User { get; set; }
    }
}
