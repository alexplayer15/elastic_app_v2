using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;

namespace elastic_app.infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        public UserRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var search = _dynamoDbContext.ScanAsync<UserModel>(new[] {
                new ScanCondition(nameof(UserModel.Email), ScanOperator.Equal, email)
            });

            var results = await search.GetRemainingAsync();
            return results.Count > 0;
        }
        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            var search = _dynamoDbContext.ScanAsync<UserModel>(new[] {
                new ScanCondition(nameof(UserModel.Username), ScanOperator.Equal, username)
            });

            var results = await search.GetRemainingAsync();
            return results.Count > 0;
        }
        public async Task AddUserAsync(UserModel user)
        {
            try
            {
               await _dynamoDbContext.SaveAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); //Add ILogger here later
            }
            
        }
    }
}
