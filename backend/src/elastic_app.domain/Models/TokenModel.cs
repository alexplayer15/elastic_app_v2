using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elastic_app.domain.Models
{
    [DynamoDBTable("TokenData")]
    public record TokenModel
    {
        [property: DynamoDBHashKey("id")] public Guid Id { get; set; }
        [property: DynamoDBProperty("userId")] public Guid UserId { get; set; }
        [property: DynamoDBProperty("token")] public string Token { get; set; }
        [property: DynamoDBProperty("createdAt")] public DateTime CreatedAt { get; set; }
        [property: DynamoDBProperty("expiresAt")] public DateTime ExpiresAt { get; set; }
    }
}
