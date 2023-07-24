using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Domain;
using FlightOffers.Shared.Models.Exceptions;
using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;
using FlightOffers.Shared.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FlightOffers.Shared.Services.Implementations;

public class FlightOffersService : IFlightOfferService
{
        private readonly ITokenService _tokenService;
        private readonly IClientService _clientService;
        private readonly IOfferRepositoryService _offerRepositoryService;
        public FlightOffersService(ITokenService tokenService, IClientService clientService, IOfferRepositoryService offerRepositoryService)
        {
                _tokenService = tokenService;
                _clientService = clientService;
                _offerRepositoryService = offerRepositoryService;
        }

        public async Task<FetchFlightOfferResponse> GetFlightsOffer(FetchFlightsOfferRequest request)
        {
                ValidateRequest(request);
                
                var token = await _tokenService.FetchAccessToken(false);
                if (String.IsNullOrEmpty(token))
                        throw new ServiceUnavailableException(ErrorMessages.ServiceUnavailable);
                
                var filter = await GetOfferFilter(request);
                
                if (filter != null)
                {
                        var offerModels = await GetOfferFromDatabase(filter.Id);
                        return MapOfferModelToResponse(offerModels, request);
                }

                var flightsOffers = await FetchFlightOffersFromClient(request, token);
                
                var offers = MapResponseFromClientToOfferModel(flightsOffers.Offers, request);
                
                await _offerRepositoryService.SaveOffers(offers);
                
                return MapOfferModelToResponse(offers.Offers.ToList(), request);
        }

        #region Private

        private void ValidateRequest(FetchFlightsOfferRequest request)
        {
                if (!Enum.TryParse<CurrencyCodes>(request.CurrencyCode, out _))
                        throw new BadRequestException(ErrorMessages.InvalidCurrencyCode);
                var dateNow = DateTime.Now.Date;
                if(request.DepartureDate < dateNow || request.ReturnDate.HasValue && request.ReturnDate.Value < dateNow)
                        throw new BadRequestException(ErrorMessages.InvalidDateTime);
                if(request.ReturnDate.HasValue && request.ReturnDate.Value < request.DepartureDate)
                        throw new BadRequestException(ErrorMessages.InvalidDateChronology);
        }

        private async Task<OfferFilterModel> GetOfferFilter(FetchFlightsOfferRequest request)
        {
                return await _offerRepositoryService.GetOfferFilter(request.OriginLocationCode,
                        request.DestinationLocationCode, request.DepartureDate.ToString("yyyy-MM-dd"),
                        request.ReturnDate.HasValue ? request.ReturnDate.Value.ToString("yyyy-MM-dd") : null, 
                        request.Adults, request.CurrencyCode);
        }
        
        private async Task<List<OfferModel>> GetOfferFromDatabase(Guid id )
        {
                return await _offerRepositoryService.GetOffers(id);
        }

        private async Task<FetchFlightOfferResponse> FetchFlightOffersFromClient(FetchFlightsOfferRequest request, string token)
        {
                var flightsOffers =  await _clientService.FetchFlightsOffer(request, token);

                if (!flightsOffers.IsSuccess && flightsOffers.ErrorMessage == ErrorMessages.Forbidden)
                {
                        var newToken = await _tokenService.FetchAccessToken(true); 
                        flightsOffers =  await _clientService.FetchFlightsOffer(request, newToken);
                }
                if (!flightsOffers.IsSuccess)
                        throw new BadRequestException(flightsOffers.ErrorMessage);
                return flightsOffers;
        }

        private FetchFlightOfferResponse MapOfferModelToResponse(List<OfferModel> offerModels, FetchFlightsOfferRequest request)
        {
                return new FetchFlightOfferResponse()
                {
                        IsSuccess = true, 
                        OriginLocationCode = request.OriginLocationCode,
                        DestinationLocationCode = request.DestinationLocationCode,
                        DepartureDate = request.DepartureDate.ToString("yyyy-MM-dd"),
                        ReturnDate = request.ReturnDate.HasValue ? request.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                        Adults = request.Adults,
                        CurrencyCode = request.CurrencyCode,
                        Offers = offerModels.Select(x=>new FlightInfoDto()
                        {
                                Id = x.Id,
                                InBoundTransfers = x.ExtraInfo.InBoundTransfers,
                                OutBoundTransfers = x.ExtraInfo.OutBoundTransfers,
                                TotalPrice = x.ExtraInfo.TotalPrice,
                                NumberOfOutBoundTransfers = x.ExtraInfo.OutBoundTransfers.Count,
                                NumberOfInBoundTransfers  = x.ExtraInfo.InBoundTransfers != null ? x.ExtraInfo.InBoundTransfers.Count : null
                        }).OrderBy(x=>x.Id).ToList()
                };
        }
        
        private OfferFilterModel MapResponseFromClientToOfferModel(List<FlightInfoDto> flightsOffers, FetchFlightsOfferRequest request)
        {
                var filterId = Guid.NewGuid();
                return  new OfferFilterModel()
                {
                        Id = filterId,
                        Adults = request.Adults,
                        CurrencyCode = request.CurrencyCode,
                        DepartureDate = request.DepartureDate.ToString("yyyy-MM-dd"),
                        ReturnDate = request.ReturnDate.HasValue ? request.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                        DestinationLocationCode = request.DestinationLocationCode,
                        OriginLocationCode = request.OriginLocationCode,
                        Offers = flightsOffers.Select(x => new OfferModel()
                        {
                                Id = Guid.NewGuid(),
                                OfferFilterId = filterId,
                                ExtraInfo = new ExtraInfo
                                {
                                        ClientId = x.ClientId,
                                        OutBoundTransfers = x.OutBoundTransfers,
                                        InBoundTransfers = x.InBoundTransfers,
                                        TotalPrice = x.TotalPrice
                                }
                        }).ToList()
                };
        }
        
        #endregion
}