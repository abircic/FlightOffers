namespace FlightOffers.Shared.Models.Response;

public class FetchFlightOfferResponse
{
    public List<FlightInfo> Data { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}

public class FlightInfo
{
    public string Id { get; set; }
    public string OriginLocationCode { get; set; }
    public string DestinationLocationCode { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int Adults { get; set; }
    public decimal TotalPrice { get; set; }
    public string CurrencyCode { get; set; }
    public int NumberOfOutBoundTransfers { get; set; }   
    public int? NumberOfInBoundTransfers { get; set; }
}