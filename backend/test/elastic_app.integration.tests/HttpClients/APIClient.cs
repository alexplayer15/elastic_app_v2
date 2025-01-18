//using System.Net.Http;
//using Microsoft.Extensions.Http;
//using Microsoft.AspNetCore.Mvc.Testing;


//namespace elastic_app.integration.tests.HttpClients
//{
//    public class APIClient
//    {
//        private readonly HttpClient _client;
//        public UserRegistrationIntegrationTests(WebApplicationFactory<Startup> factory)
//        {
//            var httpClientFactory = factory.Services.GetRequiredService<IHttpClientFactory>();
//            _client = httpClientFactory.CreateClient("RegisterApi");
//        }
//    }
//}
