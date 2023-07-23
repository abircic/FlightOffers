using FlightOffers.Shared.Models.Database;
using FlightOffers.Shared.Models.Domain;
using FlightOffers.Shared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace FlightOffers.Shared.Services.Repositories;

public class OfferRepositoryService : IOfferRepositoryService
{
    private readonly DatabaseSettingsModel _databaseSettings;

    public OfferRepositoryService(IOptionsMonitor<DatabaseSettingsModel> databaseSettings)
    {
        _databaseSettings = databaseSettings.CurrentValue;
    }
    public async Task<List<OfferModel>> GetOffers(string originLocationCode, string destinationLocationCode, string departureDate,
        string? returnDate, int adults, string? currencyCode)
    {
        using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_databaseSettings.ConnectionString))
        {
                return await databaseContext.Offer.Where(x =>
                    x.OriginLocationCode == originLocationCode &&
                    x.DestinationLocationCode == destinationLocationCode &&
                    x.DepartureDate == departureDate && x.ReturnDate == returnDate && x.Adults == adults &&
                    x.CurrencyCode == currencyCode).ToListAsync();
        }
    }

    public async Task SaveOffers(List<OfferModel> offers)
    {
        using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_databaseSettings.ConnectionString))
        {
            try
            {
                await databaseContext.Offer.AddRangeAsync(offers);
                await databaseContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException is PostgresException pSqlEx && pSqlEx.SqlState == "23505")
                {
                    return;
                }
                throw;
            }
        }
    }
}