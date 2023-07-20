using System.Text.Json.Serialization;
using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Services.Hosted;
using FlightOffers.Shared.Services.Implementations;
using FlightOffers.Shared.Services.Interfaces;

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
        services.AddCors();
        services.AddMemoryCache();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.Configure<AppSettingsModel>(Configuration.GetSection("AppSettings"));
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IClientService, ClientService>();
        services.AddHostedService<TokenHostedService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        
        app.UseCors(x => x
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}