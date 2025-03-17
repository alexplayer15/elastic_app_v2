using Amazon.DynamoDBv2.DataModel;

namespace elastic_app.domain.Models
{
    [DynamoDBTable(DynamoDbConstants.UserData)]
    public record UserModel
    {
        [property: DynamoDBHashKey("id")] public Guid Id { get; set; }
        [property: DynamoDBProperty("forename")] public string Forename { get; set; } = string.Empty;
        [property: DynamoDBProperty("surname")] public string Surname { get; set; } = string.Empty;
        [property: DynamoDBProperty("username")] public string Username { get; set; } = string.Empty;
        [property: DynamoDBProperty("email")] public string Email { get; set; } = string.Empty;
        [property: DynamoDBProperty("emailVerified")] public bool EmailVerified { get; set; } = false;
        [property: DynamoDBProperty("password")] public string PasswordHash { get; set; }
        [property: DynamoDBProperty("createdAt")] public DateTime CreatedAt { get; set; }
    }
}
