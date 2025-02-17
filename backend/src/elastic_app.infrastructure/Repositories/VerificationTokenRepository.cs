using Amazon.DynamoDBv2.DataModel;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;

namespace elastic_app.infrastructure.Repositories
{
    public class VerificationTokenRepository : IVerificationTokenRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        public VerificationTokenRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }
        public async Task AddTokenAsync(TokenModel token)
        {
            try
            {
                await _dynamoDbContext.SaveAsync(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); //Add ILogger here later
            }
        }
    }
}
