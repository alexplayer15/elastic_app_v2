using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using elastic_app.api.Models;

namespace elastic_app.api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DynamoDBContext _dbContext;
        public UserRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _dbContext = new DynamoDBContext(dynamoDbClient);
        }
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var search = _dbContext.ScanAsync<User>(new[] {
                new ScanCondition(nameof(User.Email), ScanOperator.Equal, email)
            });

            var results = await search.GetRemainingAsync();
            return results.Count > 0;
        }
        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            var search = _dbContext.ScanAsync<User>(new[] {
                new ScanCondition(nameof(User.Username), ScanOperator.Equal, username)
            });

            var results = await search.GetRemainingAsync();
            return results.Count > 0;
        }
        public async Task AddUserAsync(User user)
        {
            await _dbContext.SaveAsync(user);
        }
    }
}
