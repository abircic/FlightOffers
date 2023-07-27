using FlightOffers.Shared.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightOffers.Shared.Services.Hosted;

public class TokenHostedService : IHostedService
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<TokenHostedService> _logger;
    private Timer _timer;

    public TokenHostedService(ITokenService tokenService,ILogger<TokenHostedService> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await FetchToken(null);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("TokenHostedService has stopped working");
    }

    #region Private

    private async Task FetchToken(object state)
    {
        try
        { 
            if(_timer != null)
                await _timer.DisposeAsync();
            var token = await _tokenService.FetchAccessToken(true);
            if (String.IsNullOrEmpty(token))
                throw new Exception();
            _timer = new Timer(async o => await FetchToken(o), null, TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(20));
            
        }
        catch (Exception e)
        {
            _timer = new Timer(async o => await FetchToken(o), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }
    }
    #endregion

}