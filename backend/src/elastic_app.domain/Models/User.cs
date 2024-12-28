using Amazon.DynamoDBv2.DataModel;

namespace elastic_app.domain.Models
{
    public class User
    {
        [DynamoDBHashKey("id")]
        public Guid Id { get; set; }

        [DynamoDBProperty("forename")]
        public string Forename { get; set; } = string.Empty;
        [DynamoDBProperty("surname")]
        public string Surname { get; set; } = string.Empty;
        [DynamoDBProperty("username")]
        public string Username { get; set; } = string.Empty;
        [DynamoDBProperty("email")]
        public string Email { get; set; } = string.Empty;
        [DynamoDBProperty("password")]
        public string PasswordHash { get; set; }
        [DynamoDBProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
