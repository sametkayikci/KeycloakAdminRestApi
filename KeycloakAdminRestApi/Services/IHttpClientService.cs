using System.Net.Http.Headers;
using System.Text;
using KeycloakAdminRestApi.Models.Error;

namespace KeycloakAdminRestApi.Services
{
    public interface IHttpClientService
    {
        Task<TResponse?> GetAsync<TResponse>(string url, Dictionary<string, object>? queryParams = null,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> PostJsonAsync<TRequest>(string url, TRequest requestData,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> PutAsync(string url, HttpContent content,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default);
    }

    public class HttpClientService : IHttpClientService, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = httpClientFactory.CreateClient("KeycloakApiClient");
            SetAuthorizationHeader();
        }

        private void SetAuthorizationHeader()
        {
            if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Authorization",
                    out var authorizationHeader) != null)
            {
                var accessToken = authorizationHeader
                    .ToString()
                    .Replace("Bearer ", string.Empty);
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    _client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
                }
            }
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url, Dictionary<string, object>? queryParams = null,
            CancellationToken cancellationToken = default)
        {
            if (queryParams is not null)
            {
                var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={kv.Value}"));
                url = $"{url}?{queryString}";
            }

            var response = await _client.GetAsync(url, cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var serialize = JsonSerializer.Deserialize<ErrorResponse>(responseContent);

                throw new HttpRequestException(serialize?.ToString(), null, response.StatusCode);
            }

            var responseObject = JsonSerializer.Deserialize<TResponse>(responseContent);
            return responseObject;
        }


        public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T data,
            CancellationToken cancellationToken = default)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseContent = await _client.PostAsync(url, content, cancellationToken);
            var responsesStringAsync = await responseContent.Content.ReadAsStringAsync(cancellationToken);

            if (!responseContent.IsSuccessStatusCode)
            {
                var serialize = JsonSerializer.Deserialize<ErrorResponse>(responsesStringAsync);

                throw new HttpRequestException(serialize?.ToString(), null, responseContent.StatusCode);
            }

            return responseContent;
        }


        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content,
            CancellationToken cancellationToken = default)
        {
            return await _client.PutAsync(url, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteAsync(url, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var serialize = JsonSerializer.Deserialize<ErrorResponse>(responseContent);

                throw new HttpRequestException(serialize?.ToString(), null, response.StatusCode);
            }

            return response;
        }

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}