using System.Text.Json;
using FluentAssertions;
using elastic_app.common.tests.Builders;
using System.Net;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using elastic_app.application.DTOs;

namespace elastic_app.integration.tests
{
    public class ServiceIntegrationTests : IClassFixture<IntegrationTestClientFixture>
    {
        private readonly IntegrationTestClient _integrationTestClient;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private const string Register = "/api/register";

        public ServiceIntegrationTests(IntegrationTestClientFixture fixture)
        {
            _integrationTestClient = fixture._client;
            _dynamoDbClient = fixture._dynamoDbClient;
        }

        [Fact]
        public async Task WhenAUserEntersValidRegistrationDetails_ShouldReturnASuccessResponse()
        {
            //Arrange
            var registrationDetails = new RegisterRequestBuilder().WithValidRegistrationDetails(true).Build();
            var userId = await GetUserIdFromDynamoDB(registrationDetails.Username);
            if (userId != null)
            {
                await DeleteTestUserAsync(userId);
            }
         
            var registrationDetailsJson = JsonSerializer.Serialize(registrationDetails);

            _integrationTestClient
                .SetDefaultHeaders();

            //Act
            await _integrationTestClient.MakeRequestAsync(HttpMethod.Post, Register, registrationDetails);

            //Assert
            var responseStatusCode = _integrationTestClient.GetResponseStatusCode();
            responseStatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await _integrationTestClient.GetResponseContentAsync();
            var deserializedResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            deserializedResponse.Message.Should().Be("Registration successful.");
            deserializedResponse.Errors.Should().BeNull();
        }

        [Fact]
        public async Task WhenAUserEntersAnExistingUsername_ShouldThrownAnErrorAndInformTheUser()
        {
            //Arrange
            var registrationDetails = new RegisterRequestBuilder().WithExistingUsername(true).Build();

            var registrationDetailsJson = JsonSerializer.Serialize(registrationDetails);

            _integrationTestClient
                .SetDefaultHeaders();

            //Act
            await _integrationTestClient.MakeRequestAsync(HttpMethod.Post, Register, registrationDetails);

            //Assert
            var responseStatusCode = _integrationTestClient.GetResponseStatusCode();
            responseStatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await _integrationTestClient.GetResponseContentAsync();
            var deserializedResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            deserializedResponse.Errors.Should().Be("This username is already in use");
            deserializedResponse.Message.Should().BeNull();
        }

        [Fact]
        public async Task WhenAUserEntersAnExistingEmail_ShouldThrownAnErrorAndInformTheUser()
        {
            //Arrange
            var registrationDetails = new RegisterRequestBuilder().WithExistingEmail(true).Build();

            var registrationDetailsJson = JsonSerializer.Serialize(registrationDetails);

            _integrationTestClient
                .SetDefaultHeaders();

            //Act
            await _integrationTestClient.MakeRequestAsync(HttpMethod.Post, Register, registrationDetails);

            //Assert
            var responseStatusCode = _integrationTestClient.GetResponseStatusCode();
            responseStatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await _integrationTestClient.GetResponseContentAsync();
            var deserializedResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            deserializedResponse.Errors.Should().Be("This email is already in use");
            deserializedResponse.Message.Should().BeNull();
        }

        private async Task<string> GetUserIdFromDynamoDB(string username)
        {
            var request = new ScanRequest
            {
                TableName = "UserData",
                FilterExpression = "username = :u",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":u", new AttributeValue { S = username } }
                }
            };

            var response = await _dynamoDbClient.ScanAsync(request);

            return response.Items.Count > 0 ? response.Items[0]["id"].S : null;
        }

        private async Task DeleteTestUserAsync(string userId)
        {
            var tableName = "UserData";

            await _dynamoDbClient.DeleteItemAsync(new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue> { { "id", new AttributeValue { S = userId } } }
            });
        }
    }
}