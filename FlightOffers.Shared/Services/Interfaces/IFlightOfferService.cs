using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;

namespace FlightOffers.Shared.Services.Interfaces;

public interface IFlightOfferService
{
    Task<FetchFlightOfferResponse> GetFlightsOffer(FetchFlightsOfferRequest request);
}