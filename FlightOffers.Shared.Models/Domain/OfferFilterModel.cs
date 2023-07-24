using System.ComponentModel.DataAnnotations.Schema;

namespace FlightOffers.Shared.Models.Domain;

[Table("offer_filter")]
public class OfferFilterModel
{
    [Column("id")] public Guid Id { get; set; }
    [Column("origin_location_code")]public string OriginLocationCode { get; set; }
    [Column("destination_location_code")]public string DestinationLocationCode { get; set; }
    [Column("departure_date")]public string DepartureDate { get; set; }
    [Column("return_date")]public string? ReturnDate { get; set; }
    [Column("adults")]public int Adults { get; set; }
    [Column("currency_code")]public string CurrencyCode { get; set; }
    public ICollection<OfferModel> Offers { get; set; } 
}