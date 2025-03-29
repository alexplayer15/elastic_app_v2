using Amazon.DynamoDBv2.DataModel;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;
using Amazon.DynamoDBv2.DocumentModel;

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
                throw new Exception(ex.Message);
            }
        }

        public async Task<TokenModel> GetTokenData(string token)
        {
            var search = _dynamoDbContext.ScanAsync<TokenModel>(new[]
            {
                new ScanCondition(nameof(TokenModel.Token), ScanOperator.Equal, token)
            });

            var result = await search.GetRemainingAsync();

            return result.First();
        }
    }
}
