using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using NSubstitute;
using elastic_app.domain.Abstractions;
using elastic_app.domain.Models;
using elastic_app.infrastructure.Repositories;
using elastic_app.common.tests.Builders;


namespace elastic_app.unit.tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IDynamoDBContext _mockDynamoDbContext;

        public UserRepositoryTests()
        {
            _mockDynamoDbContext = Substitute.For<IDynamoDBContext>();
            _userRepository = new UserRepository(_mockDynamoDbContext);
        }

        [Fact]
        public async Task WhenICheckForAnExistingEmail_CheckEmailExistsAsyncShouldReturnTrue()
        {
            //Arrange
            string email = "existingEmail@example.com";

            List<UserModel> userList = new List<UserModel>()
            {
                new UserModelBuilder().WithEmail(email).Build()
            };

            var mockAsyncSearch = Substitute.For<AsyncSearch<UserModel>>();

            _mockDynamoDbContext.ScanAsync<UserModel>(Arg.Any<IEnumerable<ScanCondition>>())
                .Returns(mockAsyncSearch);

            mockAsyncSearch.GetRemainingAsync().Returns(Task.FromResult(userList));

            //Act
            bool emailExists = await _userRepository.CheckEmailExistsAsync(email);

            //Assert
            emailExists.Should().BeTrue();
        }

        [Fact]
        public async Task WhenICheckForANonExistingEmail_CheckEmailExistsAsyncShouldReturnFalse()
        {
            //Arrange
            string email = "nonExistingEmail@example.com";

            List<UserModel> userList = new List<UserModel>()
            {

            };

            var mockAsyncSearch = Substitute.For<AsyncSearch<UserModel>>();

            _mockDynamoDbContext.ScanAsync<UserModel>(Arg.Any<IEnumerable<ScanCondition>>())
                .Returns(mockAsyncSearch);

            mockAsyncSearch.GetRemainingAsync().Returns(Task.FromResult(userList));

            //Act
            bool emailExists = await _userRepository.CheckEmailExistsAsync(email);

            //Assert
            emailExists.Should().BeFalse();
        }


        [Fact]

        public async Task WhenICheckForAnExistingUsername_CheckUsernameExistsAsyncShouldReturnTrue()
        {
            //Arrange
            string username = "existingUser";

            List<UserModel> userList = new List<UserModel>()
            {
                new UserModelBuilder().WithUsername(username).Build()
            };

            var mockAsyncSearch = Substitute.For<AsyncSearch<UserModel>>();

            _mockDynamoDbContext.ScanAsync<UserModel>(Arg.Any<IEnumerable<ScanCondition>>())
                .Returns(mockAsyncSearch);

            mockAsyncSearch.GetRemainingAsync().Returns(Task.FromResult(userList));

            //Act
            bool userExists = await _userRepository.CheckUsernameExistsAsync(username);

            //Assert
            userExists.Should().BeTrue();

        }

        [Fact]

        public async Task WhenICheckForANonExistingUsername_CheckUsernameExistsAsyncShouldReturnFalse()
        {
            //Arrange
            string username = "existingUser";

            List<UserModel> userList = new List<UserModel>()
            {

            };

            var mockAsyncSearch = Substitute.For<AsyncSearch<UserModel>>();

            _mockDynamoDbContext.ScanAsync<UserModel>(Arg.Any<IEnumerable<ScanCondition>>())
                .Returns(mockAsyncSearch);

            mockAsyncSearch.GetRemainingAsync().Returns(Task.FromResult(userList));

            //Act
            bool userExists = await _userRepository.CheckUsernameExistsAsync(username);

            //Assert
            userExists.Should().BeFalse();
        }

        [Fact]

        public async Task WhenValidUserDetailsAreSubmitted_AddUserAsyncShouldAddTheUserToTheDb()
        {
            //Arrange
            var user = new UserModelBuilder().Build();
            _mockDynamoDbContext.SaveAsync(Arg.Any<UserModel>()).Returns(Task.CompletedTask);

            //Act
            await _userRepository.AddUserAsync(user);

            //Assert
            await _mockDynamoDbContext.Received(1).SaveAsync(user);
        }
    }
}
