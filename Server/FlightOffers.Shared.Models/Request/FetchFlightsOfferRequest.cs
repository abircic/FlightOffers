using System.ComponentModel.DataAnnotations;

namespace FlightOffers.Shared.Models.Request;

public class FetchFlightsOfferRequest
{
    [Required] [StringLength(3)] public string OriginLocationCode { get; set; }
    [Required] [StringLength(3)]public string DestinationLocationCode { get; set; }
    [Required] public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    [Required] public int Adults { get; set; }
    [Required] public string CurrencyCode { get; set; }
}