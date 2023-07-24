using System.ComponentModel.DataAnnotations.Schema;
using FlightOffers.Shared.Models.Internal;

namespace FlightOffers.Shared.Models.Domain;

[Table("offer")]
public class OfferModel
{
    [Column("id")] public Guid Id { get; set; }
    [Column("offer_filter_id")] public Guid OfferFilterId { get; set; }
    [Column("extra_info", TypeName = "jsonb")]public ExtraInfo? ExtraInfo { get; set; }
    public OfferFilterModel Filter { get; set; } 
}

public class ExtraInfo
{
    public string ClientId { get; set; }
    public List<Transfer> OutBoundTransfers { get; set; }
    public List<Transfer>? InBoundTransfers { get; set; }
    public decimal TotalPrice { get; set; }
}
public class Transfer
{
    public FlightLeg Departure { get; set; }
    public FlightLeg Arrival { get; set; }
}