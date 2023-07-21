using FluentMigrator;

namespace FlightOffers.Migrations.Migrations;

[Migration(20230718123456)]
public class Migration_2023_07_20_Create_Offer_Table : Migration
{
    public override void Up()
    {
        Create.Table("offer").InSchema("flight_offer")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("client_id").AsString()
            .WithColumn("origin_location_code").AsString()
            .WithColumn("destination_location_code").AsString()
            .WithColumn("departure_date").AsString()
            .WithColumn("return_date").AsString().Nullable()
            .WithColumn("adults").AsInt32()
            .WithColumn("currency_code").AsString()
            .WithColumn("extra_info").AsCustom("jsonb").Nullable();

        Execute.Sql(@"
-- Index for rows with NULL currency_code and return_date
CREATE UNIQUE INDEX IX_UniqueOfferFields__ReturnDateNull
ON flight_offer.offer (client_id, origin_location_code, destination_location_code, departure_date, currency_code)
WHERE return_date IS NULL;

-- Index for rows with NULL currency_code and return_date
CREATE UNIQUE INDEX IX_UniqueOfferFields_ReturnDateNotNull
ON flight_offer.offer (client_id, origin_location_code, destination_location_code, departure_date, currency_code)
WHERE return_date IS  NOT NULL;
");
    }

    public override void Down()
    {
        Delete.Table("offer");
    }
}