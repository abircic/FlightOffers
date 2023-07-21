using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;

namespace FlightOffers.Shared.Services.Interfaces;

public interface IClientService
{
    Task<string> FetchAccessToken();
    Task<FetchFlightOfferResponse> FetchFlightsOffer(FetchFlightsOfferRequest request, string token);
}