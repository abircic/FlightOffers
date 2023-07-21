using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Exceptions;
using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;
using FlightOffers.Shared.Services.Interfaces;

namespace FlightOffers.Shared.Services.Implementations;

public class FlightOffersService : IFlightOfferService
{
        private readonly ITokenService _tokenService;
        private readonly IClientService _clientService;

        public FlightOffersService(ITokenService tokenService, IClientService clientService)
        {
                _tokenService = tokenService;
                _clientService = clientService;
        }

        public async Task<FetchFlightOfferResponse> GetFlightsOffer(FetchFlightsOfferRequest request)
        {
                ValidateRequest(request);
                var token = await _tokenService.FetchAccessToken();
                if (String.IsNullOrEmpty(token))
                        throw new ServiceUnavailableException(ErrorMessages.ServiceUnavailable);
                
                var flightsOffer =  await _clientService.FetchFlightsOffer(request, token);

                if (!flightsOffer.IsSuccess)
                        throw new BadRequestException(flightsOffer.ErrorMessage);

                return flightsOffer.Data.Count <= 0 ? 
                        new FetchFlightOfferResponse() { IsSuccess = true, Data = new List<FlightInfoDto>() }
                        : new FetchFlightOfferResponse { IsSuccess = true, Data = flightsOffer.Data };
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

        #endregion
}