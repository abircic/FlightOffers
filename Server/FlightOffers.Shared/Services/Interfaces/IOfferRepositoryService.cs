using FlightOffers.Shared.Models.Domain;

namespace FlightOffers.Shared.Services.Interfaces;

public interface IOfferRepositoryService
{
    Task<List<OfferModel>> GetOffers(Guid filterId);
    Task SaveOffers(OfferFilterModel offers);
    Task<OfferFilterModel> GetOfferFilter(string originLocationCode, string destinationLocationCode,
        string departureDate, string? returnDate, int adults, string currencyCode);
}