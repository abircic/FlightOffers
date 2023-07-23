using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Domain;
using FlightOffers.Shared.Models.Internal;
using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;
using FlightOffers.Shared.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FlightOffers.Shared.Services.Implementations;

public class ClientService : IClientService
{  
    static HttpClient _httpClient = new();
    private readonly AppSettingsModel _appSettingsModel;

    public ClientService(IOptionsMonitor<AppSettingsModel>appSettingsModel)
    {
        _appSettingsModel = appSettingsModel.CurrentValue;
    }
    public async Task<string> FetchAccessToken()
    {
        var httpClient = new HttpClient();
        var requestUri = $"{_appSettingsModel.FlightOffersBaseUrl}{Endpoints.GetToken}";
        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _appSettingsModel.ApiKey },
            { "client_secret", _appSettingsModel.ApiSecretKey}
        };

        var content = new FormUrlEncodedContent(formData);
        
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);

        if (!response.IsSuccessStatusCode)
            return "";
        
        string responseBody = await response.Content.ReadAsStringAsync();
        var body = JsonSerializer.Deserialize<FetchTokenResponseDto>(responseBody);
        return body != null ? body.AccessToken : "";
    }
    
    public async Task<FetchFlightOfferResponse> FetchFlightsOffer(FetchFlightsOfferRequest request, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    
        HttpResponseMessage response = await _httpClient.GetAsync(CreateRequestUri(request));
        var responseBody = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var body = JsonSerializer.Deserialize<FetchFlightsOfferResponseDto>(responseBody);
            return body == null ? new FetchFlightOfferResponse{ IsSuccess = true } : MapToResponse(body, request);
        }
        else
        {
            var body = JsonSerializer.Deserialize<ErrorResponseDto>(responseBody);
            if (body != null && body.Errors.Any(x => x.Status == 401))
            {
                return new FetchFlightOfferResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ErrorMessages.Forbidden
                };
            }
            return new FetchFlightOfferResponse()
            {
                IsSuccess = false,
                ErrorMessage = String.Join(",", body == null ? "" : String.Join(",", body.Errors.Select(x => x.Detail)))
            };
        }
    }
    
    #region Private

    private string CreateRequestUri(FetchFlightsOfferRequest request)
    {
        var requestUri = $"{_appSettingsModel.FlightOffersBaseUrl}{Endpoints.GetFlightOffers}?";
        requestUri +=
            $"originLocationCode={request.OriginLocationCode}&" +
            $"destinationLocationCode={request.DestinationLocationCode}&" +
            $"departureDate={request.DepartureDate:yyyy-MM-dd}&" +
            $"adults={request.Adults}";
        if (request.ReturnDate != null)
            requestUri += $"&returnDate={request.ReturnDate:yyyy-MM-dd}";
        if (!String.IsNullOrEmpty(request.CurrencyCode))
            requestUri += $"&currencyCode={request.CurrencyCode}";
        return requestUri;
    }

    private FetchFlightOfferResponse MapToResponse(FetchFlightsOfferResponseDto body, FetchFlightsOfferRequest request)
    {
        return new FetchFlightOfferResponse
        {
            Data = body.Data.Select(item => new FlightInfoDto
            {
                Id = Guid.NewGuid(),
                ClientId = item.Id,
                OriginLocationCode = request.OriginLocationCode,
                DestinationLocationCode = request.DestinationLocationCode,
                DepartureDate = request.DepartureDate.ToString("yyyy-MM-dd"),
                ReturnDate = request.ReturnDate.HasValue ? request.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                Adults = request.Adults,
                NumberOfOutBoundTransfers = item.Itineraries[0].Segments.Length ,
                NumberOfInBoundTransfers = request.ReturnDate != null ? item.Itineraries[1].Segments.Length : null,
                OutBoundTransfers = item.Itineraries[0].Segments.Select(x=> new Transfer()
                {
                    Departure = x.Departure,
                    Arrival = x.Arrival
                }).ToList(),
                InBoundTransfers = request.ReturnDate != null ? item.Itineraries[1].Segments.Select(x=> new Transfer()
                {
                    Departure = x.Departure,
                    Arrival = x.Arrival
                }).ToList() : null,
                TotalPrice = Decimal.Parse(item.Price.Total,CultureInfo.InvariantCulture),
                CurrencyCode = item.Price.Currency
            }).ToList(),
            IsSuccess = true
        };
    }
    
    #endregion
}