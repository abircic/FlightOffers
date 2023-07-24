using System.Text.Json;
using FlightOffers.Shared.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlightOffers.Shared.Models.Database;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<OfferModel> Offer { get; set; }
    public DbSet<OfferFilterModel> OfferFilter { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<OfferModel>()
            .HasKey(x=>x.Id);
        builder.Entity<OfferFilterModel>()
            .HasKey(x => x.Id);
        
        builder.Entity<OfferModel>()
            .HasOne(x => x.Filter)
            .WithMany(x => x.Offers)
            .HasForeignKey(x => x.OfferFilterId);

        builder.Entity<OfferFilterModel>()
            .HasMany(x => x.Offers) 
            .WithOne(x => x.Filter) 
            .HasForeignKey(x => x.OfferFilterId); 

        
        builder.Entity<OfferModel>()
            .Property(b => b.ExtraInfo)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions{PropertyNameCaseInsensitive = true}),
                v => JsonSerializer.Deserialize<ExtraInfo>(v, new JsonSerializerOptions{PropertyNameCaseInsensitive = true}));
        builder.HasDefaultSchema("flight_offers");
        base.OnModelCreating(builder);
    }
    public static DatabaseContext GenerateContext(string connString)
    {
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.UseNpgsql(connString);
        return new DatabaseContext(builder.Options);
    }
}