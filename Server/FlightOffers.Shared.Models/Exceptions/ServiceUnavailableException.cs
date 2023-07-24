namespace FlightOffers.Shared.Models.Exceptions;

public class ServiceUnavailableException: Exception
{
    public ServiceUnavailableException(string message) : base(message)
    {
    }
}
