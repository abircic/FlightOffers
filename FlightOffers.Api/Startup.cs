using System.Text.Json.Serialization;
using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Database;
using FlightOffers.Shared.Models.Middlewares;
using FlightOffers.Shared.Services.Hosted;
using FlightOffers.Shared.Services.Implementations;
using FlightOffers.Shared.Services.Interfaces;
using FlightOffers.Shared.Services.Repositories;
using Microsoft.OpenApi.Models;

namespace FlightOffers.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options => {       
            options.JsonSerializerOptions.DefaultIgnoreCondition 
                = JsonIgnoreCondition.WhenWritingNull;;
        });
        
        services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "FlightOffers.Api"
            });
        });

        services.Configure<DatabaseSettingsModel>(Configuration.GetSection("DatabaseSettings"));
        services.AddCors();
        services.AddMemoryCache();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.Configure<AppSettingsModel>(Configuration.GetSection("AppSettings"));
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IClientService, ClientService>();
        services.AddScoped<IFlightOfferService, FlightOffersService>();
        services.AddScoped<IOfferRepositoryService, OfferRepositoryService>();
        services.AddHostedService<TokenHostedService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseRouting();
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        
        app.UseCors(x => x
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}