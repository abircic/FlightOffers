using FlightOffers.Migrations.Migrations;
using FlightOffers.Migrations.Models;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;
using ErrorMessages = FlightOffers.Shared.Models.Constants.ErrorMessages;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = args.FirstOrDefault(x => x.Contains("connectionString"));
        if (connectionString == null)
            throw new Exception(ErrorMessages.InvalidConnectionString);
        var extractedConnectionString = connectionString.Substring(connectionString.IndexOf("=") + 1);
        var serviceProvider = CreateServices(extractedConnectionString);
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider);
    }

    private static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Migration_2023_07_20_Create_Offer_Table).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .AddScoped<IVersionTableMetaData, VersionTableInfo>()
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}