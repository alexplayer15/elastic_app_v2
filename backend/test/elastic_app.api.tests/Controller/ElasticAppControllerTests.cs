using FluentAssertions;
using NSubstitute;
using elastic_app.api.Controller;
using elastic_app.application.Services;
using elastic_app.common.tests.Builders;
using elastic_app.application.Services.User;
using elastic_app.application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;

namespace elastic_app.unit.tests.Controller
{
    public class ElasticAppControllerTests
    {
        
        private readonly IUserService _mockUserService;
        private readonly ElasticAppController _elasticAppController;

        public ElasticAppControllerTests()
        {
            _mockUserService = Substitute.For<IUserService>();
            _elasticAppController = new ElasticAppController(_mockUserService);
        }

        [Fact]
        public async void WhenAllRegistrationDetailsAreEnteredCorrectly_ShouldReturnRegistrationSucessfull()
        {
            //Arrange 
            var registrationData = new RegisterRequestBuilder().Build();

            var userService = _mockUserService.RegisterUserAsync(Arg.Any<RegisterRequest>())
                .Returns(Task.CompletedTask);
            //Act 
            var controllerResponse = await _elasticAppController.Register(registrationData);

            //Assert
            controllerResponse.Should().NotBeNull();
            controllerResponse.Should().BeOfType<OkObjectResult>();
            var okResult = controllerResponse as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(new { message = "Registration successful." });
        }

        [Fact]
        public async Task WhenRegistrationDetailsAreEnteredIncorrectly_ShouldReturnBadRequest()
        {
            //Arrange 
            var registrationData = new RegisterRequestBuilder().WithPassword("badpassword").WithReEnterPassword("badpassword").Build();

            var userService = _mockUserService.RegisterUserAsync(Arg.Any<RegisterRequest>())
                .Returns(Task.FromException(new InvalidOperationException("Password must contain at least 2 uppercase letters and 2 numbers.")));
            //Act 
            var controllerResponse = await _elasticAppController.Register(registrationData);

            //Assert
            controllerResponse.Should().NotBeNull();
            controllerResponse.Should().BeOfType<BadRequestObjectResult>();
            var badResult = controllerResponse as BadRequestObjectResult;
            badResult!.Value.Should().BeEquivalentTo(new { errors = "Password must contain at least 2 uppercase letters and 2 numbers." });
        }
    }
}