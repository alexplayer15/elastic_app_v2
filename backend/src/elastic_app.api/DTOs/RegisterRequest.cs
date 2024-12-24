namespace elastic_app.api.DTOs
{
    public class RegisterRequest
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public string ReEnterPassword { get; set; }

    }
}
