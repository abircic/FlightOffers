using System.Text.Json.Serialization;

namespace FlightOffers.Shared.Models.Internal;

public class ErrorResponseDto
{
    [JsonPropertyName("errors")]
    public List<Error> Errors { get; set; }
}

public class Error
{

    [JsonPropertyName("detail")]
    public string Detail { get; set; }
    [JsonPropertyName("status")]
    public int Status { get; set; }
}
    