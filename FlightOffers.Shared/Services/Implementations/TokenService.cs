using FlightOffers.Shared.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FlightOffers.Shared.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IClientService _clientService;
    private const string TokenKey = "token";
    
    public TokenService(IMemoryCache memoryCache, IClientService clientService)
    {
        _memoryCache = memoryCache;
        _clientService = clientService;
    }

    public async Task<string> FetchAccessToken()
    {
        var token = await _clientService.FetchAccessToken();
        if (String.IsNullOrEmpty(token)) return "";
        SetToken(token);
        return token;
    }
    public string GetToken()
    {
        return _memoryCache.TryGetValue(TokenKey, out string key) ? key : null;
        
    }

    #region Private

    private void SetToken(string token)
    {
        _memoryCache.Set(TokenKey, token, TimeSpan.FromMinutes(30));
    }

    #endregion
}