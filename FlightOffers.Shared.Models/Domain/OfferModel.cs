using System.ComponentModel.DataAnnotations.Schema;
using FlightOffers.Shared.Models.Internal;

namespace FlightOffers.Shared.Models.Domain;

[Table("offer")]
public class OfferModel
{
    [Column("id")] public Guid Id { get; set; }
    [Column("client_id")] public string ClientId { get; set; }
    [Column("origin_location_code")]public string OriginLocationCode { get; set; }
    [Column("destination_location_code")]public string DestinationLocationCode { get; set; }
    [Column("departure_date")]public string DepartureDate { get; set; }
    [Column("return_date")]public string? ReturnDate { get; set; }
    [Column("adults")]public int Adults { get; set; }
    [Column("currency_code")]public string CurrencyCode { get; set; }
    [Column("extra_info", TypeName = "jsonb")]public ExtraInfo? ExtraInfo { get; set; }
    
}

public class ExtraInfo
{
    public List<Transfer> OutBoundTransfers { get; set; }
    public List<Transfer>? InBoundTransfers { get; set; }
    public decimal TotalPrice { get; set; }
}
public class Transfer
{
    public FlightLeg Departure { get; set; }
    public FlightLeg Arrival { get; set; }
}