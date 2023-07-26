using FluentMigrator;

namespace FlightOffers.Migrations.Migrations;

[Migration(20230718123456)]
public class Migration_2023_07_20_Create_Offer_Table : Migration
{
    public override void Up()
    {
        Create.Table("offer_filter").InSchema("flight_offers")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("origin_location_code").AsString()
            .WithColumn("destination_location_code").AsString()
            .WithColumn("departure_date").AsString()
            .WithColumn("return_date").AsString().Nullable()
            .WithColumn("adults").AsInt32()
            .WithColumn("currency_code").AsString();
        
        Create.Table("offer").InSchema("flight_offers")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("offer_filter_id").AsGuid().ForeignKey("fk_offer_filter","flight_offers","offer_filter","id")
            .WithColumn("extra_info").AsCustom("jsonb").Nullable();
        
        Execute.Sql(@"
-- Index for rows with NULL currency_code and return_date
CREATE UNIQUE INDEX IX_UniqueOfferFields__ReturnDateNull
ON flight_offers.offer_filter (origin_location_code, destination_location_code, departure_date, currency_code, adults)
WHERE return_date IS NULL;

-- Index for rows with NULL currency_code and return_date
CREATE UNIQUE INDEX IX_UniqueOfferFields__ReturnDateNOTNull
ON flight_offers.offer_filter (origin_location_code, destination_location_code, departure_date, currency_code,return_date, adults);

");
    }

    public override void Down()
    {
        Delete.Table("offer");
        Delete.Table("offer_filter");
    }
}