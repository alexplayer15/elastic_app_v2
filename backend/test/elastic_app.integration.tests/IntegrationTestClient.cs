using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Text.Json;
using elastic_app.application.DTOs;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Headers;

namespace elastic_app.integration.tests
{
    public class IntegrationTestClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessage _httpRequestMessage = new();
        private HttpResponseMessage _httpResponseMessage = new();
        public IntegrationTestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8081");
        }

        public IntegrationTestClient SetUri(string uri)
        {
            _httpRequestMessage.RequestUri = new Uri(_httpClient.BaseAddress, uri);
            return this;
        }

        public IntegrationTestClient SetMethod(HttpMethod httpMethod)
        {
            _httpRequestMessage.Method = httpMethod;
            return this;
        }

        public IntegrationTestClient SetDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return this;
        }
        public IntegrationTestClient SetRequestBody(string requestBody)
        {
            _httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            return this;
        }

        public async Task<HttpResponseMessage> MakeRequestAsync(HttpMethod method, string uri, object? body = null)
        {
            using var request = new HttpRequestMessage(method, uri);

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            _httpResponseMessage = await _httpClient.SendAsync(request);
            return _httpResponseMessage;
        }


        //public async Task MakeRequestAsync()
        //{
        //    _httpResponseMessage = await _httpClient.SendAsync(_httpRequestMessage);
        //}

        public HttpStatusCode GetResponseStatusCode()
        {
            return _httpResponseMessage.StatusCode;
        }

        public async Task<string> GetResponseContentAsync()
        {
            if (_httpResponseMessage == null)
            {
                throw new InvalidOperationException($"No response available. The response is {_httpResponseMessage}");
            }

            return await _httpResponseMessage.Content.ReadAsStringAsync();
        }
        public void Dispose()
        {
            _httpResponseMessage?.Dispose();
        }
    }
}
