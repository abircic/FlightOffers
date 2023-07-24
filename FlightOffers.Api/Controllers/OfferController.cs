using System.Net;
using FlightOffers.Shared.Models.Exceptions;
using FlightOffers.Shared.Models.Request;
using FlightOffers.Shared.Models.Response;
using FlightOffers.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightOffers.Api.Controllers;

[Produces("application/json")]
[Route("/api/public/[controller]/[action]")]
public class OfferController : Controller
{
    private readonly IFlightOfferService _flightOfferService;

    public OfferController(IFlightOfferService flightOfferService)
    {
        _flightOfferService = flightOfferService;
    }
    [HttpGet]
    [ProducesResponseType(typeof(FetchFlightOfferResponse),(int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.ServiceUnavailable)]
    public async Task<IActionResult> Fetch([FromQuery]FetchFlightsOfferRequest request)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException($"Invalid data supplied. Required: {string.Join(',', ModelState.Keys)}");
        
        return Ok(await _flightOfferService.GetFlightsOffer(request));
    }
}