using FlightOffers.Shared.Models.Database;
using FlightOffers.Shared.Models.Domain;
using FlightOffers.Shared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace FlightOffers.Shared.Services.Repositories;

public class OfferRepositoryService : IOfferRepositoryService
{
    private readonly DatabaseSettingsModel _databaseSettings;
    private readonly ILogger<OfferRepositoryService> _logger;


    public OfferRepositoryService(IOptionsMonitor<DatabaseSettingsModel> databaseSettings, ILogger<OfferRepositoryService> logger)
    {
        _databaseSettings = databaseSettings.CurrentValue;
        _logger = logger;
    }

    public async Task<OfferFilterModel> GetOfferFilter(string originLocationCode, string destinationLocationCode, string departureDate,
        string? returnDate, int adults, string currencyCode)
    {
        using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_databaseSettings.ConnectionString))
        {
            return await databaseContext.OfferFilter.SingleOrDefaultAsync(x =>
                x.OriginLocationCode == originLocationCode &&
                x.DestinationLocationCode == destinationLocationCode &&
                x.DepartureDate == departureDate && x.ReturnDate == returnDate && x.Adults == adults &&
                x.CurrencyCode == currencyCode);
        }
    }
    public async Task<List<OfferModel>> GetOffers(Guid filterId)
    {
        using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_databaseSettings.ConnectionString))
        {
                return await databaseContext.Offer.Where(x =>
                    x.OfferFilterId == filterId).ToListAsync();
        }
    }

    public async Task SaveOffers(OfferFilterModel offers)
    {
        using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_databaseSettings.ConnectionString))
        {
            using (var transaction = await databaseContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await databaseContext.OfferFilter.AddAsync(offers);
                    await databaseContext.Offer.AddRangeAsync(offers.Offers);
                    await databaseContext.SaveChangesAsync();
                    
                    await transaction.CommitAsync();

                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                    await transaction.RollbackAsync(); 
                    
                    if (e.InnerException != null && e.InnerException is PostgresException pSqlEx && pSqlEx.SqlState == "23505")
                    {
                        return;
                    }
                    throw;
                }
            }
        }
    }
}