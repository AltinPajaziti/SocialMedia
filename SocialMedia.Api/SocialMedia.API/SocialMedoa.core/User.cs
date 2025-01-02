using System.Data;

namespace SocialMedoa.core
{
    public class User : BaseEntity
    {

  



        public string Name { get; set; }
        public string Surname { get; set; }

        public string Adress { get; set; }


        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int Roleid { get; set; }

        public Roles Roli { get; set; }

        public string Email { get; set; }


        public string RefreshToken { get; set; } = string.Empty;

        public bool IsPrivate { get; set; }

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }


        public ICollection<Post> Posts { get; set; }

        public ICollection<FollowRequests> SentFollowRequests { get; set; } 
        public ICollection<FollowRequests> ReceivedFollowRequests { get; set; }




    }
}



