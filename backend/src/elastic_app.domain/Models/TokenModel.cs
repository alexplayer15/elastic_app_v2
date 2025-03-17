using Amazon.DynamoDBv2.DataModel;

namespace elastic_app.domain.Models
{
    [DynamoDBTable(DynamoDbConstants.TokenData)]
    public record TokenModel
    {
        [property: DynamoDBHashKey("id")] public Guid Id { get; set; }
        [property: DynamoDBProperty("userId")] public Guid UserId { get; set; }
        [property: DynamoDBProperty("token")] public string Token { get; set; }
        [property: DynamoDBProperty("createdAt")] public DateTime CreatedAt { get; set; }
        [property: DynamoDBProperty("expiresAt")] public DateTime ExpiresAt { get; set; }
    }
}
