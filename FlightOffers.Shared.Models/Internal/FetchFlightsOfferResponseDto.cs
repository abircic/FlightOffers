using System.Text.Json.Serialization;

namespace FlightOffers.Shared.Models.Internal;

public class FetchFlightsOfferResponseDto
{
    [JsonPropertyName("meta")]public Meta Meta { get; set; }
    [JsonPropertyName("data")]public FlightData[] Data { get; set; }
}

public class Meta
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
}

public class FlightData
{

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("numberOfBookableSeats")]
    public int NumberOfBookableSeats { get; set; }

    [JsonPropertyName("itineraries")]
    public Itinerary[] Itineraries { get; set; }

    [JsonPropertyName("price")]
    public Price Price { get; set; }
}

public class Itinerary
{
    [JsonPropertyName("segments")]
    public Segment[] Segments { get; set; }
}

public class Segment
{
    [JsonPropertyName("departure")]
    public FlightLeg Departure { get; set; }

    [JsonPropertyName("arrival")]
    public FlightLeg Arrival { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("numberOfStops")]
    public int NumberOfStops { get; set; }
}

public class FlightLeg
{
    [JsonPropertyName("iataCode")]
    public string IataCode { get; set; }

    [JsonPropertyName("at")]
    public DateTime At { get; set; }
}
public class Price
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("total")]
    public string Total { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; }

    [JsonPropertyName("fees")]
    public Fee[] Fees { get; set; }

    [JsonPropertyName("grandTotal")]
    public string GrandTotal { get; set; }
}

public class Fee
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
public class Currencies
{
    [JsonPropertyName("EUR")]
    public string EUR { get; set; }
}