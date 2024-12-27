namespace SocialMedoa.core
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }

    }
}



