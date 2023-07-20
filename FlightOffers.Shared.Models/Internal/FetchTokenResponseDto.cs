using System.Text.Json.Serialization;

namespace FlightOffers.Shared.Models.Internal;

public class FetchTokenResponseDto
{
    [JsonPropertyName("access_token")]public string AccessToken { get; set; }
}