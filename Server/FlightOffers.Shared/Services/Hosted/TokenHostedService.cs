using FlightOffers.Shared.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace FlightOffers.Shared.Services.Hosted;

public class TokenHostedService : IHostedService
{
    private readonly ITokenService _tokenService;
    private Timer _timer;

    public TokenHostedService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await FetchToken(null);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
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