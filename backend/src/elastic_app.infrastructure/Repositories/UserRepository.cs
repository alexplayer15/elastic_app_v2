using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;
using Microsoft.Extensions.Logging;


namespace elastic_app.infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IDynamoDBContext dynamoDbContext, ILogger<UserRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
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
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<UserModel?> GetUserDetailsAsync(string username)
        {
            var search = _dynamoDbContext.ScanAsync<UserModel>(new[]
            {
                new ScanCondition(nameof(UserModel.Username), ScanOperator.Equal, username)
            });

            var results = await search.GetRemainingAsync();
            return results.FirstOrDefault();
        }

        public async Task UpdateAsync(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null");
            }

            try
            {
                await _dynamoDbContext.SaveAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}");
            }
        }

        //think about replacing these calls with the lower level dynamoDbClient
        public async Task UpdateEmailVerificationAsync(Guid userId)
        {
            var search = _dynamoDbContext.ScanAsync<UserModel>(new[]
            {
                new ScanCondition(nameof(UserModel.Id), ScanOperator.Equal, userId)
            });

            var results = await search.GetRemainingAsync();

            var user = results.First();

            if (user != null)
            {
                user.EmailVerified = true;

                await _dynamoDbContext.SaveAsync(user);
            }
            else
            {
                _logger.LogError("User not found or already verified.");
            }
        }
    }
}
