using NSubstitute;
using FluentAssertions;
using FluentValidation;
using NSubstitute.Core;
using FluentValidation.Results;
using elastic_app.application.Services.User;
using elastic_app.domain.Abstractions;
using elastic_app.domain.Models;
using elastic_app.application.DTOs;
using elastic_app.common.tests.Builders;
using System.Reflection;

namespace elastic_app.unit.tests.Services.User
{
    public class UserServiceTests
    {
        private readonly IUserRepository _mockUserRepository;
        private readonly IValidator<RegisterRequest> _mockRegisterRequestValidator;
        private readonly IUserService _userService;
        public static readonly System.ComponentModel.DataAnnotations.ValidationResult? Success;

        public UserServiceTests()
        {
            _mockUserRepository = Substitute.For<IUserRepository>();
            _mockRegisterRequestValidator = Substitute.For<IValidator<RegisterRequest>>();
            _userService = new UserService(_mockUserRepository, _mockRegisterRequestValidator);
        }

        [Fact]
        public async Task WhenRegistrationDetailsAreNull_ShouldReturnNullException()
        {
            //Arrange 
            RegisterRequest? registrationDetails = null;

            //Act
            Func<Task> act = async () => await  _userService.RegisterUserAsync(registrationDetails);

            //Asssert
            await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("registrationDetails")
            .Where(e => e.Message.StartsWith("registration details cannot be null"));
        }

        [Fact]
        public async Task WhenInvalidRegistrationDetailsAreEntered_ShouldReturnInvalidOperationException()
        {
            //Arrange
            RegisterRequest? registrationDetails = new RegisterRequestBuilder().WithPassword("invalidPassword").Build();

            var validationFailure = new ValidationFailure("Password", "Password must contain at least 2 uppercase letters and 2 numbers.");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _mockRegisterRequestValidator.ValidateAsync(Arg.Any<RegisterRequest>())
                .Returns(Task.FromResult(validationResult));

            //Act
            Func<Task> act = async () => await _userService.RegisterUserAsync(registrationDetails);

            //Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Password must contain at least 2 uppercase letters and 2 numbers.");
        }

        [Fact]

        public async Task WhenAnExistingEmailIsEntered_ShouldInformUser()
        {
            //Arrange
            RegisterRequest? registrationDetails = new RegisterRequestBuilder().WithEmail("existingemail@example.com").Build();

            ValidationResult successfulValidationResult = new ValidationResult();

            _mockRegisterRequestValidator.ValidateAsync(Arg.Any<RegisterRequest>()).Returns(successfulValidationResult);

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

            ValidationResult successfulValidationResult = new ValidationResult();

            _mockRegisterRequestValidator.ValidateAsync(Arg.Any<RegisterRequest>()).Returns(successfulValidationResult);

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

            ValidationResult successfulValidationResult = new ValidationResult();

            _mockRegisterRequestValidator.ValidateAsync(Arg.Any<RegisterRequest>()).Returns(successfulValidationResult);

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
