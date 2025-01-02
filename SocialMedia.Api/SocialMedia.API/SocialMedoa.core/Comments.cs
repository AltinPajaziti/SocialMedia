using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class Comments : BaseEntity
    {

        public string Content { get; set; }

        public bool IsActive { get; set; }

        public Post posts { get; set; }
        public long PostId { get; set; }    
    }
}
