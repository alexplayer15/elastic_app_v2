using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentValidation;
using Amazon.DynamoDBv2.DataModel;
using elastic_app.application.Services.User;
using elastic_app.common.tests.Builders;
using elastic_app.application.DTOs;
using elastic_app.application.Validations;
using elastic_app.infrastructure.Config;
using elastic_app.domain.Abstractions;
using elastic_app.infrastructure.Repositories;
using elastic_app.domain.Models;

namespace elastic_app.integration.tests
{
    public class ServiceIntegrationTests
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterRequest> _registerRequestValidator;
        private readonly IDynamoDBContext _mockDynamoDbContext;

        public ServiceIntegrationTests()
        {
            _mockDynamoDbContext = Substitute.For<IDynamoDBContext>();
            _registerRequestValidator = new RegisterRequestValidation();
            _userRepository = new UserRepository(_mockDynamoDbContext);
            _userService = new UserService(_userRepository, _registerRequestValidator);
        }

        [Fact]
        public async Task WhenAUserEntersValidRegistrationDetails_ShouldRegisterUserAsync()
        {
            //Arrange
            var registrationDetails = new RegisterRequestBuilder().WithValidRegistrationDetails(true).Build();

            var emailSearch = Substitute.For<AsyncSearch<UserModel>>();
            emailSearch.GetRemainingAsync().Returns(Task.FromResult(new List<UserModel>()));

            var usernameSearch = Substitute.For<AsyncSearch<UserModel>>();
            usernameSearch.GetRemainingAsync().Returns(Task.FromResult(new List<UserModel>()));

            _mockDynamoDbContext.ScanAsync<UserModel>(
                Arg.Is<IEnumerable<ScanCondition>>(conditions =>
                    conditions.Any(c => c.PropertyName == nameof(UserModel.Email) &&
                                        c.Values.Contains(registrationDetails.Email))
                )
            ).Returns(emailSearch);

            _mockDynamoDbContext.ScanAsync<UserModel>(
                Arg.Is<IEnumerable<ScanCondition>>(conditions =>
                    conditions.Any(c => c.PropertyName == nameof(UserModel.Username) &&
                                        c.Values.Contains(registrationDetails.Username))
                )
            ).Returns(usernameSearch);

            //Act
            await _userService.RegisterUserAsync(registrationDetails);

            //Assert
            await _mockDynamoDbContext.Received(1).SaveAsync(Arg.Is<UserModel>(user =>
                  user.Forename == registrationDetails.Forename &&
                  user.Surname == registrationDetails.Surname &&
                  user.Username == registrationDetails.Username &&
                  user.Email == registrationDetails.Email &&
                  user.CreatedAt.Date == DateTime.UtcNow.Date));
        }
    }
}