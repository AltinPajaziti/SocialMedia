using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public User Users { get; set; }
        public long UserId { get; set; }    

        public ICollection<Likes> Likes { get; set; }

    }
}
