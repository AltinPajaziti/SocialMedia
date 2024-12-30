using System.Data;

namespace SocialMedoa.core
{
    public class User : BaseEntity
    {

  



        public string Name { get; set; }
        public string Surname { get; set; }

        public string Adress { get; set; }


        public string Bio { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int Roleid { get; set; }

        public Roles Roli { get; set; }

        public string Email { get; set; }


        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }



    }
}



