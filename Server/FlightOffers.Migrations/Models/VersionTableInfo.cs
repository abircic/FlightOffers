using FluentMigrator.Runner.VersionTableInfo;

namespace FlightOffers.Migrations.Models;

[VersionTableMetaData]
public class VersionTableInfo : DefaultVersionTableMetaData
{
    public override string SchemaName => "flight_offers";

    public override string TableName => "version_info";
}