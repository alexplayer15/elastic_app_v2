using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace elastic_app.domain.Models
{
    [DynamoDBTable("UserData")]
    public record User
    {
        [property: DynamoDBHashKey("id")] public Guid Id { get; set; }
        [property: DynamoDBProperty("forename")] public string Forename { get; set; }
        [property: DynamoDBProperty("surname")] public string Surname { get; set; }
        [property: DynamoDBProperty("username")] public string Username { get; set; }
        [property: DynamoDBProperty("email")] public string Email { get; set; }
        [property: DynamoDBProperty("password")] public string PasswordHash { get; set; }
        [property: DynamoDBProperty("createdAt")] public DateTime CreatedAt { get; set; }
    }
}
