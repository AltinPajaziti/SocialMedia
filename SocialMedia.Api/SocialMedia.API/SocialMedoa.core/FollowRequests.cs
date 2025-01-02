using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class FollowRequests : BaseEntity
    {
        public long SenderId { get; set; }
        public User Sender { get; set; } 
        public long ReceiverId { get; set; }
        public User Receiver { get; set; } 

        public FollowRequestStatus Status { get; set; } = FollowRequestStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    }

    public enum FollowRequestStatus
    {
        Pending,
        Accepted,
        Declined
    }
}
