using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using elastic_app.application.Commands;


namespace elastic_app.integration.tests
{
    public class EmailVerificationIntegrationTests : IClassFixture<IntegrationTestClientFixture>
    {
        private readonly IntegrationTestClient _integrationTestClient;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private const string EmailVerification = "/api/verify-email";

        public EmailVerificationIntegrationTests(IntegrationTestClientFixture fixture)
        {
            _integrationTestClient = fixture._client;
            _dynamoDbClient = fixture._dynamoDbClient;
        }

        [Fact]
        public async Task VerifyEmailAsync_WhenAUserMakesARequestWithAValidToken_ThatUsersEmailVerificationShouldBeSetToTrue()
        {
            //Arrange
            var registeredToken = "registeredToken";
            var registeredUserId = "d2b19b7b-7289-4f7f-9d36-1f6a1cdcb8f6";
            _integrationTestClient.SetDefaultHeaders();

            //Act
            await _integrationTestClient.MakeRequestAsync(HttpMethod.Get, EmailVerification, new EmailVerificationCommand(registeredToken));

            //Assert
            bool isUsersEmailVerified = await CheckIfUsersEmailIsVerifiedAsync(registeredUserId);

            Assert.True(isUsersEmailVerified);
        }

        //Create common Client call file and see if we can use context here or if we have to use the client.
        //Also see if this is the best way to run these tests. 
        private async Task<bool> CheckIfUsersEmailIsVerifiedAsync(string userId)
        {
            var request = new ScanRequest
            {
                TableName = "UserData",
                FilterExpression = "userId = :u",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":u", new AttributeValue { S = userId } }
                }
            };

            var response = await _dynamoDbClient.ScanAsync(request);

            var userData = response.Items.First();

            if (userData != null && userData.ContainsKey("emailVerified"))
            {
                return userData["emailVerified"].BOOL;
            }

            return false;
        }

    }
}
