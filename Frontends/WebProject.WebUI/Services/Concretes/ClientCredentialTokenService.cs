using IdentityModel.Client;
using Microsoft.Extensions.Options;
using WebProject.DtoLayer.IdentityDtos.LoginDtos;
using WebProject.WebUI.Services.Interfaces;
using WebProject.WebUI.Settings;
public interface ITokenCache
{
    Task<string> GetTokenAsync(string key);
    Task SetTokenAsync(string key, string token, int expiresIn);
}

public class InMemoryTokenCache : ITokenCache
{
    private readonly Dictionary<string, (string Token, DateTime Expiration)> _cache = new();

    public Task<string> GetTokenAsync(string key)
    {
        if (_cache.TryGetValue(key, out var entry) && entry.Expiration > DateTime.UtcNow)
        {
            return Task.FromResult(entry.Token);
        }
        return Task.FromResult<string>(null);
    }

    public Task SetTokenAsync(string key, string token, int expiresIn)
    {
        var expiration = DateTime.UtcNow.AddSeconds(expiresIn);
        _cache[key] = (token, expiration);
        return Task.CompletedTask;
    }
}


namespace WebProject.WebUI.Services.Concretes
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly HttpClient _httpClient;
        private readonly ITokenCache _tokenCache;
        private readonly ClientSettings _clientSettings;

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, HttpClient httpClient,
            ITokenCache tokenCache, IOptions<ClientSettings> clientSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _httpClient = httpClient;
            _tokenCache = tokenCache;
            _clientSettings = clientSettings.Value;
        }

        public async Task<string> GetToken()
        {
            var token1 = await _tokenCache.GetTokenAsync("webprojecttoken");
            if (token1 != null)
            {
                return token1;
            }
            var discoveryEndPoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebProjectVisitorClient.ClientId,
                ClientSecret = _clientSettings.WebProjectVisitorClient.ClientSecret,
                Address = discoveryEndPoint.TokenEndpoint
            };

            var token2 = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);
            await _tokenCache.SetTokenAsync("webprojecttoken", token2.AccessToken, token2.ExpiresIn);
            return token2.AccessToken;
        }
    }
}

