namespace FlightOffers.Shared.Services.Interfaces;

public interface ITokenService
{
    Task<string> FetchAccessToken(bool forceUpdate);
}