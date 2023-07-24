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

    public async Task<string> FetchAccessToken(bool forceUpdate)
    {
        string token;
    
        if (!forceUpdate)
        {
            _memoryCache.TryGetValue(TokenKey, out token);
            if (!string.IsNullOrEmpty(token))
                return token;
        }

        token = await _clientService.FetchAccessToken();
        if (string.IsNullOrEmpty(token))
            return "";

        SetAccessToken(token);
        return token;
    }

    #region Private

    private void SetAccessToken(string token)
    {
        _memoryCache.Set(TokenKey, token, TimeSpan.FromMinutes(20));
    }

    #endregion
}