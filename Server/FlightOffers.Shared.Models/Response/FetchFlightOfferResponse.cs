using FlightOffers.Shared.Models.Domain;

namespace FlightOffers.Shared.Models.Response;

public class FetchFlightOfferResponse
{
    public string OriginLocationCode { get; set; }
    public string DestinationLocationCode { get; set; }
    public string DepartureDate { get; set; }
    public string? ReturnDate { get; set; }
    public int Adults { get; set; }
    public string CurrencyCode { get; set; }
    public List<FlightInfoDto> Offers { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}

public class FlightInfoDto
{
    public Guid Id { get; set; }
    public string ClientId { get; set; }
    public List<Transfer> OutBoundTransfers { get; set; }
    public List<Transfer>? InBoundTransfers { get; set; }
    public decimal TotalPrice { get; set; }
    public int NumberOfOutBoundTransfers { get; set; }   
    public int? NumberOfInBoundTransfers { get; set; }
}