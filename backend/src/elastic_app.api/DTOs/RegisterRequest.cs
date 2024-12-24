namespace elastic_app.api.DTOs
{
    public class RegisterRequest
    {
        public string Forename { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public string ReEnterPassword { get; set; }

    }
}
