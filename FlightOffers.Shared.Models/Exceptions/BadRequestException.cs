namespace FlightOffers.Shared.Models.Exceptions;

using System;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
