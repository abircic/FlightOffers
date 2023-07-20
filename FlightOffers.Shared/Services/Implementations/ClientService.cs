using System.Text.Json;
using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Internal;
using FlightOffers.Shared.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FlightOffers.Shared.Services.Implementations;

public class ClientService : IClientService
{  
    static readonly HttpClient httpClient = new();
    private readonly AppSettingsModel _appSettingsModel;

    public ClientService(IOptionsMonitor<AppSettingsModel>appSettingsModel)
    {
        _appSettingsModel = appSettingsModel.CurrentValue;
    }
    public async Task<string> FetchAccessToken()
    {
        var requestUri = $"{_appSettingsModel.FlightOffersBaseUrl}{Endpoints.GetToken}";
        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _appSettingsModel.ApiKey },
            { "client_secret", _appSettingsModel.ApiSecretKey}
        };

        var content = new FormUrlEncodedContent(formData);
      
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var body = JsonSerializer.Deserialize<FetchTokenResponseDto>(responseBody);
            if (body != null)
            {
                return body.AccessToken;
            }
        }
        return "";
    }
}