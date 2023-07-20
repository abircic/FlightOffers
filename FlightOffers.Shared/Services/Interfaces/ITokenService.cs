namespace FlightOffers.Shared.Services.Interfaces;

public interface ITokenService
{
    Task<string> FetchAccessToken();
    string GetToken();
}