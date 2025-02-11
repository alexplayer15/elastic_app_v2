using FluentAssertions;
using NSubstitute;
using elastic_app.api.Controller;
using elastic_app.common.tests.Builders;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using elastic_app.application.Commands;

namespace elastic_app.unit.tests.Controller
{
    public class ElasticAppControllerTests
    {
        
        private readonly IMediator _mockMediator;
        private readonly ElasticAppController _elasticAppController;

        public ElasticAppControllerTests()
        {
            _mockMediator = Substitute.For<IMediator>();
            _elasticAppController = new ElasticAppController(_mockMediator);
        }

        [Fact]
        public async Task WhenAllRegistrationDetailsAreEnteredCorrectly_ShouldReturnRegistrationSucessfull()
        {
            //Arrange 
            var registrationData = new RegisterRequestCommandBuilder().Build();

            var userService = _mockMediator.Send(Arg.Any<RegisterRequestCommand>())
                .Returns(Task.FromResult(Unit.Value));
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
            var registrationData = new RegisterRequestCommandBuilder().WithPassword("badpassword").WithReEnterPassword("badpassword").Build();

            _mockMediator.Send(Arg.Any<RegisterRequestCommand>())
                .Returns(Task.FromException<Unit>(new InvalidOperationException("Password must contain at least 2 uppercase letters and 2 numbers.")));
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