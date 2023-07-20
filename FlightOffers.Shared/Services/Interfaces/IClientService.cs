namespace FlightOffers.Shared.Services.Interfaces;

public interface IClientService
{
    Task<string> FetchAccessToken();
}