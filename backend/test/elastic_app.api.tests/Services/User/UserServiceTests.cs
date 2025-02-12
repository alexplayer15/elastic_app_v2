using NSubstitute;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using elastic_app.application.Services.User;
using elastic_app.domain.Abstractions;
using elastic_app.domain.Models;
using elastic_app.application.DTOs;
using elastic_app.common.tests.Builders;


namespace elastic_app.unit.tests.Services.User
{
    public class UserServiceTests
    {
        private readonly IUserRepository _mockUserRepository;
        private readonly IUserService _userService;
        public static readonly System.ComponentModel.DataAnnotations.ValidationResult? Success;

        public UserServiceTests()
        {
            _mockUserRepository = Substitute.For<IUserRepository>();
            _userService = new UserService(_mockUserRepository);
        }

        [Fact]
        public async Task WhenRegistrationDetailsAreNull_ShouldReturnNullException()
        {
            //Arrange 
            RegisterRequest? registrationDetails = null;

            //Act
            Func<Task> act = async () => await _userService.RegisterUserAsync(registrationDetails);

            //Asssert
            await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("registrationDetails")
            .Where(e => e.Message.StartsWith("registration details cannot be null"));
        }

        [Fact]

        public async Task WhenAnExistingEmailIsEntered_ShouldInformUser()
        {
            //Arrange
            RegisterRequest? registrationDetails = new RegisterRequestBuilder().WithEmail("existingemail@example.com").Build();

            _mockUserRepository.CheckEmailExistsAsync(registrationDetails.Email).Returns(true);

            //Act
            Func<Task> act = async () => await _userService.RegisterUserAsync(registrationDetails);

            //Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("This email is already in use");
        }

        [Fact]
        public async Task WhenAnExistingUsernameIsEntered_ShouldInformUser()
        {
            //Arrange
            RegisterRequest? registrationDetails = new RegisterRequestBuilder().WithUsername("existingUser").Build();

            _mockUserRepository.CheckUsernameExistsAsync(registrationDetails.Username).Returns(true);

            //Act
            Func<Task> act = async () => await _userService.RegisterUserAsync(registrationDetails);

            //Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("This username is already in use");
        }

        [Fact]
        public async Task WhenAllRegistrationDataIsValid_ShouldAddUserToTheDb()
        {
            //Arrange
            RegisterRequest? registrationDetails = new RegisterRequestBuilder().Build();

            UserModel user = new UserModelBuilder().Build();

            _mockUserRepository.CheckEmailExistsAsync(registrationDetails.Email).Returns(false);

            _mockUserRepository.CheckUsernameExistsAsync(registrationDetails.Username).Returns(false);

            _mockUserRepository.AddUserAsync(Arg.Any<UserModel>()).Returns(Task.CompletedTask);

            //Act
            await _userService.RegisterUserAsync(registrationDetails);

            //Assert
            await _mockUserRepository.Received(1).
                AddUserAsync(Arg.Any<UserModel>());
        }
    }
}
