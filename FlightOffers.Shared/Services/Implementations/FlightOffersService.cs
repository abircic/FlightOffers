using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Domain;
using FlightOffers.Shared.Models.Exceptions;
using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;
using FlightOffers.Shared.Services.Interfaces;

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
                
                var offerModels = await GetOfferFromDatabase(request);
                if (offerModels.Count > 0)
                        return MapOfferModelToResponse(offerModels);
                
                var flightsOffer =  await _clientService.FetchFlightsOffer(request, token);

                if (!flightsOffer.IsSuccess && flightsOffer.ErrorMessage == ErrorMessages.Forbidden)
                {
                        var newToken = await _tokenService.FetchAccessToken(true); 
                        flightsOffer =  await _clientService.FetchFlightsOffer(request, newToken);
                }
                
                if (!flightsOffer.IsSuccess)
                        throw new BadRequestException(flightsOffer.ErrorMessage);

                if (flightsOffer.Data.Count == 0)
                        return new FetchFlightOfferResponse() { IsSuccess = true, Data = new List<FlightInfoDto>() };
                
                await _offerRepositoryService.SaveOffers(MapResponseToOfferModel(flightsOffer.Data));
                
                return new FetchFlightOfferResponse { IsSuccess = true, Data = flightsOffer.Data };
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

        private async Task<List<OfferModel>> GetOfferFromDatabase(FetchFlightsOfferRequest request)
        {
                return await _offerRepositoryService.GetOffers(request.OriginLocationCode,
                        request.DestinationLocationCode, request.DepartureDate.ToString("yyyy-MM-dd"),
                        request.ReturnDate.HasValue ? request.ReturnDate.Value.ToString("yyyy-MM-dd") : null, request.Adults, request.CurrencyCode);
        }
        
        private FetchFlightOfferResponse MapOfferModelToResponse(List<OfferModel> offerModels)
        {
                return new FetchFlightOfferResponse()
                {
                        IsSuccess = true, 
                        Data = offerModels.Select(x=>new FlightInfoDto()
                        {
                                Id = x.Id,
                                ClientId = x.ClientId,
                                OriginLocationCode = x.OriginLocationCode,
                                DestinationLocationCode = x.DestinationLocationCode,
                                DepartureDate = x.DepartureDate,
                                ReturnDate = x.ReturnDate,
                                Adults = x.Adults,
                                CurrencyCode = x.CurrencyCode,
                                InBoundTransfers = x.ExtraInfo.InBoundTransfers,
                                OutBoundTransfers = x.ExtraInfo.OutBoundTransfers,
                                TotalPrice = x.ExtraInfo.TotalPrice,
                                NumberOfOutBoundTransfers = x.ExtraInfo.OutBoundTransfers.Count,
                                NumberOfInBoundTransfers  = x.ExtraInfo.InBoundTransfers != null ? x.ExtraInfo.InBoundTransfers.Count : null
                        }).OrderBy(x=>x.ClientId).ToList()
                };
        }
        
        private List<OfferModel> MapResponseToOfferModel(List<FlightInfoDto> flightsOffers)
        {
                return flightsOffers.Select(x => new OfferModel()
                {
                        Id = x.Id,
                        ClientId = x.ClientId,
                        OriginLocationCode = x.OriginLocationCode,
                        DestinationLocationCode = x.DestinationLocationCode,
                        Adults = x.Adults,
                        CurrencyCode = x.CurrencyCode,
                        DepartureDate = x.DepartureDate,
                        ReturnDate = x.ReturnDate,
                        ExtraInfo = new ExtraInfo()
                        {
                                OutBoundTransfers = x.OutBoundTransfers,
                                InBoundTransfers = x.InBoundTransfers,
                                TotalPrice = x.TotalPrice
                        }
                }).ToList();
        }
        
        #endregion
}