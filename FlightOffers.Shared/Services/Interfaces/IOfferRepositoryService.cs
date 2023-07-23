using FlightOffers.Shared.Models.Domain;

namespace FlightOffers.Shared.Services.Interfaces;

public interface IOfferRepositoryService
{
    Task<List<OfferModel>>  GetOffers(string originLocationCode, string destinationLocationCode, string departureDate,
        string? returnDate, int adults, string? currencyCode);

    Task SaveOffers(List<OfferModel> offers);
}