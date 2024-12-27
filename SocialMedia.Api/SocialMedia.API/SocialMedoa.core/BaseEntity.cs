using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class BaseEntity
    {

        public BaseEntity()
        {
            InsertedOn = DateTime.UtcNow;
        }
        public long Id { get; set; }
        public DateTime InsertedOn { get; set; } = DateTime.UtcNow;
        public long? InsertedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long? ModifiedBy { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
