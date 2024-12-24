namespace elastic_app.api.Models
{
    public class User
    {
        public Guid Id { get; set; } 
        public string ForeName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
